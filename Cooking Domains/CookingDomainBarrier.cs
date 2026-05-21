// XRL.World.Effects.CookingDomainDarkness_UnitDV

using System;
using XRL.Rules;
using XRL.World.Parts.Mutation;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainBarrier : ProceduralCookingEffectUnitMutation<ForceWall>
    {
        public override string GetDescription()
        {
            return "Create a barrier to protect yourself.";
        }

        public override string GetTemplatedDescription()
        {
            return "You now use the force wall mutation.";
        }

        public override void Apply(GameObject Object, Effect parent)
        {
            if (!Object.HasPart<ForceWall>())
            {
                Object.RequirePart<Mutations>().AddMutation("ForceWall");
            }

            if (Object.HasPart<ForceWall>())
            {
                var MutationLevel = Stat.Random(3, 8);
                var MutationHook = Object.GetPart<ForceWall>();
                
                MutationHook.Level = MutationLevel;
            }
        }

        public override void Remove(GameObject Object, Effect parent)
        {
            if (Object.HasPart<ForceWall>())
            {
                var MutationHook = Object.GetPart<ForceWall>();
                Object.RequirePart<Mutations>().RemoveMutation(MutationHook);
            }
        }
    }
}