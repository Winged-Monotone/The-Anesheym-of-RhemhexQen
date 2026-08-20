using System;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World;


namespace XRL.World.Effects
{
    [Serializable]
    public class wmIncapacitated : Effect
    {
        public int DVPenalty;

        public int SaveTarget;

        public wmIncapacitated()
        {
            base.DisplayName = "{{R|incapacitated}}";
        }

        public wmIncapacitated(int Duration, int SaveTarget)
            : this()
        {
            this.SaveTarget = SaveTarget;
            base.Duration = Duration;
        }

        public override int GetEffectType()
        {
            return TYPE_NEGATIVE;
        }

        public override bool SameAs(Effect e)
        {
            wmIncapacitated wmIncapacitated = e as wmIncapacitated;
            if (wmIncapacitated.DVPenalty != DVPenalty)
            {
                return false;
            }
            if (wmIncapacitated.SaveTarget != SaveTarget)
            {
                return false;
            }
            return base.SameAs(e);
        }

        public override string GetDetails()
        {
            if (DVPenalty < 0)
            {
                return "Unconscious, they cannot move or attack.\n" + DVPenalty + " DV";
            }
            return "Can't move or attack.\nDV set to 0.";
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.TryGetEffect(out wmIncapacitated fx))
            {
                if (Duration > fx.Duration)
                {
                    fx.Duration = Duration;
                }
                
                return false;
            }

            if (!Object.FireEvent("ApplyIncapacitate")) return false;
            
            ApplyStats();
            DidX("are", "incapacitated", "!", null, null, Object);
            Object.ParticleText("&R*KO'd!!!*");
            
            return true;
        }

        public override void Remove(GameObject Object)
        {            
            DidX("regain", "consciousness", "!", null, "G", Object);
            UnapplyStats();
            base.Remove(Object);
        }

        private void ApplyStats()
        {
            int combatDV = Stats.GetCombatDV(base.Object);
            if (combatDV > 0)
            {
                DVPenalty += combatDV;
                base.StatShifter.SetStatShift(base.Object, "DV", -DVPenalty);
            }
            else
            {
                DVPenalty = 0;
            }
        }

        private void UnapplyStats()
        {
            DVPenalty = 0;
            base.StatShifter.RemoveStatShifts(base.Object);
        }

        public override void Register(GameObject Object, IEventRegistrar registrar)
        {
            registrar.Register("BeginTakeAction");
            registrar.Register("CanChangeBodyPosition");
            registrar.Register("CanChangeMovementMod");
            registrar.Register("CanMoveExtremities");
            registrar.Register("IsMobile");
            base.Register(Object, registrar);
        }


        public override bool Render(RenderEvent E)
        {
            int num = XRLCore.CurrentFrame % 60;
            if (num > 15 && num < 30)
            {
                E.Tile = null;
                E.RenderString = "X";
                E.ColorString = "&R^r";
            }
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginTakeAction")
            {
                if (Duration > 0)
                {
                    if (base.Object.IsPlayer())
                    {
                        IComponent<GameObject>.AddPlayerMessage("You are {{r|incapacitated}}.");
                    }
                    Duration--;
                    return false;
                }
            }
            else if (E.ID == "IsMobile" || E.ID == "IsConversationallyResponsive")
            {
                if (Duration > 0)
                {
                    return false;
                }
            }
            else if (E.ID == "CanChangeBodyPosition" || E.ID == "CanChangeMovementMode" || E.ID == "CanMoveExtremities")
            {
                if (Duration > 0 && !E.HasFlag("Involuntary"))
                {
                    if (E.HasFlag("ShowMessage") && base.Object.IsPlayer())
                    {
                        Popup.Show("You are {{R|incapacitated}}!");
                    }
                    return false;
                }
            }
            
            return base.FireEvent(E);
        }
    }
}