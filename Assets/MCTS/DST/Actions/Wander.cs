using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;


namespace MCTS.DST.Actions
{

    public class Wander : ActionDST
    {
        public float Duration;

        public Wander() : base("Wander")
        {
            this.Duration = 0.33f;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            worldModel.Cycle += this.Duration;
            worldModel.IncreaseHunger(1);
        }

        public override List<Pair<string, string>> Decompose(PreWorldState preWorldState)
        {
            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;

            System.Random RandomGenerator = new System.Random();

            //int index = RandomGenerator.Next(0, preWorldState.Entities.Count);
            //pair = new Pair<string, string>("Action(WALKTO, -, " + (preWorldState.Entities[index].Position.Item1 + 1).ToString() + ", " + (preWorldState.Entities[index].Position.Item2 + 1).ToString() + ", -)", "-");

            //int xIncrement = RandomGenerator.Next(-10, 10);
            //int zIncrement = RandomGenerator.Next(-10,10);

            //int posxWalter = preWorldState.Walter.Position.Item1;
            //int poszWalter = preWorldState.Walter.Position.Item2;

            //posxWalter += xIncrement;
            //poszWalter += zIncrement;

            //pair = new Pair<string, string>("Action(WALKTO, -, " + posxWalter.ToString() + ", " + poszWalter.ToString() + ", -)", "-");
            //ListOfActions.Add(pair);

            //int distance = RandomGenerator.Next(0, 50);
            //double mindistance = double.MaxValue;
            //ObjectProperties closest = new ObjectProperties();

            //foreach (var entity in preWorldState.Entities)
            //{
            //    double howclose = Math.Pow((Math.Sqrt(Math.Pow(Convert.ToDouble(entity.Position.Item1 - posxWalter), 2) + Math.Pow(Convert.ToDouble(entity.Position.Item1 - posxWalter), 2)) - distance), 2);

            //    if (howclose < mindistance)
            //    {
            //        closest = entity;
            //        mindistance = howclose;
            //    }
            //}
            //pair = new Pair<string, string>("Action(WALKTO, -, " + (closest.Position.Item1 + 1).ToString() + ", " + (closest.Position.Item2 + 1).ToString() + ", -)", "-");

            pair = new Pair<string, string>("Action(WANDER, -, -, -, -)", "-");

            ListOfActions.Add(pair);

            return ListOfActions;
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
