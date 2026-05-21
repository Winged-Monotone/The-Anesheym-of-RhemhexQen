// XRL.World.Skills.Cooking.GoatAndSweetLeaf

using System;
using System.Collections.Generic;
using XRL.World.Effects;


namespace XRL.World.Skills.Cooking
{
    [Serializable]
    public class DecadentEggAndAmritaBenedict : CookingRecipe
    {
        public DecadentEggAndAmritaBenedict()
        {
            Components.Add(new PreparedCookingRecipieComponentBlueprint("Decadent Egg"));
            Components.Add(new PreparedCookingRecipieComponentBlueprint("Anesheym Honey"));
            Components.Add(new PreparedCookingRecipieComponentBlueprint("Shatter Bread"));
            Effects.Add(new CookingRecipeResultProceduralEffect(ProceduralCookingEffect.CreateSpecific(new List<string>
            {
                "CookingDomainRubber_Extra2Jumps",
                "CookingDomainGreaterHP_UnitHP",
                "CookingDomainSuperiorHightier_RegenerationUnit"
            })));
        }

        public override string GetDescription()
        {
            return "+30-50% max HP.\n 100-200% to natural healing rate.";
        }

        public override string GetApplyMessage()
        {
            return "";
        }

        public override string GetDisplayName()
        {
            return "{{W|Decadent Eggs and Amrita Benedict}}";
        }
    }
}