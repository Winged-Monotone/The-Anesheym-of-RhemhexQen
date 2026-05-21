// XRL.World.Effects.CookingDomainDarkness_UnitDV

using System;
using XRL.Rules;
using XRL.World.Parts.Mutation;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainSulfurSpit : ProceduralCookingEffectUnitMutation<wmSpitSulfur>
    {
        public override string GetDescription()
        {
            return "Spit liquid sulfur at your foes.";
        }

        public override string GetTemplatedDescription()
        {
            return "You can spit liquid sulfur.";
        }

        public override void Apply(GameObject Object, Effect parent)
        {
            if (!Object.HasPart<wmSpitSulfur>())
            {
                Object.RequirePart<Mutations>().AddMutation("wmSpitSulfur");
            }

            if (Object.HasPart<wmSpitSulfur>())
            {
                var MutationLevel = Stat.Random(3, 8);
                var MutationHook = Object.GetPart<wmSpitSulfur>();
                
                MutationHook.Level = MutationLevel;
            }
        }

        public override void Remove(GameObject Object, Effect parent)
        {
            if (Object.HasPart<wmSpitSulfur>())
            {
                var MutationHook = Object.GetPart<wmSpitSulfur>();
                Object.RequirePart<Mutations>().RemoveMutation(MutationHook);
            }
        }
    }
}