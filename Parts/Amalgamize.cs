using System;
using System.Collections.Generic;
using XRL.World.Anatomy;
using XRL.World.Capabilities;
using XRL.World.Effects;

namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class Amalgamize : BaseMutation
    {
        public Amalgamize()
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
            Amalgamize obj = base.DeepCopy(Parent) as Amalgamize;
            return obj;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == AIGetOffensiveAbilityListEvent.ID
                   || ID == CommandEvent.ID
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(AIGetOffensiveAbilityListEvent E)
        {
            if (E.Distance <= 1 && IsMyActivatedAbilityAIUsable(ActivatedAbilityID) && GameObject.Validate(E.Target) &&
                E.Actor.HasLOSTo(E.Target, IncludeSolid: true, BlackoutStops: false, UseTargetability: true))
            {
                E.Add("CommandAmalInfect");
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(CommandEvent E)
        {
            if (E.Command != "CommandAmalInfect" || ParentObject.IsNowhere())
            {
                return base.HandleEvent(E);
            }

            
            var AdjCell = ParentObject.CurrentCell.GetAdjacentCells();
            
            GameObject Target = null;
            foreach (var C in AdjCell)
            {
                Target = C.GetCombatTarget(ParentObject, AllowInanimate: false);
                if (Target.IsValid() && Target.IsHostileTowards(ParentObject) == true)
                    break;
            }

            if (!Target.MakeSave("Agility", 16, ParentObject, "Agility"))
            {
                var TargetBody = Target.Body;

                var enumTargetBody = TargetBody.LoopParts();

                var ListTargetBody = new List<BodyPart>();

                foreach (var B in enumTargetBody)
                {
                    if (!B.Abstract)
                    {
                        ListTargetBody.Add(B);
                    }
                }

                var Limb = ListTargetBody.GetRandomElement();

                Limb.AddPart("Skin Slink", 0, "Amalgam Skin Slink");
                Target.ApplyEffect(new AmalgamConversion());
                
                DidXToY("slither", "under", Target, "skin", "...", ColorAsBadFor: Target, SubjectPossessedBy: Target);
                ParentObject.Obliterate();
            }
             
            CooldownMyActivatedAbility(ActivatedAbilityID, 40);
            UseEnergy(1000, "Physical Mutation Amal Infect");
            
            return base.HandleEvent(E);
        }


        public override bool AllowStaticRegistration()
        {
            return true;
        }
        

        public override bool Mutate(GameObject GO, int Level)
        {
            ActivatedAbilityID =
                AddMyActivatedAbility("Infect", "CommandAmalInfect", "Physical Mutations", null, "YUK");
            return base.Mutate(GO, Level);
        }

        public override bool Unmutate(GameObject GO)
        {
            RemoveMyActivatedAbility(ref ActivatedAbilityID);
            return base.Unmutate(GO);
        }
    }
}