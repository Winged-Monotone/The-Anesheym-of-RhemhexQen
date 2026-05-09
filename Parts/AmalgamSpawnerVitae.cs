using System;
using XRL.Rules;
using Genkit;
using XRL.World.AI;
using System.IO;
using XRL.World.AI.GoalHandlers;
using System.Linq;
using XRL.UI.ObjectFinderClassifiers;
using UnityEngine.Rendering;
using HistoryKit;

namespace XRL.World.Parts
{
    [Serializable]
    public class AmalgamSpawnerVitae : IPart
    {
        public int SpawnLimit = Stat.Random(1, 3);
        public int Spawn = 0;

        public AmalgamSpawnerVitae()
        {
        }


        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == EndTurnEvent.ID;
            }

            return true;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            var ChanceForAmalgam = Stat.Random(1, 100);

            if (ChanceForAmalgam <= 5)
            {
                if (Spawn < SpawnLimit)
                {
                    ParentObject.Splatter("{{red|*}}");
                    ParentObject.CurrentCell.AddObject("Amalgam" + Stat.Random(1, 5));
                    ++Spawn;
                }
                else
                {
                }
            }

            return base.HandleEvent(E);
        }
    }
}