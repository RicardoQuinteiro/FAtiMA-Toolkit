﻿using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    public class Equip : ActionDST
    {
        public string Target;
        public float Duration;

        public Equip(string target) : base("Equip_" + target)
        {
            this.Target = target;
            this.Duration = 0.0f;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            worldModel.Cycle += this.Duration;

            if (this.Target == "torch")
            {
                worldModel.RemoveFromPossessedItems("torch",1);
                worldModel.AddToEquipped("torch");

                ActionDST action = new Unequip("torch");
                worldModel.AddAction(action);

                if (!worldModel.Possesses("torch"))
                {
                    worldModel.RemoveAction("Equip_torch");
                }

            }
        }

        public override List<Pair<string, string>> Decompose(PreWorldState preWorldState)
        {
                int guid = preWorldState.GetInventoryGUID(this.Target);

                List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
                Pair<string, string> pair;

                pair = new Pair<string, string>("Action(EQUIP, " + guid.ToString() + ", -, -, -)", "-");

                ListOfActions.Add(pair);

                return ListOfActions;
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
