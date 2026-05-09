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
    public class GasVentVenting : IPart
    {
        public GasVentVenting()
        {
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            Object.RegisterPartEvent(this, "EndTurn");
            Object.RegisterPartEvent(this, "ObjectCreated");

            base.Register(Object, eventRegistrar);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == AfterZoneBuiltEvent.ID;
            }

            return true;
        }

        public override bool HandleEvent(AfterZoneBuiltEvent E)
        {
            var Structure = ParentObject.CurrentCell.GetAdjacentCells();

            // AddPlayerMessage("Radial Paths");

            foreach (var C in Structure)
            {
                if (C.IsEmpty())
                {
                    var Ran = Stat.Random(1, 5);
                    if (Ran == 1)
                        C.AddObject("SulfurMound1");
                }
                // AddPlayerMessage("Created Radial Structure.");
            }

            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                var GasChance = Stat.Random(1, 100);
                var GasChance2 = Stat.Random(1, 100);


                if (GasChance > 95)
                {
                    var Chance = ParentObject.CurrentCell.GetAdjacentCells();
                    foreach (var C in Chance)
                    {
                        if (GasChance2 > 97)
                        {
                            C.AddObject("SulfuricGas500");
                        }
                    }
                }
            }

            return true;
        }
    }
}