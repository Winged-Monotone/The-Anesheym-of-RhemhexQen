// XRL.World.Parts.GasPoison

using System;
using System.Collections.Generic;
using XRL.Rules;
using XRL.World.Effects;
using System.Text;
using XRL.World.Anatomy;


namespace XRL.World.Parts
{
    [Serializable]
    public class CrystniumGas : IPart
    {
        public string GasType = "CrysteningGas";
        public string MessageColor = "&M";
        public string Noun = "damaging gas";
        public string DamageAttributes = "Gas";
        public int GasLevel = 1;
        public string TargetPart;
        public string TargetTag;
        public string TargetTagValue;
        public string TargetEquippedPart;
        public string TargetEquippedTag;
        public string TargetEquippedTagValue;
        public string ExcludeTag;
        public int TargetBodyPartCategoryCode;
        public bool AffectEquipment;
        public bool AffectCybernetics;
        public bool Respiratory;

        public override bool SameAs(IPart p)
        {
            CrystniumGas CrystniumGas = p as CrystniumGas;
            if (CrystniumGas.GasType != GasType)
            {
                return false;
            }

            if (CrystniumGas.GasLevel != GasLevel)
            {
                return false;
            }

            if (CrystniumGas.GasType != GasType)
            {
                return false;
            }

            if (CrystniumGas.Noun != Noun)
            {
                return false;
            }

            if (CrystniumGas.MessageColor != MessageColor)
            {
                return false;
            }

            if (CrystniumGas.DamageAttributes != DamageAttributes)
            {
                return false;
            }

            if (CrystniumGas.TargetPart != TargetPart)
            {
                return false;
            }

            if (CrystniumGas.TargetTag != TargetTag)
            {
                return false;
            }

            if (CrystniumGas.ExcludeTag != ExcludeTag)
            {
                return false;
            }

            if (CrystniumGas.TargetTagValue != TargetTagValue)
            {
                return false;
            }

            if (CrystniumGas.TargetEquippedPart != TargetEquippedPart)
            {
                return false;
            }

            if (CrystniumGas.TargetEquippedTag != TargetEquippedTag)
            {
                return false;
            }

            if (CrystniumGas.TargetEquippedTagValue != TargetEquippedTagValue)
            {
                return false;
            }

            if (CrystniumGas.TargetBodyPartCategoryCode != TargetBodyPartCategoryCode)
            {
                return false;
            }

            if (CrystniumGas.AffectEquipment != AffectEquipment)
            {
                return false;
            }

            if (CrystniumGas.AffectCybernetics != AffectCybernetics)
            {
                return false;
            }

            if (CrystniumGas.Respiratory != Respiratory)
            {
                return false;
            }

            return base.SameAs(p);
        }

        private void ProcessDamagingGasBehavior()
        {
            Cell currentCell = ParentObject.CurrentCell;
            if (currentCell != null && currentCell.Objects.Count > 1)
            {
                List<GameObject> list = Event.NewGameObjectList();
                list.AddRange(currentCell.Objects);
                int i = 0;
                for (int count = list.Count; i < count; i++)
                {
                    ApplyGasToOthers(list[i]);
                }
            }
        }

        private bool MatchInner(GameObject GO)
        {
            if (GO == null)
            {
                return false;
            }

            if (Respiratory && !GO.Respires)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(ExcludeTag) && GO.HasTag(ExcludeTag))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(TargetPart) && GO.HasPart(TargetPart))
            {
                return true;
            }

            Body body = null;
            if (TargetBodyPartCategoryCode != 0)
            {
                if (body == null)
                {
                    body = (GO.GetPart("Body") as Body);
                }

                if (body != null && body.AnyCategoryParts(TargetBodyPartCategoryCode))
                {
                    return true;
                }
            }

            if (!string.IsNullOrEmpty(TargetTag) && GO.HasTag(TargetTag) &&
                (string.IsNullOrEmpty(TargetTagValue) || GO.GetTag(TargetTag) == TargetTagValue))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(TargetEquippedPart) || !string.IsNullOrEmpty(TargetEquippedTag))
            {
                if (body == null)
                {
                    body = (GO.GetPart("Body") as Body);
                }

                if (body != null)
                {
                    foreach (BodyPart part in body.GetParts())
                    {
                        if (part.Equipped != null)
                        {
                            if (!string.IsNullOrEmpty(TargetEquippedPart) && part.Equipped.HasPart(TargetEquippedPart))
                            {
                                return true;
                            }

                            if (!string.IsNullOrEmpty(TargetEquippedTag) && part.Equipped.HasTag(TargetEquippedTag) &&
                                (string.IsNullOrEmpty(TargetEquippedTagValue) ||
                                 part.Equipped.GetTag(TargetEquippedTag) == TargetEquippedTagValue))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool Match(GameObject GO)
        {
            if (MatchInner(GO))
            {
                return GO.PhaseMatches(ParentObject);
            }

            return false;
        }

        public bool ApplyDamagingGasWithResult(GameObject GO)
        {
            if (GO.IsInvalid() || GO.IsInGraveyard())
            {
                return false;
            }

            Gas gas = ParentObject.GetPart("Gas") as Gas;
            if (!CheckGasCanAffectEvent.Check(GO, ParentObject, gas))
            {
                return false;
            }

            if (!Match(GO))
            {
                return false;
            }

            int num = Respiratory ? GetRespiratoryAgentPerformanceEvent.GetFor(GO, ParentObject, gas) : gas.Density;
            if (num <= 0)
            {
                return false;
            }

            int num2 = 0;
            num2 = ((!GO.HasPart("Combat") || GO.HasPropertyOrTag("GasDamageAsIfInanimate"))
                ? ((int)Math.Ceiling((0.75f * (float)gas.Level + 0.25f) * (float)num))
                : ((int)Math.Ceiling((decimal)(num * gas.Level) / 200m)));
            if (num2 == 0)
            {
                num2 = 1;
            }

            StringBuilder stringBuilder = Event.NewStringBuilder();
            stringBuilder.Append("from ");
            GameObject creator = gas.Creator;
            if (creator != null)
            {
                stringBuilder.Append("%t ");
            }

            if (!string.IsNullOrEmpty(MessageColor))
            {
                stringBuilder.Append("{{").Append(MessageColor).Append('|');
            }

            stringBuilder.Append(Noun);
            if (!string.IsNullOrEmpty(MessageColor))
            {
                stringBuilder.Append("}}");
            }

            stringBuilder.Append('!');
            int amount = num2;
            GameObject attacker = creator;
            return GO.TakeDamage(amount, stringBuilder, DamageAttributes, null, null, attacker);
        }

        public void ApplyDamagingGas(GameObject GO)
        {
            ApplyDamagingGasWithResult(GO);
        }

        private void ApplyGasToOthers(GameObject GO)
        {
            if (GO == ParentObject)
            {
                return;
            }

            ApplyDamagingGas(GO);
            if (AffectEquipment || AffectCybernetics)
            {
                List<GameObject> list = Event.NewGameObjectList();
                if (AffectEquipment)
                {
                    GO.GetEquippedObjects(list);
                }

                if (AffectCybernetics)
                {
                    GO.GetInstalledCybernetics(list);
                }

                int i = 0;
                for (int count = list.Count; i < count; i++)
                {
                    ApplyDamagingGas(list[i]);
                }
            }
        }

        public string TargetBodyPartCategory
        {
            get
            {
                if (TargetBodyPartCategoryCode == 0)
                {
                    return null;
                }

                return BodyPartCategory.GetName(TargetBodyPartCategoryCode);
            }
            set
            {
                if (value == null)
                {
                    TargetBodyPartCategoryCode = 0;
                }
                else
                {
                    TargetBodyPartCategoryCode = BodyPartCategory.GetCode(value);
                }
            }
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade) && ID != GetNavigationWeightEvent.ID)
            {
                return ID == ObjectEnteredCellEvent.ID;
            }

            return true;
        }

        public override bool HandleEvent(GetNavigationWeightEvent E)
        {
            if (CheckGasCanAffectEvent.Check(E.Actor, ParentObject))
            {
                if (E.Smart)
                {
                    if (E.Actor == null || (E.Actor.FireEvent("CanApplyCrystniumGasEffect") &&
                                            E.Actor.PhaseMatches(ParentObject)))
                    {
                        E.MinWeight(GasLevel / 2 + GasLevel * 10, Math.Min(50 + GasLevel * 10, 80));
                    }
                }
                else
                {
                    E.MinWeight(8);
                }
            }

            return true;
        }

        public override bool HandleEvent(ObjectEnteredCellEvent E)
        {
            ApplyCrystniumGas(E.Object);
            if (E.Type != "Thrown")
            {
                ApplyGasToOthers(E.Object);
            }

            return true;
        }

        public override bool WantTurnTick()
        {
            ApplyCrystniumGas();
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("DensityChange");
            base.Register(Object, eventRegistrar);
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

        public void ApplyCrystniumGas()
        {
            ApplyCrystniumGas(ParentObject.CurrentCell);
        }

        public void ApplyCrystniumGas(Cell C)
        {
            if (C == null)
            {
                return;
            }

            List<GameObject> list = C.GetObjectsInCell();
            int i = 0;
            for (int count = list.Count; i < count; i++)
            {
                if (ApplyCrystniumGas(list[i]) && list == C.GetObjectsInCell())
                {
                    list = Event.NewGameObjectList();
                    list.AddRange(C.Objects);
                    count = C.Objects.Count;
                }
            }
        }

        public bool ApplyCrystniumGas(GameObject GO)
        {
            if (GO == ParentObject)
            {
                return false;
            }

            Gas gas = ParentObject.GetPart("Gas") as Gas;
            if (!CheckGasCanAffectEvent.Check(GO, ParentObject, gas))
            {
                return false;
            }

            if (!GO.FireEvent("CanApplyCrystniumGasEffect"))
            {
                return false;
            }

            if (!CanApplyEffectEvent.Check(GO, "CrystniumGasEffect"))
            {
                return false;
            }

            if (!GO.PhaseMatches(ParentObject))
            {
                return false;
            }

            int @for = GetRespiratoryAgentPerformanceEvent.GetFor(GO, ParentObject, gas);
            if (@for <= 0)
            {
                return false;
            }

            if (!GO.HasEffect("CrystniumGasEffect"))
            {
                CrystniumGasEffect crystniumGasEffect = new CrystniumGasEffect(Stat.Random(15, 25),
                    (
                        +Stat.Random(1, 10) + GasLevel + (GasLevel * 4)
                    ),
                    gas.Creator);
                GO.ApplyEffect(crystniumGasEffect);
                return false;
            }
            else

                return false;
        }
    }
}