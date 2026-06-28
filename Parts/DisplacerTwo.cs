using System;
using XRL.Core;

using XRL.UI;

namespace XRL.World.Parts 
{
    [Serializable]
    public class DisplacerTwo : IPart
    {
        public static void DisplaceBetweenFloors2()
        {
            
            Zone CurrentZone = XRLCore.Core.Game.ZoneManager.ActiveZone;
            GameObject DisplacerPanel = CurrentZone.FindObject("DisplacementPanel2");

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845652" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Third Floor Hub - Production and Manufacturing.", 
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845653", X: 75, Y: 13, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor three.");
                The.Player.DilationSplat();
            }

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845653" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Second Floor Hub - Serenity Complex.",
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845652", X:40, Y: 21, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor two.");
                The.Player.DilationSplat();
            }
        }

        
    }
}