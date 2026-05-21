// XRL.World.Effects.CookingDomainDarkness_UnitDV

using System;
using XRL.Rules;
using XRL.World.Parts.Mutation;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainGills : ProceduralCookingEffectUnitMutation<Gills>
    {
        public override string GetDescription()
        {
            return "Swim better.";
        }

        public override string GetTemplatedDescription()
        {
            return "You now swim better.";
        }

        public override void Apply(GameObject Object, Effect parent)
        {
            if (!Object.HasPart<Gills>())
            {
                Object.RequirePart<Mutations>().AddMutation("Gills");
            }

            if (Object.HasPart<Gills>())
            {
                var MutationLevel = Stat.Random(3, 8);
                var MutationHook = Object.GetPart<Gills>();
                
                MutationHook.Level = MutationLevel;
            }
        }

        public override void Remove(GameObject Object, Effect parent)
        {
            if (Object.HasPart<Gills>())
            {
                var MutationHook = Object.GetPart<Gills>();
                Object.RequirePart<Mutations>().RemoveMutation(MutationHook);
            }
        }
    }
}