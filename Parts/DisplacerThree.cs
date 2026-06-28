using System;
using XRL.Core;

using XRL.UI;

namespace XRL.World.Parts 
{
    [Serializable]
    public class DisplacerThree : IPart
    {
        public static void DisplaceBetweenFloors3()
        {
            
            Zone CurrentZone = XRLCore.Core.Game.ZoneManager.ActiveZone;
            GameObject DisplacerPanel = CurrentZone.FindObject("DisplacementPanel3");

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845654" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Fifth Floor - Biospecimen Genetic Therapeutics Overwatch Hub.", 
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845655", X: 2, Y: 11, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor five.");
                The.Player.DilationSplat();
            }

            if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845655" &&
                Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Forth Floor - Mainframe & Power Management.",
                    defaultResult: DialogResult.Cancel) == DialogResult.Yes)
            {
                The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845654", X:61, Y: 10, Device: DisplacerPanel,
                    SuccessMessage: "You arrive at floor four.");
                The.Player.DilationSplat();
            }
        }

        
    }
}