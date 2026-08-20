using XRL.World;
using System;
using UnityEngine.ParticleSystemJobs;
using XRL.Core;

namespace XRL.World.Parts
{
    public class DispenseItemBajaBlastAction : IPart
    {
        public bool Dispensed = false;
        public override bool WantEvent(int ID, int cascade)
        {
                return base.WantEvent(ID, cascade)
                   || ID == GetInventoryActionsEvent.ID
                   || ID == InventoryActionEvent.ID;
        }
        
        // XRL.World.Parts.DeployableInfrastructure
        public override bool HandleEvent(GetInventoryActionsEvent E)
        {

                E.AddAction("Dispense Baja-Blast", "dispense baja-blast", "DispenseBaja", null, 'y');
                
                return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(InventoryActionEvent E)
        {
            
                if (!Dispensed && E.Command == "DispenseBaja")
                {
                    E.Actor.GiveDrams(10, "bajablast");
                    ThePlayer.ShowSuccess("You receive ten drams of baja-blast.");
                    Dispensed = true;
                }
                else
                {
                    ThePlayer.ShowFailure("The machine dryly sputters to a stop.");
                }
            
                return base.HandleEvent(E);
        }
    }
}