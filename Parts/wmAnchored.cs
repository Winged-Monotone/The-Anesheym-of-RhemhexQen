using System;
using XRL.Rules;
using Genkit;
using XRL.World.AI;
using System.IO;
using XRL.World.AI.GoalHandlers;
using System.Linq;
using XRL.UI.ObjectFinderClassifiers;
using UnityEngine.Rendering;
using HistoryKit;
using System.Text;
using XRL;
using XRL.Core;
using XRL.UI;
using XRL.World;
using XRL.World.Capabilities;
using XRL.World.Effects;

namespace XRL.World.Parts
{
    [Serializable]
    public class wmAnchored : IPart
    {
        public int SpawnLimit = Stat.Random(1, 5);
        public int Spawn = 0;
        public string DependsOn;
        public bool DependsOnMustBeFrozen;
        public bool DependsOnMustBeSolid;

        public int KineticResistanceLinearBonus;
        public int KineticResistancePercentageBonus;

        public string Adjective = "stuck";

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == ModifyDefendingSaveEvent.ID
                   || ID == EffectRemovedEvent.ID
                   || ID == BeginTakeActionEvent.ID
                   || ID == CanChangeMovementModeEvent.ID
                   || ID == EffectAppliedEvent.ID
                   || ID == EndTurnEvent.ID
                   || ID == GetCompanionStatusEvent.ID
                   || ID == GetKineticResistanceEvent.ID
                   || ID == GetNavigationWeightEvent.ID;
        }

        public override bool HandleEvent(CanChangeMovementModeEvent E)
        {
            if (E.Object == ParentObject)
            {
                E.Object.Fail("You are " + Adjective + "!");
                return false;
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetNavigationWeightEvent E)
        {
            if (!E.Flying)
            {
                E.Uncacheable = true;
                E.MinWeight(100);
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeginTakeActionEvent E)
        {
            CheckDependsOn();
            return base.HandleEvent(E);
        }

        public GameObject CheckDependsOn(bool Immediate = false, bool AllowDisplayNameUpdate = true)
        {
            GameObject Object = null;
            if (!DependsOn.IsNullOrEmpty())
            {
                Object = GameObject.FindByID(DependsOn);
                if (!GameObject.Validate(ref Object) || !Object.InSameOrAdjacentCellTo(ParentObject) ||
                    (!Object.PhaseMatches(ParentObject) && !Object.HasTagOrProperty("IgnorePhaseMatchForStuck")) ||
                    (DependsOnMustBeFrozen && !Object.IsFrozen()) ||
                    (DependsOnMustBeSolid && !Object.ConsiderSolidFor(ParentObject)))
                {
                }
            }

            return Object;
        }

        public GameObject CheckDependsOn(string Invalidate, bool Immediate = false, bool AllowDisplayNameUpdate = true)
        {
            return CheckDependsOn(Immediate, AllowDisplayNameUpdate);
        }

        public override bool HandleEvent(GetKineticResistanceEvent E)
        {
            if (KineticResistanceLinearBonus > 0)
            {
                E.LinearIncrease += KineticResistanceLinearBonus;
            }
            else if (KineticResistanceLinearBonus < 0)
            {
                E.LinearReduction += -KineticResistanceLinearBonus;
            }

            if (KineticResistancePercentageBonus > 0)
            {
                E.PercentageIncrease += KineticResistancePercentageBonus;
            }
            else if (KineticResistancePercentageBonus < 0)
            {
                E.PercentageReduction += -KineticResistancePercentageBonus;
            }

            return base.HandleEvent(E);
        }
    }
}