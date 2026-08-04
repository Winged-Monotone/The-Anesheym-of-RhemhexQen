using System.Collections.Generic;
using Wingytone.MonoBehaviours;
using Wintellect.PowerCollections;
using XRL.UI;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    public class NeutronFluxDetonation : IPart
    {
        // Sets a timer based on the amount of turns the player enters. After the countdown, the device explodes, ending the game for the player if they are in the zone. If outside of it, it deactivates the displacers and makes returning to the Long Sigh Impossible. 
        // Prolly needs a options.pick
        // Explosion is prolly cosmetic only
        // Static Bool that applies to the Displacers that's like Have you completed long sigh? Have you activated beeg bomb? Then you can't go here anymore.
        // Put a warning on this that uh, messing with will fucking end you 
        
        // also, fuck you.

        public bool Primed = true;
        
        public const string PrimeDet = "Prime Detonation";
        static public bool NFDGoesBoom
        {
            get => The.Game.GetBooleanGameState("NFDGoesBoom");
            set => The.Game.SetBooleanGameState("NFDGoesBoom", value);
        }

        public static HashSet<string> Floors = new() {"JoppaWorld.33.3.1.1.29845651","JoppaWorld.33.3.1.1.29845652","JoppaWorld.33.3.1.1.29845653","JoppaWorld.33.3.1.1.29845654","JoppaWorld.33.3.1.1.2984565","JoppaWorld.33.3.1.1.29845656","JoppaWorld.33.3.1.1.29845657","JoppaWorld.33.3.1.1.29845658"};

        public int DetonationCountdown; 
        

        public static readonly List<string> MainOptions = new List<string>()
        {
            PrimeDet,
            "Cancel"
        };

        public static string GetChoice(List<string> valuesToShow)
        {
            int result = Popup.PickOption(
                Title: "Select an Option",
                Options: valuesToShow.ToArray(),
                AllowEscape: true);
            if (result < 0 || valuesToShow[result].ToUpper() == "CANCEL")
            {
                // The user escaped out of the menu, or chose "Cancel"
                return string.Empty;
            }
            else
            {
                // The user selected a value - return the select value as a string
                return valuesToShow[result];
            }
        }

        public void InitiatePriming()
        {
            // PlayWorldSound("beginmanifest");
            string mainChoice = GetChoice(MainOptions);

            if (!string.IsNullOrEmpty(mainChoice))
            {
                if (mainChoice == PrimeDet)
                {
                   var Confirmation = Popup.ShowYesNo("You get the feeling messing with this can end very badly. Continue?");
                   if (Confirmation == DialogResult.Yes)
                   {
                       var SetTimer = Popup.AskNumber("Set the countdown-sequence until detonation", Max: 300);
                       if (SetTimer != null && SetTimer > 0)
                       {
                           DetonationCountdown = SetTimer.Value;
                           // StartStrobing();
                           // RedAlarmStrobe.Enabled = true;
                       }
                   }
                   else
                   {
                       {
                           
                       }
                   }
                }
            }
        }

        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                || ID == EndTurnEvent.ID
                || ID == GetInventoryActionsEvent.ID
                || ID == InventoryActionEvent.ID;
        }
        
        public override bool HandleEvent(GetInventoryActionsEvent E)
        {
            E.AddAction("Prime", "{{red|prime}}", "CommandPrime", null, 'p');
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(InventoryActionEvent E)
        {
            if (E.Command == "CommandPrime")
            {
                InitiatePriming();
                E.RequestInterfaceExit();
            }
            
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            if (DetonationCountdown > 0)
            {
                --DetonationCountdown;
                if (DetonationCountdown <= 0)
                {
                    if (Floors.Contains(ThePlayer.CurrentZone.ZoneID))
                    {
                        if (ThePlayer.CurrentZone.ZoneID == "JoppaWorld.33.3.1.1.29845658")
                        {
                            ParentObject.Explode(70000000, Neutron: true);
                        }
                        else
                        {
                            var BoomCell = ThePlayer.CurrentZone.GetCell(x: 40, 12);
                            Physics.ApplyExplosion(BoomCell, 70000000, Neutron: true);
                            ThePlayer.Die(Reason: "Vaporised by neutron detonation.");
                        }
                    }
                    else
                    {
                        CombatJuice.cameraShake(7);
                        Popup.Show("As the bomb detonates, you feel the world around you tremble.");
                        NFDGoesBoom = true;
                    }
                }
            }
            
            return base.HandleEvent(E);
        }

        public void StrobeAction()
        {
            var mainCamera = GameManager.MainCamera;
            if (!mainCamera.GetComponent<RedAlarmStrobe>())
                GameManager.MainCamera.AddComponent<RedAlarmStrobe>();
        }

        public void StartStrobing()
        {
            GameManager.Instance.uiQueue.queueTask(StrobeAction);
        }
    }
}