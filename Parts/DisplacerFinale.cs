using System;
using XRL.Core;

using XRL.UI;

namespace XRL.World.Parts 
{
    [Serializable]
    public class DisplacerFinale : IPart
    {
        public static void DisplaceBetweenFloorsFinale()
        {
            
            Zone CurrentZone = XRLCore.Core.Game.ZoneManager.ActiveZone;
            GameObject DisplacerPanel = CurrentZone.FindObject("DisplacementPanel5");

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845658" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Clientele and Staffing Department.", 
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.13", X: 25, Y: 20, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at Rhemhex'Qen.");
                The.Player.DilationSplat();
            }

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.13" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Seventh Floor - Sealed Recumbancy Chambers.",
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845658", X:21, Y: 9, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor seven of the Long Sigh.");
                The.Player.DilationSplat();
            }
        }

        
    }
}