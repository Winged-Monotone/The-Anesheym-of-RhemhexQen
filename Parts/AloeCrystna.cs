// XRL.World.Parts.AloePorta

using System;
using System.Collections.Generic;
using XRL.Rules;
using XRL.World;

namespace XRL.World.Parts
{
    [Serializable]
    public class AloeCrystna : IActivePart
    {
        public string GroupKey = "AloeCrystna";

        public int CooldownLeft;

        public string Cooldown = "1";

        public int Chance = 100;

        public string SaveStat;

        public string SaveDifficultyStat;

        public int SaveTarget = 15;

        public string SaveVs;

        public bool TriggerOnSaveSuccess = true;

        public bool DoCooldownRecolor;

        public string CooldownColorString = "&B";

        public new string ReadyColorString = "&b";

        public string CooldownTileColor = "&Y";

        public string ReadyTileColor = "&y";

        public string CooldownDetailColor = "m";

        public new string ReadyDetailColor = "M";

        public AloeCrystna()
        {
            WorksOnCellContents = true;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("EndTurn");
            eventRegistrar.Register("ObjectEnteredCell");
            eventRegistrar.Register("ObjectCreated");
            base.Register(Object, eventRegistrar);
        }

        public override bool SameAs(IPart p)
        {
            DischargeOnStep dischargeOnStep = p as DischargeOnStep;
            if (dischargeOnStep.Chance != Chance)
            {
                return false;
            }

            if (dischargeOnStep.SaveStat != SaveStat)
            {
                return false;
            }

            if (dischargeOnStep.SaveDifficultyStat != SaveDifficultyStat)
            {
                return false;
            }

            if (dischargeOnStep.SaveTarget != SaveTarget)
            {
                return false;
            }

            if (dischargeOnStep.SaveVs != SaveVs)
            {
                return false;
            }

            if (dischargeOnStep.CooldownLeft != CooldownLeft)
            {
                return false;
            }

            if (dischargeOnStep.TriggerOnSaveSuccess != TriggerOnSaveSuccess)
            {
                return false;
            }

            if (dischargeOnStep.Cooldown != Cooldown)
            {
                return false;
            }

            return base.SameAs(p);
        }

        public void SyncColor()
        {
            if (!DoCooldownRecolor)
            {
                return;
            }

            if (CooldownLeft > 0)
            {
                if (!string.IsNullOrEmpty(CooldownColorString))
                {
                    ParentObject.Render.ColorString = CooldownColorString;
                }

                if (!string.IsNullOrEmpty(CooldownTileColor))
                {
                    ParentObject.Render.TileColor = CooldownTileColor;
                }

                if (!string.IsNullOrEmpty(CooldownDetailColor))
                {
                    ParentObject.Render.DetailColor = CooldownDetailColor;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ReadyColorString))
                {
                    ParentObject.Render.ColorString = ReadyColorString;
                }

                if (!string.IsNullOrEmpty(ReadyTileColor))
                {
                    ParentObject.Render.TileColor = ReadyTileColor;
                }

                if (!string.IsNullOrEmpty(ReadyDetailColor))
                {
                    ParentObject.Render.DetailColor = ReadyDetailColor;
                }
            }
        }

        public bool ValidStepTarget(GameObject obj)
        {
            if (obj.HasPart("Combat"))
            {
                return obj.FlightMatches(ParentObject);
            }

            return false;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn" && E.ID == "EndTurn" && CooldownLeft > 0)
            {
                CooldownLeft--;
                if (CooldownLeft <= 0)
                {
                    SyncColor();
                }
            }

            if (E.ID == "ObjectEnteredCell")
            {
                if (CooldownLeft <= 0 && GetActivePartFirstSubject(ValidStepTarget) != null && IsReady() &&
                    (Chance >= 100 || Stat.Random(1, 100) < Chance))
                {
                    foreach (GameObject activePartSubject in GetActivePartSubjects(ValidStepTarget))
                    {
                        if (activePartSubject.HasStringProperty("aloevoltatarget") ||
                            (!string.IsNullOrEmpty(SaveStat) &&
                             activePartSubject.MakeSave(SaveStat, SaveTarget, ParentObject, SaveDifficultyStat) !=
                             TriggerOnSaveSuccess))
                        {
                            continue;
                        }

                        List<GameObject> objects = ParentObject.Physics.CurrentCell.ParentZone.GetObjects(
                            delegate(GameObject o)
                            {
                                if (o != ParentObject && o.HasPart("AloeCrystna") &&
                                    o.GetPart<AloeCrystna>().GroupKey == GroupKey &&
                                    o.GetPart<AloeCrystna>().CooldownLeft <= 0)
                                {
                                    if (o.Physics.CurrentCell.IsSolid())
                                    {
                                        return false;
                                    }

                                    if (o.Physics.CurrentCell.IsPassable(o))
                                    {
                                        return true;
                                    }
                                }

                                return false;
                            });
                        if (objects.Count > 0)
                        {
                            GameObject randomElement = objects.GetRandomElement();
                            List<Cell> adjacentCells1 = ParentObject.CurrentCell.GetAdjacentCells();
                            CooldownLeft = Stat.Roll(Cooldown);
                            randomElement.GetPart<AloeCrystna>().CooldownLeft =
                                Stat.Roll(randomElement.GetPart<AloeCrystna>().Cooldown);
                            
                            activePartSubject.CurrentCell.AddObject("CrysteningGas500", Stat.Random(1, 4));
                            
                            foreach (Cell cell in adjacentCells1)
                            {
                                if (Stat.Random(1, 100) <= 95)
                                {
                                    cell.AddObject("CrysteningGas500");
                                }
                            }

                            activePartSubject.ParticlePulse(".");
                        }
                    }
                }
            }
            else if (E.ID == "ObjectCreated")
            {
                SyncColor();
            }

            return base.FireEvent(E);
        }
    }
}