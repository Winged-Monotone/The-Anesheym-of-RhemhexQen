using System;
using XRL.Rules;
using XRL.World;
using XRL.World.Effects;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainGreaterHP_UnitHP : ProceduralCookingEffectUnit
    {
        public int Tier;

        public int Bonus;

        public override string GetDescription()
        {
            return Tier.Signed() + "% max HP";
        }

        public override string GetTemplatedDescription()
        {
            return "+50-100% max HP"+
                   "\n-10 to Quickness";
        }

        public override void Init(GameObject target)
        {
            Bonus = 0;
            Tier = Stat.Random(50, 100);
            base.Init(target);
        }

        public override void Apply(GameObject Object, Effect parent)
        {
            Bonus = (int)Math.Ceiling((float)Tier * 0.01f * (float)Object.Statistics["Hitpoints"].BaseValue);
            

            parent.StatShifter.SetStatShift("Hitpoints", Bonus, true);
            parent.StatShifter.SetStatShift("Quickness", -10);

        }

        public override void Remove(GameObject Object, Effect parent)
        {
            parent.StatShifter.RemoveStatShifts();
            Bonus = 0;
        }
    }
}