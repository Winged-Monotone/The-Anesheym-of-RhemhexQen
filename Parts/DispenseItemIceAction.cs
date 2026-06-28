using XRL.World;
using System;
using UnityEngine.ParticleSystemJobs;
using XRL.Core;

namespace XRL.World.Parts
{
    public class DispenseItemIceAction : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
                return base.WantEvent(ID, cascade)
                   || ID == GetInventoryActionsEvent.ID
                   || ID == InventoryActionEvent.ID;
        }
        
        public override bool HandleEvent(GetInventoryActionsEvent E)
        {

                E.AddAction("Dispense Ice", "dispense ice", "DispenseIce", null, 'y');
            
                return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(InventoryActionEvent E)
        {

                if (E.Command == "DispenseIce")
                {
                    ThePlayer.ShowSuccess("Ice-cubes fall from the hidden aperture, and into your cup.");
                }
            
                return base.HandleEvent(E);
        }
    }
}