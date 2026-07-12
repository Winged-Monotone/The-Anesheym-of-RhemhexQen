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
    public class WallListensForDestruction : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == LeftCellEvent.ID;
            }

            return true;
        }


        public override bool HandleEvent(LeftCellEvent E)
        {
            if (E.Object.IsWall())
            {
                E.Cell.AddObject("OmniForcefield");
                AddPlayerMessage("{{yellow|ALERT!! EMERGENCY SEALING SYSTEMS ACTIVATED!}}");
                PlayWorldSound("");
            }
            return base.HandleEvent(E);
        }
        
        
    }
}