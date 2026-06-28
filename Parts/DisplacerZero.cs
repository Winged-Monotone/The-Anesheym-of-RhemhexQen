using System;
using XRL.Core;

using XRL.UI;

namespace XRL.World.Parts 
{
    [Serializable]
    public class DisplacerZero : IPart
    {
        public static void DisplaceBetweenFloors0()
        {
            
                Zone CurrentZone = XRLCore.Core.Game.ZoneManager.ActiveZone;
                GameObject DisplacerPanel = CurrentZone.FindObject("DisplacementPanel0");

                if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.13" &&
                    Popup.ShowYesNo("Interact with the displacer panel?\n Destination: First Floor Hub - Check-In & Security.", 
                        defaultResult: DialogResult.Cancel) == DialogResult.Yes)
                {
                    The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.29845651", X: 40, Y: 4, Device: DisplacerPanel,
                        SuccessMessage: "You arrive at The Long Sigh.");
                    The.Player.DilationSplat();
                    The.Player.PlayWorldSound("facilitysiren.mp3");
                }

                if (CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845651" &&
                    Popup.ShowYesNo("Interact with the displacer panel?\n Destination: Clientele and Staffing Department.",
                        defaultResult: DialogResult.Cancel) == DialogResult.Yes)
                {
                    The.Player.ZoneTeleport("JoppaWorld.33.3.1.1.13", X: 23, Y: 20, Device: DisplacerPanel,
                        SuccessMessage: "You arrive at Rhemhex'Qen.");
                    The.Player.DilationSplat();
                }
        }
    }
}