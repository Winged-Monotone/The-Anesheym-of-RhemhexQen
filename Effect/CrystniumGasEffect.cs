// XRL.World.Effects.PoisonGasPoison

using System;
using XRL.Core;
using ConsoleLib.Console;
using System.Threading;
using System.Linq;
using XRL.World;
using XRL.World.Parts.Mutation;
using System.Collections.Generic;
using XRL.Rules;
using XRL.World.Effects;
using XRL.Language;
using XRL.World.Capabilities;
using UnityEngine;


namespace XRL.World.Effects
{
    [Serializable]
    public class CrystniumGasEffect : Effect
    {
        public int Damage = 3;
        public int Severity = 0;
        public int SeverityScaling = 0;
        public int carrySeverityScaling = 0;
        public bool isCrystened = false;
        public GameObject Owner;

        public CrystniumGasEffect()
        {
            base.DisplayName = "{{M|crystening}}";
        }

        public CrystniumGasEffect(int Duration, int Severity, GameObject Owner)
            : this()
        {
            this.Owner = Owner;
            base.Duration = Duration;
            this.Severity = Severity;
        }

        public override int GetEffectType()
        {
            return 8;
        }

        public override bool SameAs(Effect e)
        {
            return false;
        }

        public override string GetDetails()
        {
            return "Your flesh is crystallizing";
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.FireEvent("ApplyCrystniumGasEffect"))
            {
                return ApplyEffectEvent.Check(Object, "CrystniumGasEffect", this);
            }

            return false;
        }

        public override void Remove(GameObject Object)
        {
            StatShifter.RemoveStatShifts(Object);
            StatShifter.RemoveStatShifts();
            Severity = 0;
            SeverityScaling = 0;
            carrySeverityScaling = 0;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return
                    ID == BeforeDeathRemovalEvent.ID
                    || ID == BeforeDieEvent.ID
                    || ID == ObjectEnteringCellEvent.ID
                    || ID == GetShortDescriptionEvent.ID
                    || ID == GetDisplayNameEvent.ID;
            }

            return true;
        }

        public int LimitToRange(int value, int inclusiveMinimum, int inlusiveMaximum)
        {
            if (value >= inclusiveMinimum)
            {
                if (value <= inlusiveMaximum)
                {
                    return value;
                }

                return inlusiveMaximum;
            }

            return inclusiveMinimum;
        }

        public override bool HandleEvent(GetDisplayNameEvent E)
        {
            if (Severity == (LimitToRange(1, 1, 25)))
            {
                E.AddTag("{{m|minor}} crystening");
            }
            else if (Severity == (LimitToRange(26, 26, 50)))
            {
                E.AddTag("{{m|moderate}} crystening");
            }
            else if (Severity == (LimitToRange(56, 56, 75)))
            {
                E.AddTag("{{M|severe}} crystening");
            }
            else if (Severity == (LimitToRange(76, 76, 100)))
            {
                E.AddTag("{{R|critical}} crystening");
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeforeDeathRemovalEvent E)
        {
            if (E.Dying.IsPlayer())
            {
                TextConsole _TextConsole = UI.Look._TextConsole;
                ScreenBuffer Buffer = TextConsole.ScrapBuffer;
                Core.XRLCore.Core.RenderMapToBuffer(Buffer);
                GameObject Player = E.Dying;
                Cell PlayerCell = Player.CurrentCell;

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.DetailColor = "Y";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);

                Thread.Sleep(100);
                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.TileColor = "&C";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeforeDieEvent E)
        {
            if (E.Dying.IsPlayer())
            {
                TextConsole _TextConsole = UI.Look._TextConsole;
                ScreenBuffer Buffer = TextConsole.ScrapBuffer;
                Core.XRLCore.Core.RenderMapToBuffer(Buffer);
                GameObject Player = E.Dying;
                Cell PlayerCell = Player.CurrentCell;

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.DetailColor = "Y";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(100);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.TileColor = "&Y";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(100);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.TileColor = "&K";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(100);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.DetailColor = "K";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(100);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.DetailColor = "Y";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(75);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.TileColor = "&Y";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(75);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.DetailColor = "K";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(100);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.TileColor = "&K";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(25);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.Tile = ("creatures/CrystalPlayer1");
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
                Thread.Sleep(75);

                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.DetailColor = "Y";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);

                Thread.Sleep(100);
                Buffer.Goto(PlayerCell.X, PlayerCell.Y);
                Player.Render.TileColor = "&C";
                Buffer.Write(Player.Render);
                _TextConsole.DrawBuffer(Buffer);
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GeneralAmnestyEvent E)
        {
            Owner = null;
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("EndTurn");
            eventRegistrar.Register("ObjectEnteredCell");
            eventRegistrar.Register("Recuperating");
            base.Register(Object, eventRegistrar);
        }

        public override bool Render(RenderEvent E)
        {
            int num = XRLCore.CurrentFrame % 60;
            if (num > 35 && num < 45)
            {
                E.Tile = null;
                E.RenderString = "!";
                E.ColorString = "&b^B";
            }

            return true;
        }

        public void Crystallize()
        {
            if (Severity == LimitToRange(1, 1, 10))
            {
                StatShifter.SetStatShift("AV", 1);
                StatShifter.SetStatShift("MoveSpeed", 10);
                StatShifter.SetStatShift("Speed", -10);
            }
            else if (Severity == LimitToRange(11, 12, 20))
            {
                StatShifter.SetStatShift("AV", 1);
                StatShifter.SetStatShift("MoveSpeed", 15);
                StatShifter.SetStatShift("Speed", -15);
            }
            else if (Severity == LimitToRange(21, 22, 30))
            {
                StatShifter.SetStatShift("AV", 2);
                StatShifter.SetStatShift("MoveSpeed", 20);
                StatShifter.SetStatShift("Speed", -20);
            }
            else if (Severity == LimitToRange(31, 32, 40))
            {
                StatShifter.SetStatShift("AV", 2);
                StatShifter.SetStatShift("MoveSpeed", 25);
                StatShifter.SetStatShift("Speed", -25);
            }
            else if (Severity == LimitToRange(41, 42, 50))
            {
                StatShifter.SetStatShift("AV", 3);
                StatShifter.SetStatShift("MoveSpeed", 35);
                StatShifter.SetStatShift("Speed", -35);
            }
            else if (Severity == LimitToRange(51, 53, 60))
            {
                StatShifter.SetStatShift("AV", 5);
                StatShifter.SetStatShift("MoveSpeed", 60);
                StatShifter.SetStatShift("Speed", -60);
            }
            else if (Severity == LimitToRange(61, 62, 70))
            {
                StatShifter.SetStatShift("AV", 8);
                StatShifter.SetStatShift("MoveSpeed", 70);
                StatShifter.SetStatShift("Speed", -70);
            }
            else if (Severity == LimitToRange(71, 72, 80))
            {
                StatShifter.SetStatShift("AV", 9);
                StatShifter.SetStatShift("MoveSpeed", 80);
                StatShifter.SetStatShift("Speed", -80);
            }
            else if (Severity == LimitToRange(81, 82, 90))
            {
                StatShifter.SetStatShift("AV", 20);
                StatShifter.SetStatShift("MoveSpeed", 90);
                StatShifter.SetStatShift("Speed", -90);
            }
            else if (Severity == LimitToRange(91, 92, 100) || Severity > 100)
            {
                Object.Die(null,
                    "{{purple|" + Object.It + Object.GetVerb("succumb", true, true) + " to the crystening.}}");
            }
        }

        public void AddCrystenedFleshEffect()
        {
            if (!Object.HasEffect("CrystenedFleshEffect"))
            {
                Object.ApplyEffect(new CrystenedFleshEffect(DURATION_INDEFINITE, null));
            }
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                if (isCrystened == false && Severity >= 50 && !Object.HasEffect("CrystenedFleshEffect"))
                {
                    AddCrystenedFleshEffect();
                    isCrystened = true;
                }

                if (Duration > 0 && base.Object.CurrentCell.HasObjectWithPart("CrystniumGas"))
                {
                    // AddPlayerMessage("[In Smoke]");
                    Severity += 1 + carrySeverityScaling;
                    ++Duration;
                    // HandleDisplayNameUpdate();
                    Crystallize();
                }
                else if (Duration > 0 && !base.Object.CurrentCell.HasObjectWithPart("CrystniumGas"))
                {
                    // AddPlayerMessage("[Not in Smoke]");
                    Severity += 1 + carrySeverityScaling;
                    --Duration;
                    // HandleDisplayNameUpdate();
                    Crystallize();
                }

                if (Severity <= 25)
                {
                    DisplayName = ("{{m|minorly crystening}}");
                }
                else if (Severity <= 50)
                {
                    DisplayName = ("{{m|moderately crystening}}");
                }
                else if (Severity <= 75)
                {
                    DisplayName = ("{{M|severely crystening}}");
                }
                else if (Severity < 100)
                {
                    DisplayName = ("{{R|critically crystening}}");
                }
                else
                {
                    DisplayName = ("{{M|crystening}}");
                }
                // HandleDisplayNameUpdate();
                // AddPlayerMessage("Duration: " + Duration);
                // AddPlayerMessage("Severity: " + Severity);
                // AddPlayerMessage("SeverityScaling: " + SeverityScaling);
                // AddPlayerMessage("Total Severity: " + (Severity + SeverityScaling));
            }
            else if (E.ID == "Recuperating")
            {
                Duration = 0;
                if (Object != ThePlayer)
                    DidX(Object.Is, "no longer crystallizing", "!", null);
                else if (Object.IsPlayer())
                    DidX("You are", "no longer crystallizing", "!", null);
            }

            return base.FireEvent(E);
        }
    }
}