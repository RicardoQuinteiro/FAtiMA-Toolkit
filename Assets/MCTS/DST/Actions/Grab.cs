using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;


namespace MCTS.DST.Actions
{

    public class Grab : ActionDST
    {
        public string Target;
        public float Duration;

        public Grab(string target) : base("Grab_" + target)
        {
            this.Target = target;   
            this.Duration = 0.33f;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            worldModel.Cycle += this.Duration;
            worldModel.IncreaseHunger(1);

            worldModel.Walter.Position = worldModel.GetNextPosition(this.Target, "world");

            if (this.Target == "tree")
            {
                worldModel.RemoveFromWorld("tree", 1);
                worldModel.AddToPossessedItems("log", 1);
                worldModel.AddToFuel("log", 1);

                ActionDST action;
                if (worldModel.Possesses("rocks", 12))
                {
                    action = new Build("firepit");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("cutgrass", 3))
                {
                    action = new Build("campfire");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "boulder")
            {
                worldModel.RemoveFromWorld("boulder", 1);
                worldModel.AddToPossessedItems("rocks", 2);
                worldModel.AddToPossessedItems("flint", 1);

                ActionDST action;
                if (worldModel.Possesses("log", 2) && worldModel.Possesses("rocks", 12))
                {
                    action = new Build("firepit");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("twigs", 1))
                {
                    action = new Build("axe");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("twigs", 2) && worldModel.Possesses("flint", 2))
                {
                    action = new Build("pickaxe");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "log")
            {
                worldModel.RemoveFromWorld("log", 1);
                worldModel.AddToPossessedItems("log", 1);
                worldModel.AddToFuel("log", 1);

                ActionDST action;
                if (worldModel.Possesses("log", 2) && worldModel.Possesses("cutgrass", 2))
                {
                    action = new Build("campfire");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("log", 2) && worldModel.Possesses("rocks", 12))
                {
                    action = new Build("firepit");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "twigs")
            {
                worldModel.RemoveFromWorld("twigs", 1);
                worldModel.AddToPossessedItems("twigs", 1);
                worldModel.AddToFuel("twigs", 1);

                ActionDST action;
                if(worldModel.Possesses("flint", 1))
                {
                    action = new Build("axe");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("flint", 2) && worldModel.Possesses("twigs", 2))
                {
                    action = new Build("pickaxe");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "sapling")
            {
                worldModel.RemoveFromWorld("sapling", 1);
                worldModel.AddToPossessedItems("twigs", 1);
                worldModel.AddToFuel("twigs", 1);

                ActionDST action;
                if (worldModel.Possesses("flint", 1))
                {
                    action = new Build("axe");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("flint", 2) && worldModel.Possesses("twigs", 2))
                {
                    action = new Build("pickaxe");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "flint")
            {
                worldModel.RemoveFromWorld("flint", 1);
                worldModel.AddToPossessedItems("flint", 1);

                ActionDST action;
                if (worldModel.Possesses("twigs", 1))
                {
                    action = new Build("axe");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("twigs", 2) && worldModel.Possesses("flint", 2))
                {
                    action = new Build("pickaxe");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "cutgrass")
            {
                worldModel.RemoveFromWorld("cutgrass", 1);
                worldModel.AddToPossessedItems("cutgrass", 1);
                worldModel.AddToFuel("cutgrass", 1);

                ActionDST action;
                if (worldModel.Possesses("cutgrass", 3) && worldModel.Possesses("log", 2))
                {
                    action = new Build("campfire");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("cutgrass", 2) && worldModel.Possesses("twigs", 2))
                {
                    action = new Build("torch");
                    worldModel.AddAction(action);
                }

            }
            else if (this.Target == "grass")
            {
                worldModel.RemoveFromWorld("grass", 1);
                worldModel.AddToPossessedItems("cutgrass", 1);
                worldModel.AddToFuel("cutgrass", 1);

                ActionDST action;
                if (worldModel.Possesses("cutgrass", 3) && worldModel.Possesses("log", 2))
                {
                    action = new Build("campfire");
                    worldModel.AddAction(action);
                }
                if (worldModel.Possesses("cutgrass", 2) && worldModel.Possesses("twigs", 2))
                {
                    action = new Build("torch");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "rocks")
            {
                worldModel.RemoveFromWorld("rocks", 1);
                worldModel.AddToPossessedItems("rocks", 1);

                ActionDST action;
                if (worldModel.Possesses("log", 2) && worldModel.Possesses("rocks", 12))
                {
                    action = new Build("firepit");
                    worldModel.AddAction(action);
                }
            }
            else if (this.Target == "berrybush")
            {
                worldModel.RemoveFromWorld("berrybush", 1);
                worldModel.AddToPossessedItems("berries", 2);

                ActionDST action = new Eat("berries");
                worldModel.AddAction(action);
            }
            else if (this.Target == "carrot" || this.Target == "carrot_planted")
            {
                worldModel.RemoveFromWorld("carrot", 1);
                worldModel.AddToPossessedItems("carrot", 1);

                ActionDST action = new Eat("carrot");
                worldModel.AddAction(action);
            }
            else if (this.Target == "berries")
            {
                worldModel.RemoveFromWorld("berries", 1);
                worldModel.AddToPossessedItems("berries", 1);

                ActionDST action = new Eat("berries");
                worldModel.AddAction(action);
            }
        }

        public override List<Pair<string, string>> Decompose(PreWorldState preWorldState)
        {
            if (this.Target == "tree")
            {
                int guid = preWorldState.GetEntitiesGUID("tree");

                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(2);
                Pair<string, string> pair;

                if (!preWorldState.IsEquipped("axe"))
                {
                    int guidAxe = preWorldState.GetEquippableGUID("axe");
                    pair = new Pair<string, string>("Action(EQUIP, " + guidAxe.ToString() + ", -, -, -)", "-");
                    ListOfActions.Add(pair);
                }

                pair = new Pair<string, string>("Action(CHOP, -, -, -, -)", guid.ToString());
                ListOfActions.Add(pair);

                return ListOfActions;
            }
            else if (this.Target == "boulder")
            {
                int guid = preWorldState.GetEntitiesGUID("boulder");                

                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(2);
                Pair<string, string> pair;

                if (!preWorldState.IsEquipped("pickaxe"))
                {
                    int guidPickaxe = preWorldState.GetEquippableGUID("pickaxe");
                    pair = new Pair<string, string>("Action(EQUIP, " + guidPickaxe.ToString() + ", -, -, -)", "-");
                    ListOfActions.Add(pair);
                }
                pair = new Pair<string, string>("Action(MINE, -, -, -, -)", guid.ToString());
                ListOfActions.Add(pair);

                return ListOfActions;
            }
            else if (preWorldState.EntityIsPickable(this.Target))
            {
                int guid = preWorldState.GetEntitiesGUID(this.Target);

                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
                Pair<string, string> pair;

                pair = new Pair<string, string>("Action(PICKUP, -, -, -, -)", guid.ToString());
                ListOfActions.Add(pair);

                return ListOfActions;
            }
            else if (preWorldState.EntityIsCollectable(this.Target))
            {
                int guid = preWorldState.GetEntitiesGUID(this.Target);

                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
                Pair<string, string> pair;

                pair = new Pair<string, string>("Action(PICK, -, -, -, -)", guid.ToString());
                ListOfActions.Add(pair);

                return ListOfActions;
            }            
            else
            {
                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
                Pair<string, string> pair = new Pair<string, string>("Action(PICK, -, -, -, -)", "-");
                ListOfActions.Add(pair);

                return ListOfActions;
            }
        }

        public override Pair<string, int> NextActionInfo()
        {
            if (this.Target == "berrybush")
            {
                return new Pair<string, int>("berrybush", 3);
            }           
            else if (this.Target == "tree")
            {
                return new Pair<string, int>("log", 1);
            }
            else if (this.Target == "boulder")
            {
                return new Pair<string, int>("rocks", 2);
            }
            return new Pair<string, int>("", 0);
        }
    }
}
