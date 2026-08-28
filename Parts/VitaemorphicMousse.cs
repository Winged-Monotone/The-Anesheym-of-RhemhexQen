using System;
using System.Collections.Generic;
using XRL.UI;
using XRL.Wish;
using XRL.World.Capabilities;
using XRL.World.Effects;

namespace XRL.World.Parts
{
    public class VitaemorphicMousse : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade) && ID != CanBeReplicatedEvent.ID)
            {
                return ID == InventoryActionEvent.ID;
            }
            
            return true;
        }

        public override bool HandleEvent(CanBeReplicatedEvent E)
        {
            return false;
        }
        
        public override bool HandleEvent(InventoryActionEvent E)
        {
            if (E.Command == "Apply")
            {
                if (!E.Actor.CheckFrozen(Telepathic: false, Telekinetic: true))
                {
                    return false;
                }

                if (E.Item.IsBroken() || E.Item.IsRusted())
                {
                    return E.Actor.Fail("The injector-lever is stuck.");
                }

                if (E.Actor == ThePlayer && !ThePlayer.HasIntProperty("DrankVitaeBefore"))
                {
                    if (The.Player.IsMutant())
                    {
                        Leveler.RapidAdvancement(7, ThePlayer);
                        
                        ThePlayer.AddStatBonus("Strength", 2);
                        ThePlayer.AddStatBonus("Agility", 2);
                        ThePlayer.AddStatBonus("Toughness", 2);
                        ThePlayer.AddStatBonus("Willpower", 2);
                        ThePlayer.AddStatBonus("Intelligence", 2);
                        ThePlayer.AddStatBonus("Ego", 2);

                        ThePlayer.GetStat("AP").BaseValue += 7;
                        
                        ThePlayer.GainMP(10);
                        ThePlayer.ShowSuccess(
                            "\n\nThe genetic manifolds of your being awaken to your senses like glancing at a gods' canvas just before the outline has formed. You might critique it, guide it. And maybe more. The brush strokes are yours now, what will you create with it?" 
                            + "\n\n You gain {{cyan|10}} mutation points."
                            + "\n You gain {{cyan|7}} attribute points."
                            + "\n"
                            + "\n{{green|+1 to Strength"
                            + "\n+1 to Agility"
                            + "\n+1 to Toughness"
                            + "\n+1 to Willpower"
                            + "\n+1 to Intelligence"
                            + "\n+1 to Ego}}");
                        ThePlayer.SetIntProperty("DrankVitaeBefore", 1);
                        ParentObject.Destroy();
                    }
                    if (The.Player.IsTrueKin())
                    {
                        ThePlayer.AddStatBonus("Strength", 4);
                        ThePlayer.AddStatBonus("Agility", 4);
                        ThePlayer.AddStatBonus("Toughness", 4);
                        ThePlayer.AddStatBonus("Willpower", 4);
                        ThePlayer.AddStatBonus("Intelligence", 4);
                        ThePlayer.AddStatBonus("Ego", 4);

                        ThePlayer.GetStat("AP").BaseValue += 14;
                        
                        ThePlayer.GainMP(20);
                        ThePlayer.ShowSuccess(
                            "\n\nThe genetic manifolds of your being awaken to your senses like glancing at a gods' canvas just before the outline has formed. You might critique it, guide it. And maybe more. The brush strokes are yours now, what will you create with it?" 
                            + "\n\n You gain {{cyan|10}} mutation points."
                            + "\n You gain {{cyan|7}} attribute points."
                            + "\n"
                            + "\n{{green|+1 to Strength"
                            + "\n+1 to Agility"
                            + "\n+1 to Toughness"
                            + "\n+1 to Willpower"
                            + "\n+1 to Intelligence"
                            + "\n+1 to Ego}}");
                        ThePlayer.SetIntProperty("DrankVitaeBefore", 1);
                        ParentObject.Destroy();
                    }
                }
                else if (ThePlayer.HasIntProperty("DrankVitaeBefore"))
                {
                    if (Popup.ShowYesNo(
                            "You've already injected pure vitae before, doing so again can have severe consequences. Continue?") ==
                        DialogResult.Yes)
                    {
                        E.Actor.ApplyEffect(new AmalgamConversion());
                    }
                }
            }

            return base.HandleEvent(E);
        }
    }
}