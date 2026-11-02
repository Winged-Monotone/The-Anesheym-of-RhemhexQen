using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using Qud.API;
using XRL.Language;
using XRL.UI;
using XRL.World.Parts;

namespace XRL.World.Conversations.Parts
{
    public class DitzyFlyGiveEggs : IConversationPart
    {
        public override bool WantEvent(int ID, int Propagation)
        {
            if (!base.WantEvent(ID, Propagation) && ID != GetChoiceTagEvent.ID && ID != EnterElementEvent.ID)
            {
                return ID == HideElementEvent.ID;
            }
            return true;
        }
        
        public override bool HandleEvent(GetChoiceTagEvent E)
        {
            E.Tag = "{{g|[Give Eggs]}}";
            return false;
        }
        
        public static bool IsAwardableEgg(GameObject Object)
        {
            if (Object.HasTagOrIntProperty("EgghuntItem"))
            {
                return true;
            }
            return false;
        }

        public static int EggRepValue(GameObject Egg)
        {
	        if (!Egg.IsValid())
	        {
		        return 0;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT1"))
	        {
		        return 10;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT2"))
	        {
		        return 20;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT3"))
	        {
		        return 55;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT4"))
	        {
		        return 65;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT5"))
	        {
		        return 155;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT6"))
	        {
		        return 305;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT7"))
	        {
		        return 500;
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT8"))
	        {
		        return 2000;
	        }
	        else
		        return 0;
        }
        
        public static string EggToTable(GameObject Egg)
        {
       
	        if (!Egg.IsValid())
		        return null;
	        if (Egg.HasPropertyOrTag("EggHuntItemT1"))
	        {
		        return "wmTier1EggRewards";
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT2"))
	        {
		        return "wmTier2EggRewards";
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT3"))
	        {
		        return "wmTier3EggRewards";
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT4"))
	        {
		        return "wmTier4EggRewards";
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT5"))
	        {
		        return "wmTier5EggRewards";
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT6"))
	        {
		        return "wmTier6EggRewards";
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT7"))
	        {
		        return "wmTier7EggRewards";
	        }
	        else if (Egg.HasPropertyOrTag("EggHuntItemT8"))
	        {
		        return "wmTier8EggRewards";
	        }
        
	        return "wmTier1EggRewards";
        }
        
        public override bool HandleEvent(EnterElementEvent E)
		{
			Inventory eInventory = The.Player.Inventory;

			List<GameObject> Rewards = new List<GameObject>();
			
			// Cumulative list of eggs. Including their values, image renders, added count, etc.
			List<string> EggDisplayNames = new List<string>();
			
			List<GameObject> DefinedEggs = new List<GameObject>();
			List<int> EggValue = new List<int>();
			
			List<int> EggCount = new List<int>();
			List<IRenderable> EggItemRenders = new List<IRenderable>();
			
			bool Important = false;
			
			foreach (GameObject @object in eInventory.GetObjects(IsAwardableEgg))
			{
				if (@object.IsMarkedImportantByPlayer())
				{
					Important = true;
					continue;
				}
				int eggRepValue = EggRepValue(@object);
				if (eggRepValue > 0)
				{
					DefinedEggs.Add(@object);
					
					EggValue.Add(eggRepValue);
					EggCount.Add(@object.Count);
					
					EggItemRenders.Add(@object.Render);
					
					EggDisplayNames.Add(@object.GetDisplayName(1120) + " [{{G|+" + eggRepValue + "}} Reputation]");
				}
			}
			
			if (DefinedEggs.Count == 0)
			{
				return The.Player.ShowFailure(Important ? "You only have books you've marked important. Unmark any you wish to donate." : "You have no books to give.");
			}
			
			List<(int, int)> Choices = Popup.PickSeveral(
				Title:"Choose eggs to give.",
				Options: EggDisplayNames, 
				Hotkeys: HotkeySpread.get(new string[2] { "Menus", "UINav" }), 
				Stacks: EggCount, 
				Icons: EggItemRenders, 
				Context: The.Speaker,
				AllowEscape: true);
			
			if (Choices.IsNullOrEmpty())
			{
				return false;
			}
			
			int TotalValues = 0;
			
			var actionCapProvide = The.Speaker.Does(
				Verb: "provide",
				NoConfusion: true,
				Stripped: true,
				WithoutTitles: true,
				BaseOnly: true
			);
			
			var nameWithIndefiniteArticle = The.Speaker.an(
				NoConfusion: true,
				Stripped: true,
				WithoutTitles: true,
				BaseOnly: true
			);
			
			foreach (var Pair in Choices)
			{
				GameObject Egg = DefinedEggs[Pair.Item1];
				int Amount = Pair.Item2;
				if (Egg.ConfirmUseImportant(The.Player, "return", null, Amount))
				{
					var Table = EggToTable(Egg);
					for (int i = 0; i < Amount; i++)
					{
						Rewards.Add(PopulationManager.CreateOneFrom(Table));
					}
					
					Egg.SplitStack(Amount, The.Player);
					GameObject speaker = The.Speaker;
					if (speaker != null && speaker.ReceiveObject(Egg))
					{
						TotalValues += EggValue[Pair.Item1] * Amount;
						
					}
				}
			}
			
			SoundManager.PlayUISound("Sounds/Interact/sfx_interact_book_turnIn");
			
			Popup.Show("You gain {{C|" + TotalValues + "}} Reputation with the Anesheym of Rhemhex'Qen.", null, "Sounds/UI/ui_notification", CopyScrap: true, Capitalize: true, DimBackground: true, LogMessage: false);
			
			The.Game.PlayerReputation.Modify(
				Faction: "Anesheym",
				Amount: TotalValues,
				Type: "AnesheymEggReturn");

			foreach (var Reward in Rewards)
				Reward.MakeUnderstood();
			
			Popup.Show($"You receive {Grammar.MakeAndList(Rewards)}!");

			foreach (var Reward in Rewards)
				The.Player.ReceiveObject(Reward);
			
			
			return base.HandleEvent(E);
		}
    }
}