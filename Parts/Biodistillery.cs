using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Wingytone.MonoBehaviours;
using XRL.UI;
using XRL.Wish;

namespace XRL.World.Parts
{
    [HasWishCommand]
    public class Biodistillery : IPart
    {
        // Interact with this, and it brings up options to synthezise purified vitae
        // Requires seven ingredients mixtures, all set to specific values that the player finds throughout the dungeon in Meh's scattered notes. 
        // If done correctly, they create purified vitae, a consumable that gives them mutation points.
        // If done incorrectly, it ends the game as its implied you have started a Second Crimson Tide, which Qud simply Couldn't recover from, and you essentially doom the world. 

        public const int Dynestrogen = 0;
        public const int Mychoglobulin = 1;
        public const int Ultraglycocyn = 2;
        public const int Zetanutriena = 3;
        public const int Psycholuceum = 4;
        public const int Xaldazium = 5;
        public const int Arsplicium = 6;

        public float[] Values = new float[7];

        public static readonly List<string> MainOptions = new List<string>()
        {
            "Synthesize a Compound",
            "Cancel"
        };

        public static readonly List<string> SecondaryOptions = new List<string>()
        {
            "Set Synthesis Levels",
            "Commence Synthesis",
            "Cancel"
        };

        public static readonly List<string> ThirdOptions = new List<string>()
        {
            "{{red|Dynestrogen}}",
            "{{orange|Mychoglobulin}}",
            "{{yellow|Ultraglycocyn}}",
            "{{green|Zetanutriena}}",
            "{{cyan|Psycholuceum}}",
            "{{blue|Xaldazium}}",
            "{{magenta|Arsplicium}}",
            "Back"
        };

        // fuck you
        public static readonly List<float> CorrectValues = new List<float>()
        {
            74.3f,
            69.3f,
            32.0f,
            25.5f,
            13.1f,
            7.7f,
            0.1f
        };

        public static int GetChoice(List<string> valuesToShow)
        {
            int result = Popup.PickOption(
                Title: "Select an Option",
                Options: valuesToShow.ToArray(),
                AllowEscape: true);

            return result;
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
            E.AddAction("Synthesize Compounds", "{{yellow|synthesize compounds}}", "CommandSynthesizeCompounds", null,
                's');
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(InventoryActionEvent E)
        {
            if (E.Command == "CommandSynthesizeCompounds")
            {
                InitiateSynthesis();
                E.RequestInterfaceExit();
            }

            return base.HandleEvent(E);
        }

        public void InitiateSynthesis()
        {
            // PlayWorldSound("beginmanifest");
            int mainChoice = GetChoice(MainOptions);

            if (mainChoice != -1)
            {
                if (mainChoice == 0)
                {
                    var FirstConfirmation =
                        Popup.ShowYesNo(
                            "{{yellow|You get the feeling messing with this can end very badly. Continue?}}");

                    if (FirstConfirmation == DialogResult.Yes)
                    {
                        ContinueSynthesis();
                    }
                }
            }
        }

        public void ContinueSynthesis()
        {
            int secondaryChoice;

            while (true)
            {
                secondaryChoice = GetChoice(SecondaryOptions);

                if (secondaryChoice == -1 || secondaryChoice == 2)
                {
                    break;
                }

                if (secondaryChoice == 0)
                {
                    SuperContinueSynthesis();
                }

                if (secondaryChoice == 1)
                {
                    // if the player does this right, then they will get the purified vitae, if they did this wrong, THEY FUCKING DIEEE!!!
                    // also fuck you

                    SuperDuperFinishSynthesis();
                    break;
                }
            }
        }

        public void SuperContinueSynthesis()
        {
            // Start gathering synthesis inputs from player

            int thirdChoice;

            while (true)
            {
                thirdChoice = GetChoice(ThirdOptions);

                if (thirdChoice == -1 || thirdChoice > Arsplicium)
                {
                    break;
                }

                // get choice is returning the index of the ingredient
                // value is the amount that was entered, the display name is access by the option index. example, 0 = dynestrogen

                var DisplayName = ThirdOptions[thirdChoice];
                var Value = Values[thirdChoice];

                // Ask the player to set a value, which is then added to the Value variable and then uses ToString to convert it to a string
                // F1 is the string format, how its supposed ot look once converted.
                var GetValue = Popup.AskString("Set " + DisplayName + " Dosage.", Value.ToString("F1"));

                if (GetValue.IsNullOrEmpty())
                {
                    // is it null? fuck it.
                    continue;
                }

                // try to convert the users input into a float
                if (float.TryParse(GetValue, out var result))
                {
                    // sets the value at the index of your choice
                    Values[thirdChoice] = result;
                }
            }
        }
        
        
        public static void FadeInAction()
        {
            var mainCamera = GameManager.MainCamera;
            var Strobe = mainCamera.GetComponent<CrimsonTide>();

            if (!Strobe)
            {
                Strobe = GameManager.MainCamera.AddComponent<CrimsonTide>();
            }
            
            Strobe.Game = new (The.Game);
        }

        [WishCommand("StartCrimsonTide")]
        public static void StartCrimsonTide()
        {
            CrimsonTide.Enabled = true;
            GameManager.Instance.uiQueue.queueTask(FadeInAction);  
        }
        
        
        public void SuperDuperFinishSynthesis()
        {
            var YouFuckedUp = false;

            for (int i = 0; i < Values.Length; i++)
            {
                if (!Mathf.Approximately(Values[i], CorrectValues[i]))
                {
                    // Player fucks up, and dies
                    YouFuckedUp = true;
                    break;
                }
            }

            if (YouFuckedUp)
            {
                //     Consider some kind of animation or something that visually represents the Crimson Tide being unleashed on the world

                StartCrimsonTide();
                The.Core.RenderDelay(7000, Interruptible: false);
                ThePlayer.Die(
                    Reason: "You've invoked the Crimson Tide, devastating Qud with an eighth plague of the Gyre.");
                // CrimsonTide.Enabled = false;
            }
            else
            {
                ThePlayer.ShowSuccess(
                    "The distiller hums as it dispenses a glittering substance into a sterilized injector.");
                SoundManager.PlaySound("sfx_interact_artifact_abort_still", Volume: 1);
                ThePlayer.ReceiveObject("Purified Vitae Injector");
                ParentObject.RemovePart(this);
            }
            
            
        }
    }
}