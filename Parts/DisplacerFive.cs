using System;
using XRL.Core;

using XRL.UI;

namespace XRL.World.Parts 
{
    [Serializable]
    public class DisplacerFive : IPart
    {
        public static void DisplaceBetweenFloors5()
        {

            Zone CurrentZone = XRLCore.Core.Game.ZoneManager.ActiveZone;
            GameObject DisplacerPanel = CurrentZone.FindObject("DisplacementPanel5");

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845657" &&
                Popup.ShowYesNo(
                    "Interact with the displacer panel?\n Destination: Seventh Floor - Sealed Recumbancy Chambers.",
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845658", X: 21, Y: 9, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor seven.");
                The.Player.DilationSplat();
            }

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845658" &&
                Popup.ShowYesNo(
                    "Interact with the displacer panel?\n Destination: Sixth Floor [A] - Genopticon Response Hub.",
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845657", X: 2, Y: 11, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor six.");
                The.Player.DilationSplat();
            }
        }
    }
}