using System;
using XRL.Core;

using XRL.UI;

namespace XRL.World.Parts 
{
    [Serializable]
    public class DisplacerOne : IPart
    {
        public static void DisplaceBetweenFloors1()
        {
            
                
            
                Zone CurrentZone = XRLCore.Core.Game.ZoneManager.ActiveZone;
                GameObject DisplacerPanel = CurrentZone.FindObject("DisplacementPanel1");

                if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845651" &&
                    Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Second Floor Hub - Serenity Complex.", 
                        defaultResult: DialogResult.Cancel) == DialogResult.Yes)
                {
                    The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845652", X: 40, Y: 22, Device: DisplacerPanel,
                        SuccessMessage: "You arrive at floor two.");
                    The.Player.DilationSplat();
                }

                if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845652" &&
                    Popup.ShowYesNo("Interact with the displacer panel?\n Destination: First Floor Hub - Check-In & Security.",
                        defaultResult: DialogResult.Cancel) == DialogResult.Yes)
                {
                    The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845651", X:40, Y: 21, Device: DisplacerPanel,
                        SuccessMessage: "You arrive at floor one.");
                    The.Player.DilationSplat();
                }
        }

        
    }
}