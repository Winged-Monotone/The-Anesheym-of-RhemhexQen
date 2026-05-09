using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using HistoryKit;
using XRL;
using XRL.UI.ObjectFinderClassifiers;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;
using XRL.World.Anatomy;
using XRL.Rules;


namespace XRL.World.Parts.Mutation
{
    [Serializable]
    // XRL.World.Parts.Mutation.LiquidSpitter
    public class wmGlareBurst : BaseMutation
    {
        public wmGlareBurst()
        {
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
            wmGlareBurst obj = base.DeepCopy(Parent) as wmGlareBurst;
            return obj;
        }

        public override void CollectStats(Templates.StatCollector stats, int Level)
        {
            stats.Set("Range", 2);
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
            if (E.Distance <= 1 && IsMyActivatedAbilityAIUsable(ActivatedAbilityID) && GameObject.Validate(E.Target) &&
                E.Actor.HasLOSTo(E.Target, IncludeSolid: true, BlackoutStops: false, UseTargetability: true))
            {
                E.Add("CommandLightBurst");
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(CommandEvent E)
        {
            if (E.Command != "CommandLightBurst")
            {
                return base.HandleEvent(E);
            }

            var CellLight = ParentObject.CurrentCell.GetLight();

            UseEnergy(100, "Physical Mutation Burst Light");
            CooldownMyActivatedAbility(ActivatedAbilityID, 40);
            DidX("release", "a burst of light!", "!", null, null, ParentObject);
            ParentObject.CurrentCell.ImplosionSplat(2);
            var AdjCell = ParentObject.CurrentCell.GetAdjacentCells();
            foreach (var C in AdjCell)
            {
                if (C.HasCombatObject() && ((int)CellLight) != 0)
                {
                    var CombatObj = C.GetCombatObject();
                    CombatObj.ApplyEffect(new Stun(5, 7), ParentObject);
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
            return "You release a burst of light!";
        }

        public override string GetLevelText(int Level)
        {
            return "Range: 1\nArea: 3x3\nCooldown: 10 rounds";
        }

        public override bool Mutate(GameObject GO, int Level)
        {
            ActivatedAbilityID =
                AddMyActivatedAbility("Release Light", "CommandLightBurst", "Physical Mutations", null, "­");
            return base.Mutate(GO, Level);
        }

        public override bool Unmutate(GameObject GO)
        {
            RemoveMyActivatedAbility(ref ActivatedAbilityID);
            return base.Unmutate(GO);
        }
    }
}