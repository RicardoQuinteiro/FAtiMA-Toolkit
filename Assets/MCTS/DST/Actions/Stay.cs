using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    class Stay : ActionDST
    {
        public string Target;
        public float Duration;

        public Stay(string target) : base("Stay_" + target)
        {
            this.Target = target;
            this.Duration = 0.2f;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            worldModel.Cycle += this.Duration;
            worldModel.IncreaseHunger(2);

            worldModel.Walter.Position = worldModel.GetNextPosition(this.Target, "fire");
        }

        public override List<Pair<string, string>> Decompose(PreWorldState preWorldState)
        {
            int guid = preWorldState.GetEntitiesGUID(this.Target);

            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;

            pair = new Pair<string, string>("Action(WALKTO, -, -, -, -)", guid.ToString());
            ListOfActions.Add(pair);

            return ListOfActions;
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}