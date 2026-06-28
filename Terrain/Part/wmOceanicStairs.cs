using System;
using ConsoleLib.Console;
using XRL;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using XRL.World.AI;
using XRL.World.AI.GoalHandlers;
using XRL.World.Parts;


namespace XRL.World.Parts
{
    [Serializable]
    public class wmOceanicStairs : IPart
    {
        public bool PullDown;

        public bool ConnectLanding = true;

        public int Levels = 1;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == CanSmartUseEvent.ID
                   || ID == PooledEvent<CheckAttackableEvent>.ID
                   || ID == CommandSmartUseEvent.ID
                   || ID == EnteredCellEvent.ID
                   || ID == GetInventoryActionsEvent.ID
                   || ID == PooledEvent<IdleQueryEvent>.ID
                   || ID == InventoryActionEvent.ID
                   || ID == PooledEvent<SubjectToGravityEvent>.ID
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            // AddPlayerMessage("This is Attached.");
            return false;
        }

        public override bool HandleEvent(SubjectToGravityEvent E)
        {
            E.SubjectToGravity = false;
            return false;
        }

        public override bool HandleEvent(GetInventoryActionsEvent E)
        {
            if (CommandBindingManager.GetCommandFromKey(Keys.Oemcomma | Keys.Shift) == "CmdMoveU")
            {
                E.AddAction("Ascend", "ascend", "Ascend", null, 'a');
            }

            if (!PullDown && CommandBindingManager.GetCommandFromKey(Keys.OemPeriod | Keys.Shift) == "CmdMoveD")
            {
                E.AddAction("Descend", "descend", "Descend", null, 'd');
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(InventoryActionEvent E)
        {
            if (E.Command == "Ascend" && E.Actor.IsPlayer() &&
                CommandBindingManager.GetCommandFromKey(Keys.Oemcomma | Keys.Shift) == "CmdMoveU")
            {
                Popup.ShowFail("Use " + ControlManager.getCommandInputFormatted("CmdMoveU") + " to ascend.");
                E.RequestInterfaceExit();
            }

            if (E.Command == "Descend" && !PullDown && E.Actor.IsPlayer() &&
                CommandBindingManager.GetCommandFromKey(Keys.OemPeriod | Keys.Shift) == "CmdMoveD")
            {
                Popup.ShowFail("Use " + ControlManager.getCommandInputFormatted("CmdMoveD") + " to descend.");
                E.RequestInterfaceExit();
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(CheckAttackableEvent E)
        {
            return false;
        }

        public override bool HandleEvent(CanSmartUseEvent E)
        {
            if (E.Actor.CurrentCell == ParentObject.CurrentCell)
            {
                return false;
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(CommandSmartUseEvent E)
        {
            if (E.Actor.CurrentCell == ParentObject.CurrentCell)
            {
                if (E.Actor.IsPlayer())
                {
                    Keyboard.PushMouseEvent("Command:CmdMoveU");
                }
                else
                {
                    E.Actor.Move("U", Forced: false, System: false, IgnoreGravity: false, NoStack: false,
                        AllowDashing: false);
                }

                if (E.Actor.IsPlayer())
                {
                    Keyboard.PushMouseEvent("Command:CmdMoveD");
                }
                else
                {
                    for (int i = 0; i < Levels; i++)
                    {
                        E.Actor.Move("D", Forced: false, Levels > 1, IgnoreGravity: false, NoStack: false,
                            AllowDashing: false);
                    }
                }
            }

            return false;
        }

        public override bool HandleEvent(IdleQueryEvent E)
        {
            if (ParentObject.HasTagOrProperty("IdleStairs") && E.Actor.HasPart<Brain>() && Stat.Random(1, 2000) == 2000)
            {
                GameObject who = E.Actor;
                who.Brain.PushGoal(new DelegateGoal(delegate(GoalHandler h)
                {
                    if (who.GetCurrentCell() == ParentObject.GetCurrentCell())
                    {
                        who.Move("U", Forced: false, System: false, IgnoreGravity: false, NoStack: false,
                            AllowDashing: false);
                    }

                    h.FailToParent();
                }));
                who.Brain.PushGoal(new MoveTo(ParentObject));
                return false;
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnteredCellEvent E)
        {
            if (E.Cell.ParentZone.Z > 10)
            { 
                ParentObject.RegisterPartEvent(this, "ClimbUp");
            }
            bool flag = false;
            foreach (GameObject @object in E.Cell.Objects)
            {
                if (@object != ParentObject && @object.HasPart<StairsUp>())
                {
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                E.Cell.RemoveObject(ParentObject);
                ParentObject.Obliterate();
                return false;
            }

            E?.Cell?.ClearWalls();
            return base.HandleEvent(E);
        }

        public override bool AllowStaticRegistration()
        {
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            // Registrar.Register("ClimbUp");
            Registrar.Register("ClimbDown");
            base.Register(Object, Registrar);
        }
        
        

        public override bool FireEvent(Event E)
        {
            if (E.ID == "ClimbUp")
            {
                GameObject gameObjectParameter = E.GetGameObjectParameter("GO");
                if (gameObjectParameter != null && gameObjectParameter.IsPlayer() && ParentObject.HasTag("KeyObject") &&
                    !gameObjectParameter.IsCarryingObject(ParentObject.GetTag("KeyObject")))
                {
                    Popup.Show("You don't have the proper key standin message");
                    return false;
                }

                if (gameObjectParameter.Move("U", Forced: false, System: false, IgnoreGravity: false, NoStack: false,
                        AllowDashing: false))
                {
                    AddPlayerMessage("You ascend from the depths.");
                }


                return false;
            }

            if (E.ID == "ClimbDown")
            {
                GameObject gameObjectParameter = E.GetGameObjectParameter("GO");
                if (gameObjectParameter != null && gameObjectParameter.IsPlayer())
                {
                    string tag = ParentObject.GetTag("KeyObject");
                    if (!string.IsNullOrEmpty(tag) && !gameObjectParameter.IsCarryingObject(tag))
                    {
                        DidX("are", "locked, and you don't have the key", null, null, null, null, null,
                            UseFullNames: false, IndefiniteSubject: false, null, null, DescribeSubjectDirection: false,
                            DescribeSubjectDirectionLate: false, AlwaysVisible: false, FromDialog: true);
                        return false;
                    }
                }

                if (!PullDown || gameObjectParameter.IsFlying)
                {
                    bool flag = false;
                    for (int i = 0; i < Levels; i++)
                    {
                        flag |= gameObjectParameter.Move("D", Forced: false, Levels > 1, IgnoreGravity: false,
                            NoStack: false, AllowDashing: false);
                    }

                    if (flag)
                    {
                        AddPlayerMessage("You descend further into the depths.");
                    }
                }

                return false;
            }

            return base.FireEvent(E);
        }
    }
}