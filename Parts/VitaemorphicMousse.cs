using System;
using System.Collections.Generic;
using XRL.UI;
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
                    ThePlayer.GainMP(10);
                    ThePlayer.SetIntProperty("DrankVitaeBefore", 1);
                    ParentObject.Destroy();
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