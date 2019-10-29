using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    public class Build : ActionDST
    {
        public string Target;
        public float Duration;

        public Build(string target) : base("Build_" + target)
        {
            this.Target = target;
            this.Duration = 0.05f;
        }
        
        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            worldModel.Cycle += this.Duration;

            if (this.Target == "torch")
            {
                worldModel.RemoveFromPossessedItems("twigs", 2);
                worldModel.RemoveFromPossessedItems("cutgrass", 2);
                worldModel.AddToPossessedItems("torch", 1);                

                if (!worldModel.Possesses("twigs", 2))
                {
                    worldModel.RemoveAction("Build_pickaxe");
                }
                if (!worldModel.Possesses("twigs", 1))
                {
                    worldModel.RemoveAction("Build_axe");
                }
                if (!worldModel.Possesses("twigs", 2) || !worldModel.Possesses("cutgrass", 2))
                {
                    worldModel.RemoveAction("Build_torch");
                }
                if (worldModel.EquippedItems.Count == 0)
                {
                    ActionDST action = new Unequip("torch");
                    worldModel.AddAction(action);
                }

            }
            else if (this.Target == "pickaxe")
            {
                worldModel.RemoveFromPossessedItems("twigs", 2);
                worldModel.RemoveFromPossessedItems("flint", 2);
                worldModel.AddToPossessedItems("pickaxe", 1);

                if (!worldModel.Possesses("twigs", 2) || !worldModel.Possesses("flint", 2))
                {
                    worldModel.RemoveAction("Build_pickaxe");
                }
                if (!worldModel.Possesses("twigs", 1) || !worldModel.Possesses("flint", 1))
                {
                    worldModel.RemoveAction("Build_axe");
                }
                if (!worldModel.Possesses("twigs", 2))
                {
                    worldModel.RemoveAction("Build_torch");
                }
            }
            else if (this.Target == "axe")
            {
                worldModel.RemoveFromPossessedItems("twigs", 1);
                worldModel.RemoveFromPossessedItems("flint", 1);
                worldModel.AddToPossessedItems("axe", 1);

                if (!worldModel.Possesses("twigs",2) || !worldModel.Possesses("flint", 2))
                {
                    worldModel.RemoveAction("Build_pickaxe");
                }
                if (!worldModel.Possesses("twigs", 1) || !worldModel.Possesses("flint", 1))
                {
                    worldModel.RemoveAction("Build_axe");
                }
                if (!worldModel.Possesses("twigs", 2))
                {
                    worldModel.RemoveAction("Build_torch");
                }
            }
            else if (this.Target == "campfire")
            {
                worldModel.RemoveFromPossessedItems("log", 2);
                worldModel.RemoveFromPossessedItems("cutgrass", 3);
                worldModel.AddToWorld("campfire", 1, worldModel.Walter.Position.Item1, worldModel.Walter.Position.Item2);
                worldModel.AddToFire("campfire", worldModel.Walter.Position.Item1, worldModel.Walter.Position.Item2);

                if (!worldModel.Possesses("log", 2))
                {
                    worldModel.RemoveAction("Build_firepit");
                }
                if (!worldModel.Possesses("log", 2) || !worldModel.Possesses("cutgrass", 3))
                {
                    worldModel.RemoveAction("Build_campfire");
                }
                if (!worldModel.Possesses("cutgrass", 2))
                {
                    worldModel.RemoveAction("Build_torch");
                }
            }
            else if (this.Target == "firepit")
            {
                worldModel.RemoveFromPossessedItems("log", 2);
                worldModel.RemoveFromPossessedItems("rocks", 12);
                worldModel.AddToWorld("firepit",1, worldModel.Walter.Position.Item1, worldModel.Walter.Position.Item2);
                worldModel.AddToFire("firepit", worldModel.Walter.Position.Item1, worldModel.Walter.Position.Item2);

                if(!worldModel.Possesses("log", 2) || !worldModel.Possesses("rocks", 12))
                {
                    worldModel.RemoveAction("Build_firepit");
                }
                if (!worldModel.Possesses("log", 2))
                {
                    worldModel.RemoveAction("Build_campfire");
                }
            }
        }

        public override List<Pair<string, string>> Decompose(PreWorldState preWorldState)
        {
            if (this.Target == "campfire" || this.Target == "firepit")
            {
                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
                Pair<string, string> pair;

                Pair<int, int> position = preWorldState.Walter.Position;
                int posx = position.Item1 + 2;
                int posz = position.Item2 + 2;

                pair = new Pair<string, string>("Action(BUILD, -, " + posx.ToString() + ", " + posz.ToString() + ", campfire)", "-");
                ListOfActions.Add(pair);

                return ListOfActions;
            }
            else if (this.Target != null)
            {
                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
                Pair<string, string> pair;

                pair = new Pair<string, string>("Action(BUILD, -, -, -, " + this.Target + ")", "-");

                ListOfActions.Add(pair);

                return ListOfActions;
            }
            else
            {
                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
                Pair<string, string> pair = new Pair<string, string>("Action(BUILD, -, -, -, -)", "-");
                ListOfActions.Add(pair);

                return ListOfActions;
            }
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
