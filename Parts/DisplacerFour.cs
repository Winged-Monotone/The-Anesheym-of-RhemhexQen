using System;
using XRL.Core;

using XRL.UI;

namespace XRL.World.Parts 
{
    [Serializable]
    public class DisplacerFour : IPart
    {
        public static void DisplaceBetweenFloors4()
        {
            
            Zone CurrentZone = XRLCore.Core.Game.ZoneManager.ActiveZone;
            GameObject DisplacerPanel = CurrentZone.FindObject("DisplacementPanel3");

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845656" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Sixth Floor [A] - Penopticon Response Hub.", 
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845657", X: 77, Y: 11, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor six.");
                The.Player.DilationSplat();
            }

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845657" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Fifth Floor - Biospecimen Genetic Therapeutics.",
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845656", X:79, Y: 6, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor five.");
                The.Player.DilationSplat();
            }
        }

        
    }
}