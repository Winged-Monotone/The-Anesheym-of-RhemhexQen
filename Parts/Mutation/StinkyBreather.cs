// XRL.World.Parts.PoisonBreather

using System;
using ConsoleLib.Console;
using XRL.World;
using XRL.World.Parts.Mutation;
using XRL;
using XRL.World.Parts;
using System.Collections.Generic;
using System.Threading;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World.Anatomy;
using System.Windows.Input;


namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class StinkyBreather : BaseMutation
    {
        public string BodyPartType = "Face";

        public bool CreateObject = true;

        public GameObject FaceObject;

        public string GetCommandDisplayName()
        {
            return "Breathe Sulfuric Gas";
        }

        public override string GetDescription()
        {
            return "You breathe sulfur dioxide.";
        }

        public override string GetLevelText(int Level)
        {
            return string.Concat(
                string.Concat("Breathes sulfuric gas in a cone.\n" + "Cone length: " + GetConeLength() + " tiles\n",
                    "Cone angle: ", GetConeAngle().ToString(), " degrees\n"), "Cooldown: 15 rounds\n");
        }

        public string GetGasBlueprint()
        {
            return "SulfuricGas50";
        }

        public string GetBreathName()
        {
            return "sulfuric gas";
        }

        public void BreatheInCell(Cell C, ScreenBuffer Buffer, bool doEffect = true)
        {
            DrawBreathInCell(C, Buffer, "O", "o", "w");
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == AIGetOffensiveAbilityListEvent.ID
                       || ID == CommandEvent.ID;
            }

            return true;
        }

        public override bool HandleEvent(CommandEvent E)
        {
            if (CommandID == E.Command)
            {
                Cast();
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AIGetOffensiveAbilityListEvent E)
        {
            if (E.Distance <= GetConeLength() - 2 && IsMyActivatedAbilityAIUsable(ActivatedAbilityID) &&
                GameObject.Validate(E.Target) && E.Actor.HasLOSTo(E.Target, IncludeSolid: true, BlackoutStops: false,
                    UseTargetability: true))
            {
                E.Add(CommandID);
            }

            return base.HandleEvent(E);
        }


        public override bool GeneratesEquipment()
        {
            if (CreateObject)
            {
                return GetFaceObject() != null;
            }

            return false;
        }

        public virtual string GetFaceObject()
        {
            return null;
        }

        public override void CollectStats(Templates.StatCollector stats, int Level)
        {
            base.CollectStats(stats, Level);
            ActivatedAbilityEntry activatedAbilityEntry = MyActivatedAbility(ActivatedAbilityID, ParentObject);
            if (activatedAbilityEntry != null)
            {
                stats.CollectCooldownTurns(activatedAbilityEntry, GetCooldownTurns(Level));
                stats.Set("ConeLength", GetConeLength(Level));
                stats.Set("ConeAngle", GetConeAngle(Level));
            }
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register(CommandID);
            base.Register(Object, Registrar);
        }

        public virtual int GetCooldownTurns(int Level)
        {
            return 15;
        }

        public virtual int GetConeLength(int L = -1)
        {
            if (L == -1)
            {
                return 4 + base.Level;
            }

            return 4 + L;
        }

        public virtual int GetConeAngle(int L = -1)
        {
            if (L == -1)
            {
                return 20 + 2 * base.Level;
            }

            return 20 + 2 * L;
        }


        public void DrawBreathInCell(Cell C, ScreenBuffer Buffer, string color1, string color2, string color3)
        {
            Buffer.Goto(C.X, C.Y);
            string text = "&G";
            int num = Stat.Random(1, 3);
            if (num == 1)
            {
                text = "&" + color1;
            }

            if (num == 2)
            {
                text = "&" + color2;
            }

            if (num == 3)
            {
                text = "&" + color3;
            }

            int num2 = Stat.Random(1, 3);
            if (num2 == 1)
            {
                text = text + "^" + color1;
            }

            if (num2 == 2)
            {
                text = text + "^" + color2;
            }

            if (num2 == 3)
            {
                text = text + "^" + color3;
            }

            if (C.ParentZone == XRLCore.Core.Game.ZoneManager.ActiveZone)
            {
                Stat.Random(1, 3);
                Buffer.Write(text + (char)(219 + Stat.Random(0, 4)));
                Popup._TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(Math.Max(1, 15 - base.Level));
            }
        }

        public virtual Gas BreatheGasInCell(Cell C, Event CreatorEvent)
        {
            string gasBlueprint = GetGasBlueprint();
            if (gasBlueprint == null)
            {
                return null;
            }

            Gas part = C.AddObject(gasBlueprint).GetPart<Gas>();
            part.Creator = ParentObject;
            CreatorEvent.SetParameter("Gas", part);
            return part;
        }

        public virtual int SortCells(Cell c1, Cell c2)
        {
            return c1.PathDistanceTo(ParentObject.Physics.CurrentCell)
                .CompareTo(c2.PathDistanceTo(ParentObject.Physics.CurrentCell));
        }

        public bool Cast()
        {
            ScreenBuffer scrapBuffer = ScreenBuffer.GetScrapBuffer1(bLoadFromCurrent: true);
            XRLCore.Core.RenderMapToBuffer(scrapBuffer);
            List<Cell> list = PickCone(GetConeLength(), GetConeAngle(), AllowVis.Any, null,
                GetCommandDisplayName() ?? "Breathe");
            if (list == null || list.Count <= 0)
            {
                return false;
            }

            if (list.Count == 1 && ParentObject.IsPlayer() &&
                Popup.ShowYesNoCancel("Are you sure you want to target " + ParentObject.itself + "?") != 0)
            {
                return false;
            }

            UseEnergy(1000);
            CooldownMyActivatedAbility(ActivatedAbilityID, GetCooldownTurns(Level));
            list.Sort(SortCells);
            ParentObject?.PlayWorldSound("Sounds/Abilities/sfx_ability_gas_breathe");
            if (ParentObject.IsPlayer())
            {
                IComponent<GameObject>.XDidY(ParentObject, "breath", "a cone of " + GetBreathName(), "!", null, null,
                    ParentObject);
            }
            else
            {
                IComponent<GameObject>.XDidY(ParentObject, "breath", "a cone of " + GetBreathName(), "!", null, null,
                    ParentObject, null, UseFullNames: false, IndefiniteSubject: false, null, null,
                    DescribeSubjectDirection: true);
            }

            string gasBlueprint = GetGasBlueprint();
            Event @event = ((gasBlueprint != null) ? Event.New("CreatorModifyGas", "Gas", (object)null) : null);
            int i = 0;
            for (int count = list.Count; i < count; i++)
            {
                if ((i == 0 || ParentObject.HasLOSTo(list[i])) &&
                    (list.Count == 1 || list[i] != ParentObject.CurrentCell))
                {
                    if (gasBlueprint != null)
                    {
                        BreatheGasInCell(list[i], @event);
                        ParentObject.FireEvent(@event);
                    }

                    BreatheInCell(list[i], scrapBuffer);
                }
            }

            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == CommandID && !Cast())
            {
                return false;
            }

            return base.FireEvent(E);
        }

        public override bool ChangeLevel(int NewLevel)
        {
            return base.ChangeLevel(NewLevel);
        }

        public virtual bool OnMutate(GameObject GO)
        {
            return true;
        }

        public virtual bool OnUnmutate(GameObject GO)
        {
            return true;
        }

        public override bool Mutate(GameObject GO, int Level)
        {
            Unmutate(GO);
            OnMutate(GO);
            if (CreateObject && GetFaceObject() != null)
            {
                Body body = GO.Body;
                if (body != null)
                {
                    foreach (BodyPart item in body.GetPart(BodyPartType))
                    {
                        if (item.ForceUnequip(Silent: true))
                        {
                            FaceObject = GameObject.Create(GetFaceObject());
                            FaceObject.GetPart<Armor>().WornOn = item.Type;
                            GO.ForceEquipObject(FaceObject, item, Silent: true, 0);
                            break;
                        }
                    }
                }
            }

            ActivatedAbilityID = AddMyActivatedAbility(GetCommandDisplayName(), CommandID, "Physical Mutations",
                GetDescription(), "­");
            return base.Mutate(GO, Level);
        }

        public override bool Unmutate(GameObject GO)
        {
            OnUnmutate(GO);
            CleanUpMutationEquipment(GO, ref FaceObject);
            if (GameObject.Validate(ref FaceObject))
            {
                FaceObject.Obliterate();
            }

            RemoveMyActivatedAbility(ref ActivatedAbilityID);
            return base.Unmutate(GO);
        }

        public Guid ActivatedAbilityId
        {
            get { return ActivatedAbilityID; }
            set { ActivatedAbilityID = value; }
        }

        public string CommandID
        {
            get
            {
                if (_CommandID == null)
                {
                    _CommandID = "CommandBreatheSulfur";
                }

                return _CommandID;
            }
        }

        public string _CommandID;
    }
}