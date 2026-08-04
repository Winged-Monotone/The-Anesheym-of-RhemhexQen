using System;
using XRL.Rules;
using HistoryKit;
using XRL.World.Parts;
using XRL.World.AI.Pathfinding;
using Genkit;
using XRL.World.AI;
using System.IO;
using XRL.World.AI.GoalHandlers;
using System.Linq;
using XRL.UI.ObjectFinderClassifiers;
using UnityEngine.Rendering;

namespace XRL.World.Parts
{
    [Serializable]
    public class FleshVent : IPart
    {
        public FleshVent()
        {
            
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("EndTurn");

            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                var GasChance = Stat.Random(1, 100);
                var GasChance2 = Stat.Random(1, 100);


                if (GasChance > 90)
                {
                    var adjCells = ParentObject.CurrentCell.GetAdjacentCells();
                    foreach (var C in adjCells)
                    {
                            C.AddObject("PinkMiasma500");
                    }
                }
            }

            return true;
        }
    }
}