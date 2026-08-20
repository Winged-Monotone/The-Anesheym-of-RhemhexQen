using System;
using Genkit;
using XRL.Core;
using XRL.UI;

namespace XRL.World.Parts
{
    public class NoSpiralBoringZoneWidget : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == GetInventoryActionsEvent.ID
                   || ID == InventoryActionEvent.ID;
        }
        
        public override bool HandleEvent(InventoryActionEvent E)
        {


            return true;
        }
    }
}