using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    public class Unequip : ActionDST
    {
        public string Target;
        public float Duration;

        public Unequip(string target) : base("Unequip_" + target)
        {
            this.Target = target;
            this.Duration = 0.0f;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            worldModel.Cycle += this.Duration;

            if (this.Target == "torch")
            {                
                worldModel.RemoveFromEquipped("torch");
                worldModel.AddToPossessedItems("torch", 1);

                ActionDST action = new Equip("torch");
                worldModel.AddAction(action);
            }
        }

        public override List<Pair<string, string>> Decompose(PreWorldState preWorldState)
        {
            if (this.Target != null)
            {
                int guid = preWorldState.GetEquippedGUID(this.Target);

                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>();
                Pair<string, string> pair;

                pair = new Pair<string, string>("Action(UNEQUIP, " + guid.ToString() + ", -, -, -)", "-");

                ListOfActions.Add(pair);

                return ListOfActions;
            }
            else
            {
                List<Pair<string, string>> list = new List<Pair<string, string>>(1);
                Pair<string, string> pair = new Pair<string, string>("Action(UNEQUIP, -, -, -, -)", "-");
                list.Add(pair);

                return list;
            }
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
