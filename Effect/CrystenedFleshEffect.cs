using System;
using XRL.Core;
using ConsoleLib.Console;
using System.Threading;
using System.Linq;
using XRL.World;
using System.Collections.Generic;
using XRL.Rules;
using XRL.World.Effects;
using XRL.Language;
using XRL.World.Capabilities;
using UnityEngine;


namespace XRL.World.Effects
{
    [Serializable]
    public class CrystenedFleshEffect : Effect

    {
        public int newSeverity;
        public int Severity = 0;
        public GameObject Owner;
        public int ArmorBonus = 1;
        public int CrysteningSeverityScale;
        public bool StatsAffected = false;

        public CrystenedFleshEffect()
        {
            base.DisplayName = "{{crystened|crystened}}";
        }

        public CrystenedFleshEffect(int Duration, GameObject Owner)
            : this()
        {
            this.Owner = Owner;
            base.Duration = Duration;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return
                    ID == EffectAppliedEvent.ID
                    || ID == GetShortDescriptionEvent.ID
                    || ID == GetDisplayNameEvent.ID;
            }

            return true;
        }

        public int LimitToRange(int value, int inclusiveMinimum, int inlusiveMaximum)
        {
            if (value >= inclusiveMinimum)
            {
                if (value <= inlusiveMaximum)
                {
                    return value;
                }

                return inlusiveMaximum;
            }

            return inclusiveMinimum;
        }

        public override int GetEffectType()
        {
            return 8;
        }

        public override bool SameAs(Effect e)
        {
            return false;
        }

        public override string GetDetails()
        {
            return
                "You've been afflicted with the crystening, your Maximum HP, Move Speed and Quickness are reduced, you gain " +
                "+" + ArmorBonus + " AV.\n\n"
                + "{{M|Crystening}} duration and severity are amplified while crystened.";
        }


        private int DigitSum(int num)
        {
            int SeveritySum = 0;
            while (num > 0)
            {
                SeveritySum += num % 10;
                num /= 10;
            }

            if (SeveritySum > 9)
            {
                SeveritySum = DigitSum(SeveritySum);
            }

            return SeveritySum;
        }

        public int SeverityFactor()
        {
            try
            {
                CrystenedFleshEffect CryFleEff = Object.GetEffect<CrystenedFleshEffect>();
                CrystniumGasEffect CryGasEff = Object.GetEffect<CrystniumGasEffect>();
                int Severity = CryGasEff.Severity;
                return DigitSum(Severity);
            }
            catch
            {
            }

            return DigitSum(10);
        }

        public int SeverityScale()
        {
            return CrysteningSeverityScale = 1 / 4;
        }

        public override bool HandleEvent(EffectAppliedEvent E)
        {
            try
            {
                GameObject ObjEffected = E.Actor;
                CrystenedFleshEffect CryFleEff = ObjEffected.GetEffect<CrystenedFleshEffect>();
                CrystniumGasEffect CryGasEff = ObjEffected.GetEffect<CrystniumGasEffect>();
                if (E.Effect == CryFleEff &&
                    (ObjEffected.HasTagOrProperty("Combat") && ObjEffected.HasTagOrProperty("Brain")))
                {
                    StatShifter.SetStatShift("AV", (1 + (SeverityFactor() * SeverityScale())));
                    StatShifter.SetStatShift("MoveSpeed", (10 + (SeverityFactor() * SeverityScale())));
                    StatShifter.SetStatShift("Speed", (-10 + (SeverityFactor() * SeverityScale())));
                    StatShifter.SetStatShift("Hitpoints", -(1 + (SeverityFactor() * SeverityScale())));
                }
            }
            catch
            {
                StatShifter.SetStatShift("AV", (1 + 1));
                StatShifter.SetStatShift("MoveSpeed", (10 + 1));
                StatShifter.SetStatShift("Speed", (-10 + 1));
                StatShifter.SetStatShift("Hitpoints", -(1 + 1));
            }

            return base.HandleEvent(E);
        }

        public override bool WantTurnTick()
        {
            ++CrysteningSeverityScale;
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("EndTurn");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                if (Object.CurrentCell.HasObjectWithBlueprint("CrystniumGas"))
                {
                    try
                    {
                        ++CrysteningSeverityScale;
                        CrystenedFleshEffect CryFleEff = Object.GetEffect<CrystenedFleshEffect>();
                        CrystniumGasEffect CryGasEff = Object.GetEffect<CrystniumGasEffect>();
                        CryGasEff.Severity += Math.Min(1, Severity / 4);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    --CrysteningSeverityScale;
                    if (CrysteningSeverityScale <= 0)
                    {
                        this.Duration = 0;
                    }
                }

                // AddPlayerMessage("Its Here.");

                if (Severity <= 25)
                {
                    DisplayName = ("{{m|minorly crystened}}");
                }
                else if (Severity <= 50)
                {
                    DisplayName = ("{{m|moderately crystened}}");
                }
                else if (Severity <= 75)
                {
                    DisplayName = ("{{M|severely crystened}}");
                }
                else if (Severity < 100)
                {
                    DisplayName = ("{{R|critically crystened}}");
                }
                else
                {
                    DisplayName = ("{{crystened|crystened}}");
                }
            }

            return base.FireEvent(E);
        }
    }
}