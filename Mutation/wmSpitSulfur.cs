using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using XRL;
using XRL.UI.ObjectFinderClassifiers;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;


namespace XRL.World.Parts.Mutation
{
    [Serializable]
    // XRL.World.Parts.Mutation.LiquidSpitter
    public class wmSpitSulfur : BaseMutation
    {
        public static readonly Dictionary<string, string> LiquidAnimationColors = new Dictionary<string, string>
        {
            { "liquidsulfur", "&o" }
        };


        public const string ABL_CMD = "CommandSpraySulfur";

        public List<string> Liquids = new List<string>();

        public string LiquidName = "Liquid Sulfur";

        public string GetAnimationColor
        {
            get
            {
                if (Liquids.Count > 0 &&
                    LiquidAnimationColors.TryGetValue(Liquids.GetRandomElementCosmetic(), out var value))
                {
                    return value;
                }

                return "&y";
            }
        }

        public wmSpitSulfur()
        {
        }

        public wmSpitSulfur(string Liquid)
            : this()
        {
            AddLiquid("liquidsulfur");
        }

        public override bool CanLevel()
        {
            return false;
        }

        public int GetCooldown()
        {
            return 40;
        }

        public override IPart DeepCopy(GameObject Parent)
        {
            wmSpitSulfur obj = base.DeepCopy(Parent) as wmSpitSulfur;
            obj.Liquids = new List<string>(Liquids);
            return obj;
        }

        public override void CollectStats(Templates.StatCollector stats, int Level)
        {
            stats.Set("Liquid", ColorUtility.StripBackgroundFormatting(LiquidName));
            stats.Set("Range", 8);
            stats.Set("Area", "3x3");
            stats.CollectCooldownTurns(MyActivatedAbility(ActivatedAbilityID), GetCooldown());
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == AIGetOffensiveAbilityListEvent.ID
                   || ID == CommandEvent.ID
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AIGetOffensiveAbilityListEvent E)
        {
            if (E.Distance <= 8 && IsMyActivatedAbilityAIUsable(ActivatedAbilityID) && GameObject.Validate(E.Target) &&
                E.Actor.HasLOSTo(E.Target, IncludeSolid: true, BlackoutStops: false, UseTargetability: true))
            {
                E.Add("CommandSpraySulfur");
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(CommandEvent E)
        {
            if (E.Command != "CommandSpraySulfur")
            {
                return base.HandleEvent(E);
            }

            List<Cell> list = PickBurst(1, 8, Locked: false, AllowVis.OnlyVisible, "Spray");
            if (list.IsNullOrEmpty())
            {
                return false;
            }

            if (list.Any((Cell C) => C.DistanceTo(ParentObject) > 9))
            {
                return ParentObject.ShowFailure("That is out of range! (8 squares)");
            }

            ParentObject.PlayWorldSound("Sounds/Abilities/sfx_ability_creature_liquid_spit");
            SlimeGlands.SlimeAnimation(GetAnimationColor, ParentObject.CurrentCell, list[0]);
            UseEnergy(1000, "Physical Mutation spray Liquid");
            CooldownMyActivatedAbility(ActivatedAbilityID, 40);
            DidX("spray", "a puddle of " + LiquidName, "!", null, null, ParentObject);
            int i = 0;
            for (int count = list.Count; i < count; i++)
            {
                if (80.in100() || i == 0)
                {
                    list[i].AddObject("LiquidSulferShallowPool");
                }
            }

            return base.HandleEvent(E);
        }


        public override bool AllowStaticRegistration()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "You spray a puddle of liquid sulfur!";
        }

        public override string GetLevelText(int Level)
        {
            return "Range: 8\nArea: 3x3\nCooldown: 10 rounds";
        }

        public override bool Mutate(GameObject GO, int Level)
        {
            ActivatedAbilityID =
                AddMyActivatedAbility("Spray Sulfur", "CommandSpraySulfur", "Physical Mutations", null, "­");
            return base.Mutate(GO, Level);
        }

        public override bool Unmutate(GameObject GO)
        {
            RemoveMyActivatedAbility(ref ActivatedAbilityID);
            return base.Unmutate(GO);
        }

        public void AddLiquid(string ID)
        {
            Liquids.Add("liquidsulfur");
        }
    }
}