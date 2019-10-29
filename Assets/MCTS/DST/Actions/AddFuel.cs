using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    class AddFuel : ActionDST
    {
        public string Target;
        public float Duration;

        public AddFuel(string target) : base("AddFuel_" + target)
        {
            this.Target = target;
            this.Duration = 0.33f;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            worldModel.Cycle += this.Duration;
            worldModel.IncreaseHunger(1);
            string usedfuel = worldModel.Fuel[0].Item1;

            worldModel.RemoveFromPossessedItems(usedfuel, 1);
            worldModel.RemoveFromFuel(usedfuel);

            worldModel.Walter.Position = worldModel.GetNextPosition(this.Target, "fire");

            ActionDST action;

            if (usedfuel == "twigs")
            {
                if (!worldModel.Possesses("twigs", 2))
                {
                    worldModel.RemoveAction("Build_torch");
                    worldModel.RemoveAction("Build_pickaxe");
                }
                if (!worldModel.Possesses("twigs", 1))
                {
                    worldModel.RemoveAction("Build_axe");
                }
            }
            else if (usedfuel == "log")
            {
                if (!worldModel.Possesses("log", 2))
                { 
                    worldModel.RemoveAction("Build_firepit");
                    worldModel.RemoveAction("Build_campfire");
                }
            }            
            else if (usedfuel == "cutgrass")
            {
                if (!worldModel.Possesses("cutgrass", 3))
                {
                    worldModel.RemoveAction("Build_campfire");
                }
                if (!worldModel.Possesses("cutgrass", 2))
                {
                    action = new Build("torch");
                    worldModel.RemoveAction("Build_torch");
                }
            }
        }

        public override List<Pair<string, string>> Decompose(PreWorldState preWorldState)
        {
            int guid = preWorldState.GetEntitiesGUID(this.Target);
            int guidfuel = preWorldState.Fuel[0].Item2;

            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;

            pair = new Pair<string, string>("Action(ADDFUEL, " + guidfuel.ToString() + ", -, -, -)", guid.ToString());
            ListOfActions.Add(pair);

            return ListOfActions;
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
