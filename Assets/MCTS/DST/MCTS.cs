using System;
using MCTS.DST.Actions;
using MCTS.DST.WorldModels;
using System.Collections.Generic;

namespace MCTS.DST
{

    public class MCTSAlgorithm
    {
        public const float C = 1.4f;
        public bool InProgress { get; private set; }
        public int MaxIterations { get; set; }
        public int MaxPlayoutDepthReached { get; private set; }
        public int MaxSelectionDepthReached { get; private set; }
        public MCTSNode BestFirstChild { get; set; }

        protected int CurrentIterations { get; set; }
        protected int CurrentDepth { get; set; }

        protected WorldModelDST CurrentState { get; set; }
        public MCTSNode InitialNode { get; set; }
        protected System.Random RandomGenerator { get; set; }

        public MCTSAlgorithm(WorldModelDST currentState)
        {
            this.InProgress = false;
            this.CurrentState = currentState;
            this.MaxIterations = 100;
            this.RandomGenerator = new System.Random();
        }

        public void InitializeMCTSearch()
        {
            this.MaxPlayoutDepthReached = 4;
            this.MaxSelectionDepthReached = 2;
            this.CurrentIterations = 0;
            this.InitialNode = new MCTSNode(this.CurrentState)
            {
                Action = null,
                Parent = null,
            };
            this.InProgress = true;
            this.BestFirstChild = null;
        }

        public ActionDST Run()
        {
            MCTSNode selectedNode;
            float reward;
            while (this.CurrentIterations < this.MaxIterations)
            {
                this.CurrentDepth = 0;
                selectedNode = this.Selection(this.InitialNode);

                if (this.CurrentDepth > this.MaxSelectionDepthReached)
                {
                    this.MaxSelectionDepthReached = this.CurrentDepth;
                }
                
                reward = this.Playout(selectedNode.State);

                if (this.CurrentDepth > this.MaxPlayoutDepthReached)
                {
                    this.MaxPlayoutDepthReached = this.CurrentDepth;
                }
                
                this.Backpropagate(selectedNode, reward);
                this.CurrentIterations++;
            }

            if (this.CurrentIterations >= this.MaxIterations - 1)
            {
                this.InProgress = false;
                return this.BestFinalAction(this.InitialNode);
            }
            else
            {
                return null;
            }
        }

        protected MCTSNode Selection(MCTSNode nodeToDoSelection)
        {
            List<ActionDST> executableActions = nodeToDoSelection.State.GetExecutableActions();
            int lenExActions;
            int randomIndex;
            List<ActionDST> expandableActions;
            List<string> executedActions;
            MCTSNode selectedNode;

            lenExActions = executableActions.Count;
            if (nodeToDoSelection.ChildNodes.Count == lenExActions)
            {
                selectedNode = BestUCTChild(nodeToDoSelection);
            }
            else
            {
                executedActions = new List<string>();

                foreach (MCTSNode node in nodeToDoSelection.ChildNodes)
                {
                    executedActions.Add(node.Action.Name);
                }

                expandableActions = new List<ActionDST>();

                foreach (ActionDST action in executableActions)
                {
                    if (!executedActions.Contains(action.Name))
                    {
                        expandableActions.Add(action);
                    }
                }

                if (expandableActions.Count != 0)
                {
                    randomIndex = this.RandomGenerator.Next(0, expandableActions.Count);
                    selectedNode = Expand(nodeToDoSelection, expandableActions[randomIndex]);
                }
                else
                {
                    selectedNode = nodeToDoSelection;
                    this.CurrentDepth = this.MaxSelectionDepthReached;
                }
            }

            this.CurrentDepth++;

            if (this.CurrentDepth < this.MaxSelectionDepthReached)
            {
                selectedNode = Selection(selectedNode);
                return selectedNode;
            }
            else
            {
                return selectedNode;
            }
        }

        protected MCTSNode Expand(MCTSNode parent, ActionDST action)
        {
            WorldModelDST newState = parent.State.GenerateChildWorldModel();
            action.ApplyActionEffects(newState);

            var child = new MCTSNode(newState)
            {
                Action = action,
                Parent = parent,
            };

            parent.ChildNodes.Add(child);

            return child;
        }

        protected float Playout(WorldModelDST initialPlayoutState)
        {
            List<ActionDST> executableActions;
            ActionDST action = null;
            int randomIndex;
            WorldModelDST state = initialPlayoutState;

            while (this.CurrentDepth < this.MaxPlayoutDepthReached)
            {
                executableActions = state.GetExecutableActions();

                if (executableActions.Count > 0)
                {
                    this.CurrentDepth++;

                    randomIndex = this.RandomGenerator.Next(0,executableActions.Count);
                    action = executableActions[randomIndex];
                    state = state.GenerateChildWorldModel();
                    action.ApplyActionEffects(state);
                }
                else
                {
                    this.CurrentDepth++;
                }
            }
            
            if (this.CurrentDepth < this.MaxPlayoutDepthReached)
            {
                this.CurrentDepth = this.MaxPlayoutDepthReached;
            }

            //Console.WriteLine("Value Heuristic:");
            //Console.WriteLine(PlayoutHeuristic(state));
            //Console.WriteLine("");

            return PlayoutHeuristic(state);
        }

        protected virtual void Backpropagate(MCTSNode node, float reward)
        {
            MCTSNode currentNode = node;

            while (currentNode.Parent != null)
            {
                currentNode.N += 1;
                currentNode.Q += reward;
                currentNode = currentNode.Parent;
            }
            currentNode.N += 1;
            return;
        }

        protected virtual MCTSNode BestUCTChild(MCTSNode node)
        {
            float UCTValue;
            float bestUCT = float.MinValue;
            MCTSNode bestNode = null;

            int i = 0;

            while (i < node.ChildNodes.Count)
            {
                UCTValue = (float)((node.ChildNodes[i].Q / node.ChildNodes[i].N) + 1.4f * Math.Sqrt(Math.Log(node.N) / node.ChildNodes[i].N));
                if (UCTValue > bestUCT)
                {
                    bestUCT = UCTValue;
                    bestNode = node.ChildNodes[i];
                }
                i++;
            }
            return bestNode;
        }

        protected ActionDST BestFinalAction(MCTSNode node)
        {
            float averageQ;
            float bestAverageQ = float.MinValue;
            MCTSNode bestNode = null;

            int i = 0;

            while (i < node.ChildNodes.Count)
            {
                averageQ = node.ChildNodes[i].Q / node.ChildNodes[i].N;
                if (averageQ > bestAverageQ)
                {
                    bestAverageQ = averageQ;
                    bestNode = node.ChildNodes[i];
                }
                i++;
            }
            return bestNode.Action;
        }

        protected float PlayoutHeuristic(WorldModelDST state)
        {
            int hp = state.Walter.HP;
            float cHP = Convert.ToSingle(hp);
            //Console.WriteLine(hp);
            //Console.WriteLine(cHP);

            int hunger = state.Walter.Hunger;
            float cHunger = Convert.ToSingle(hunger);

            //Console.WriteLine("Is it Night?");
            //Console.WriteLine(Convert.ToSingle(13 - state.CycleInfo[2]) <= (state.Cycle));
            //Console.WriteLine("");

            if (cHP <= 0)
            {
                return 0.0f;
            }
            else if (Convert.ToSingle(15 - state.CycleInfo[2]) <= (state.Cycle))
            {
                //Console.WriteLine("light");
                //Console.WriteLine(state.LightValueNight());
                //Console.WriteLine(state.FoodValue());
                //Console.WriteLine("");
                return (state.LightValueNight())*0.9f + state.FoodValue()*0.1f;
            }
            else
            {
                //float reward = ((cHP/150) + (cHunger/150)*4 + (state.LogsValue())*2 + (state.RocksValue())*2 + (state.TorchValueDay())*10 + (state.AxePickaxeValue())*6 + (state.FoodValue())*7)/32;
                float reward = (state.LightValueDay())*0.7f + (state.FoodValue())*0.3f;
                return reward;
            }
        }

        public MCTSNode InitialNodeInfo()
        {
            return this.InitialNode;
        }
    }
}
