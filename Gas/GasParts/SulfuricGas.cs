// XRL.World.Parts.GasPoison

using System;
using System.Collections.Generic;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.Rules;
using XRL.World.Effects;
using XRL.World.Parts.Skill;


namespace XRL.World.Parts
{
    [Serializable]
    public class SulfuricGas : IObjectGasBehavior
    {
        public string GasType = "SulfuricGas";
        public int GasLevel = 1;

        public override bool SameAs(IPart p)
        {
            if ((p as SulfuricGas).GasType != GasType)
            {
                return false;
            }

            return base.SameAs(p);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == ObjectEnteredCellEvent.ID
                   || ID == BeforeTemperatureChangeEvent.ID
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(BeforeTemperatureChangeEvent E)
        {
            if (E.Object == ParentObject && E.Amount > 50)
            {
                Physics.ApplyExplosion(ParentObject.CurrentCell, 150);
                ParentObject.CurrentCell.TemperatureChange(25);
                ParentObject.Destroy();
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            Cell cell = ParentObject.CurrentCell;
            if (cell != null)
            {
                List<GameObject> list = Event.NewGameObjectList(cell.Objects);
                int i = 0;
                for (int count = list.Count; i < count; i++)
                {
                    GameObject gameObject = list[i];
                    if (gameObject != ParentObject && !gameObject.IsScenery)
                    {
                        ApplyGas(gameObject);
                    }
                }
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ObjectEnteredCellEvent E)
        {
            if (E.Type != "Thrown")
            {
                ApplyGas(E.Object);
            }

            return base.HandleEvent(E);
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("DensityChange");
            base.Register(Object, Registrar);
        }


        public bool IsAffectable(GameObject Object, Gas Gas = null)
        {
            if (!CheckGasCanAffectEvent.Check(Object, ParentObject, Gas))
            {
                return false;
            }

            if (Object == null)
            {
                return true;
            }

            if (Object.FireEvent("CanApplyPoisonGasPoison") && CanApplyEffectEvent.Check<PoisonGasPoison>(Object))
            {
                return Object.PhaseMatches(ParentObject);
            }

            return false;
        }

        public override bool ApplyGas(GameObject Object)
        {
            if (Object == ParentObject)
            {
                return false;
            }

            if (!Object.Respires)
            {
                return false;
            }

            if (!Object.HasTag("Creature"))
            {
                return false;
            }

            Gas part = ParentObject.GetPart<Gas>();

            if (!IsAffectable(Object, part))
            {
                return false;
            }

            int @for = GetRespiratoryAgentPerformanceEvent.GetFor(Object, ParentObject, part);
            if (@for <= 0)
            {
                return false;
            }

            Object.RemoveEffect<PoisonGasPoison>();
            PoisonGasPoison poisonGasPoison = new PoisonGasPoison(Stat.Random(1, 10), part.Creator);
            poisonGasPoison.Damage = GasLevel * 2;
            Object.ApplyEffect(poisonGasPoison);
            int amount = (int)Math.Max(Math.Floor((double)(@for + 1) / 20.0), 1.0);
            return Object.TakeDamage(amount, "from %t {{g|poison}}!", "InhaleDanger Poison Gas", null, null, null,
                part.Creator, null, null, null, Accidental: false, Environmental: true);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "DensityChange" &&
                StepValue(E.GetIntParameter("OldValue")) != StepValue(E.GetIntParameter("NewValue")))
            {
                FlushNavigationCaches();
            }

            return base.FireEvent(E);
        }
    }
}