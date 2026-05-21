// XRL.World.Effects.CookingDomainDarkness_UnitDV

using System;
using XRL.Rules;
using XRL.World.Parts.Mutation;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainLightManip : ProceduralCookingEffectUnitMutation<LightManipulation>
    {
        public override string GetDescription()
        {
            return "Manipulate light.";
        }

        public override string GetTemplatedDescription()
        {
            return "You now manipulate light.";
        }

        public override void Apply(GameObject Object, Effect parent)
        {
            if (!Object.HasPart<LightManipulation>())
            {
                Object.RequirePart<Mutations>().AddMutation("LightManipulation");
            }

            if (Object.HasPart<LightManipulation>())
            {
                var MutationLevel = Stat.Random(3, 8);
                var MutationHook = Object.GetPart<LightManipulation>();
                
                MutationHook.Level = MutationLevel;
            }
        }

        public override void Remove(GameObject Object, Effect parent)
        {
            if (Object.HasPart<LightManipulation>())
            {
                var MutationHook = Object.GetPart<LightManipulation>();
                Object.RequirePart<Mutations>().RemoveMutation(MutationHook);
            }
        }
    }
}