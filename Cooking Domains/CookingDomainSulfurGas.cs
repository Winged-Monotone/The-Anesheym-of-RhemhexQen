// XRL.World.Effects.CookingDomainDarkness_UnitDV

using System;
using XRL.Rules;
using XRL.World.Parts.Mutation;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainSulfurGas : ProceduralCookingEffectUnitMutation<StinkyBreather>
    {
        public override string GetDescription()
        {
            return "Breath sulfuric gas in a cone at your targets.";
        }

        public override string GetTemplatedDescription()
        {
            return "You can now breath sulphuric gas.";
        }

        public override void Apply(GameObject Object, Effect parent)
        {
            if (!Object.HasPart<StinkyBreather>())
            {
                Object.RequirePart<Mutations>().AddMutation("StinkyBreather");
            }

            if (Object.HasPart<StinkyBreather>())
            {
                var MutationLevel = Stat.Random(3, 8);
                var MutationHook = Object.GetPart<StinkyBreather>();
                
                MutationHook.Level = MutationLevel;
            }
        }

        public override void Remove(GameObject Object, Effect parent)
        {
            if (Object.HasPart<StinkyBreather>())
            {
                var MutationHook = Object.GetPart<StinkyBreather>();
                Object.RequirePart<Mutations>().RemoveMutation(MutationHook);
            }
        }
    }
}