// XRL.World.Effects.CookingDomainDarkness_UnitDV

using System;
using XRL.Rules;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts.Mutation;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]
    public class CookingDomainFlight : ProceduralCookingEffectUnitMutation<Wings>
    {
        public override string GetDescription()
        {
            return "Royal jelly has gifted you elytra to fly with.";
        }

        public override string GetTemplatedDescription()
        {
            return "You've grown elytra, you can now fly.";
        }

        public override void Apply(GameObject Object, Effect parent)
        {
            if (!Object.HasPart<Wings>())
            {
                Object.RequirePart<Mutations>().AddMutation("Wings");
            }

            if (Object.HasPart<Wings>())
            {
                var flightlvl = Stat.Random(3, 5);
                var MutationHook = Object.GetPart<Wings>();

                MutationHook.SetDisplayName("Royal Elytra");
                MutationHook.Level = flightlvl;
            }
        }

        public override void Remove(GameObject Object, Effect parent)
        {
            if (Object.HasPart<Wings>())
            {
                var MutationHook = Object.GetPart<Wings>();
                Object.RequirePart<Mutations>().RemoveMutation(MutationHook);
            }

            Object.ShowSuccess("Your elytra rapidly atrophy and decay away.");
        }
    }
}