// XRL.World.Effects.CookingDomainRegenHightier_RegenerationUnit

using System;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainSuperiorHightier_RegenerationUnit : CookingDomainRegenHightier_RegenerationUnit
    {
        public int regenunit = Stat.Random(100, 200);

        public override void Init(GameObject target)
        {
            Tier = regenunit;
        }

        public override string GetTemplatedDescription()
        {
            return "" + regenunit + "% to natural healing rate";
        }
    }
}