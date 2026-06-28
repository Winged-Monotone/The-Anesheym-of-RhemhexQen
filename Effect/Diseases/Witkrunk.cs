using Qud.API;
using System;
using XRL;
using XRL.Language;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using System.Collections.Generic;
using ConsoleLib.Console;
using XRL.Core;

namespace XRL.World.Effects
{
    [Serializable]
    public class Witkrunk : Effect
    {
        private string Population;

        public bool triggered;
        

        public Witkrunk()
        {
            base.DisplayName = "{{M|Witkrunk}}";
        }

        public Witkrunk(int Duration, string Population = "MutatingResults")
            : this()
        {
            base.Duration = Duration;
            this.Population = Population;
        }

        public Witkrunk(int Duration, int PermuteMutationsAt, string Population = "MutatingResults")
            : this(Duration, Population)
        {
            
        }

        public override int GetEffectType()
        {
            return 4;
        }

        public override bool SameAs(Effect e)
        {
            Witkrunk mutating = e as Witkrunk;
            
            return base.SameAs(e);
        }

        public override string GetDetails()
        {
            return "Your mind feels addled, and your head feels lighter; your brain has become like pumice. Your mental attributes have been reduced, every month, you must save or lose -1 to your mental attributes, if any of your mental attributes reaches 0, you die.";
        }

        public override bool Apply(GameObject Object)
        {
            StatShifter.SetStatShift("Ego", -1, true);
            StatShifter.SetStatShift("Willpower", -1, true);
            StatShifter.SetStatShift("Intelligence", -1, true);

            Object.ShowFailure("The thick meat in your head hollows out like a pumice stone.");
            Object.ShowFailure("You've contracted Witkrunk.");
            return false;
        }
        

        public override void Remove(GameObject Object)
        {
            StatShifter.RemoveStatShifts();
            Object.ShowSuccess("You are cured of Witkrunk.");
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("EndTurn");
            base.Register(Object, eventRegistrar);
        }

        public override bool WantTurnTick()
        {
            return base.WantTurnTick();
        }

        public override void TurnTick(long TimeTick, int Amount)
        {
            if (TimeTick % 36000L == 0)
            {
                StatShifter.SetStatShift("Ego", -1, true);
                StatShifter.SetStatShift("Willpower", -1, true);
                StatShifter.SetStatShift("Intelligence", -1, true);

                Object.ShowFailure("You feel your brain hollowing out ...");
            }
            
            
            base.TurnTick(TimeTick, Amount);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                var EgoStatHook = Object?.GetStatValue("Ego");
                var WillpowerStatHook = Object?.GetStatValue("Willpower");
                var IntelligenceStatHook = Object?.GetStatValue("Intelligence");


                if (EgoStatHook <= 0 || WillpowerStatHook <= 0 || IntelligenceStatHook <= 0)
                {
                    Object?.Die(null, "Your head has been hollowed out.", "You have succumbed to Witkrunk.");
                }
            }
            
            return base.FireEvent(E);
        }
    }
}