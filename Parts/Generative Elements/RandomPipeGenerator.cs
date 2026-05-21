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
    public class RandomPipeGenerator : IPart
    {
        public RandomPipeGenerator()
        {
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("ObjectCreated");
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
            // AddPlayerMessage("Creating Object.");

            var TargetCell = ParentObject.CurrentZone.GetRandomCell();
            var TargetCell2 = ParentObject.CurrentZone.GetRandomCell();
            var TargetCell3 = ParentObject.CurrentZone.GetRandomCell();
            var TargetCell4 = ParentObject.CurrentZone.GetRandomCell();


            // AddPlayerMessage("Randomizing Cells.");

            FindPath Pathing1 = new FindPath(ParentObject.CurrentZone, ParentObject.CurrentCell.X,
                ParentObject.CurrentCell.Y, ParentObject.CurrentZone, TargetCell.X, TargetCell.Y, CardinalOnly: true,
                Juggernaut: true);

            FindPath Pathing2 = new FindPath(ParentObject.CurrentZone, ParentObject.CurrentCell.X,
                ParentObject.CurrentCell.Y, ParentObject.CurrentZone, TargetCell2.X, TargetCell2.Y, CardinalOnly: true,
                Juggernaut: true);

            FindPath Pathing3 = new FindPath(ParentObject.CurrentZone, ParentObject.CurrentCell.X,
                ParentObject.CurrentCell.Y, ParentObject.CurrentZone, TargetCell3.X, TargetCell3.Y, CardinalOnly: true,
                Juggernaut: true);

            FindPath Pathing4 = new FindPath(ParentObject.CurrentZone, ParentObject.CurrentCell.X,
                ParentObject.CurrentCell.Y, ParentObject.CurrentZone, TargetCell4.X, TargetCell4.Y, CardinalOnly: true,
                Juggernaut: true);

            // AddPlayerMessage("Paths Located.");

            var CenterPipeStructure = ParentObject.CurrentCell.GetAdjacentCells(2);

            // AddPlayerMessage("Radial Paths");

            foreach (var C in CenterPipeStructure)
            {
                C.AddObject("GlassHydraulicSulfurPipe");
                // AddPlayerMessage("Created Radial Structure.");
            }

            // AddPlayerMessage("Starting Pipe Structuring.");

            if (TargetCell == null)
            {
                // AddPlayerMessage("NullCell Return.");

                return false;
            }

            if (Pathing1.Usable)
            {
                foreach (var Path in Pathing1.Steps)
                {
                    Path.AddObject("GlassHydraulicSulfurPipe");
                    // AddPlayerMessage("Creating Object 1.");
                }
            }

            if (Pathing2.Usable)
            {
                foreach (var Path in Pathing2.Steps)
                {
                    Path.AddObject("GlassHydraulicSulfurPipe");
                    // AddPlayerMessage("Creating Object 2.");
                }
            }

            if (Pathing3.Usable)
            {
                foreach (var Path in Pathing3.Steps)
                {
                    Path.AddObject("GlassHydraulicSulfurPipe");
                    // AddPlayerMessage("Creating Object 3.");
                }
            }

            if (Pathing4.Usable)
            {
                foreach (var Path in Pathing4.Steps)
                {
                    Path.AddObject("GlassHydraulicSulfurPipe");
                    // AddPlayerMessage("Creating Object 4.");
                }
            }

            return true;
        }


        public override bool FireEvent(Event E)
        {
            if (E.ID == "ObjectCreated")
            {
            }

            return true;
        }
    }
}