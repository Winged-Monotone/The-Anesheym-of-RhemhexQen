using System;
using System.Collections.Generic;
using XRL.World.Parts;
using XRL.Rules;
using XRL.UI;
using ConsoleLib.Console;
using XRL.Core;
using EMPMutationPart = XRL.World.Parts.Mutation.ElectromagneticPulse;

namespace XRL.World.Parts
{
    public class wmLightningStrike : IPart
    {
        public override bool WantTurnTick() => true;

        public override void TurnTick(long TimeTick, int Amount)
        {
            var lightningChance = Stat.Random(1, 100);

            if (lightningChance <= 7)
            {
                LightningAnimation();
            }

            base.TurnTick(TimeTick, Amount);
        }

        public void ReverseParticleText(string Text, float Velocity, int Life)
        {
            Zone ParentZone = ParentObject.CurrentZone;

            if (ParentZone == XRLCore.Core.Game.ZoneManager.ActiveZone)
            {
                int X = ParentObject.CurrentCell.X;
                int Y = ParentObject.CurrentCell.Y;
                // The 360 refers to a Circular Pattern associated with map.
                float num = (float)Stat.Random(0, 359) / 58f;
                float YVelocity = (float)Math.Sin(num) / 4f;
                float XVelocity = (float)Math.Cos(num) / 4f;

                YVelocity *= Velocity;
                XVelocity *= Velocity;

                int DeltaX = (int)(XVelocity * Life);
                int DeltaY = (int)(YVelocity * Life);

                X = X + DeltaX;
                Y = Y + DeltaY;

                if (X >= 80 || X < 0 || Y >= 25 || Y < 0)
                {
                    if (X >= 80 && ParentObject.CurrentCell.X != 79)
                    {
                        X = 79;
                    }

                    else if (X < 0 && ParentObject.CurrentCell.X != 0)
                    {
                        X = 0;
                    }

                    if (Y >= 25 && ParentObject.CurrentCell.Y != 24)
                    {
                        Y = 24;
                    }

                    else if (Y < 0 && ParentObject.CurrentCell.Y != 0)
                    {
                        Y = 0;
                    }

                    YVelocity = ParentObject.CurrentCell.Y - Y;
                    XVelocity = ParentObject.CurrentCell.X - X;

                    float Magnitude = (float)Math.Sqrt((XVelocity * XVelocity) + (YVelocity * YVelocity));

                    XVelocity = -(XVelocity / Life);
                    YVelocity = -(YVelocity / Life);
                }

                XRLCore.ParticleManager.Add(Text, X, Y, -XVelocity, -YVelocity, Life, 0f, 0f);
            }
        }

        public void ElectricChargePulse(Cell TargetCell, int Charges)
        {
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 3 + Charges; j++)
                {
                    TargetCell.ParticleText("&W" + (char)Stat.RandomCosmetic(191, 198), 2.9f, 5 + Charges);
                }

                for (int k = 0; k < 2 + Charges; k++)
                {
                    TargetCell.ParticleText("&Y" + (char)Stat.RandomCosmetic(191, 198), 3.9f, 5 + Charges);
                }

                for (int l = 0; l < 4 + Charges; l++)
                {
                    TargetCell.ParticleText("&B" + (char)Stat.RandomCosmetic(191, 198), 4.9f, 5 + Charges);
                }
            }
        }

        public void PotencyChargeEffectPulse(int Potency)
        {
            // AddPlayerMessage("Its Working");
            int ParticlesCount = Potency;

            if (Potency >= 21)
            {
                ParticlesCount = 20;
            }

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1 + (ParticlesCount); j++)
                {
                    ReverseParticleText("&W" + (char)Stat.RandomCosmetic(191, 198), 2.9f, (1 + ParticlesCount));
                }

                for (int k = 0; k < 1 + (ParticlesCount); k++)
                {
                    ReverseParticleText("&Y" + (char)Stat.RandomCosmetic(191, 198), 3.9f, (1 + ParticlesCount));
                }

                for (int l = 0; l < 1 + (ParticlesCount); l++)
                {
                    ReverseParticleText("&B" + (char)Stat.RandomCosmetic(191, 198), 4.9f, (1 + ParticlesCount));
                }
            }
        }

        public void LightningAnimation()
        {
            if (!OnWorldMap)
            {
                var _TextConsole = UI.Look._TextConsole;
                var Buffer = TextConsole.ScrapBuffer;

                var TargetCell = ParentObject.CurrentZone.GetRandomCell(3);
                var zone = TargetCell.ParentZone;

                // var SkyCell = zone.GetCell(TargetCell.X, 0);

                var SpawningLine = Zone.Line(0, 0, 79, 0);

                var CurrentLightningPosition = SpawningLine.GetRandomElement();
                var NextLightning = new Point(TargetCell.X, TargetCell.Y);

                var Lightning = new List<Point>();

                float DeltaX = NextLightning.X - CurrentLightningPosition.X;
                float DeltaY = NextLightning.Y - CurrentLightningPosition.Y;

                var RandomLength = Stat.Random(0.0f, 0.25f);

                NextLightning =
                    new Point(
                        CurrentLightningPosition.X +
                        (int)Math.Round(DeltaX * RandomLength, MidpointRounding.AwayFromZero),
                        CurrentLightningPosition.Y +
                        (int)Math.Round(DeltaY * RandomLength, MidpointRounding.AwayFromZero));

                var LightningSegment = Zone.Line(CurrentLightningPosition.X, CurrentLightningPosition.Y,
                    NextLightning.X, NextLightning.Y);

                Lightning.AddRange(LightningSegment);

                CurrentLightningPosition = NextLightning;

                // AddPlayerMessage("TargetCell X" + TargetCell.X);
                // AddPlayerMessage("TargetCell Y" + TargetCell.Y);

                while (CurrentLightningPosition.X != TargetCell.X || CurrentLightningPosition.Y != TargetCell.Y)
                {
                    if (Stat.Random(0, 100) <= 50 && CurrentLightningPosition.Y < TargetCell.Y)
                    {
                        RandomLength = Stat.Random(1.0f, 10.0f);

                        DeltaX = Stat.Random(-1.0f, 1.0f);
                        DeltaY = Stat.Random(0.0f, 1.0f);
                    }
                    else
                    {
                        DeltaX = TargetCell.X - CurrentLightningPosition.X;
                        DeltaY = TargetCell.Y - CurrentLightningPosition.Y;

                        RandomLength = Stat.Random(0.0f, 0.50f);
                    }

                    NextLightning =
                        new Point(
                            CurrentLightningPosition.X +
                            (int)Math.Round(DeltaX * RandomLength, MidpointRounding.AwayFromZero),
                            CurrentLightningPosition.Y +
                            (int)Math.Round(DeltaY * RandomLength, MidpointRounding.AwayFromZero));

                    LightningSegment = Zone.Line(CurrentLightningPosition.X, CurrentLightningPosition.Y,
                        NextLightning.X,
                        NextLightning.Y);

                    // AddPlayerMessage("CurrentLightningPosition X" + CurrentLightningPosition.X);
                    // AddPlayerMessage("CurrentLightningPosition Y" + CurrentLightningPosition.Y);
                    //
                    // AddPlayerMessage("NextLightning X" + NextLightning.X);
                    // AddPlayerMessage("NextLightning Y" + NextLightning.Y);
                    //
                    // AddPlayerMessage("TargetCell X" + TargetCell.X);
                    // AddPlayerMessage("TargetCell Y" + TargetCell.Y);

                    Lightning.AddRange(LightningSegment);

                    CurrentLightningPosition = NextLightning;
                }

                var SparkySparkyChars = new List<string>() { "\xf8", "*", "." };

                Buffer.Fill(0, 0, zone.Width, zone.Height, 'Û', 'W');

                _TextConsole.DrawBuffer(Buffer);

                System.Threading.Thread.Sleep(90);

                Core.XRLCore.Core.RenderMapToBuffer(Buffer);

                for (var index = 0; index < Lightning.Count; index++)
                {
                    var point = Lightning[index];
                    var cell = zone.GetCell(point);

                    var SteamObj = GameObject.Create("Steam");
                    var SteamProps = SteamObj.GetPart<Gas>();
                    SteamProps.Density = 10;

                    cell?.AddObject(SteamObj);

                    var Jaggeds = Stat.Random(1, 7);

                    char DisplayBeam;

                    if (index % 2 == 0)
                    {
                        DisplayBeam = '/';
                    }
                    else
                    {
                        DisplayBeam = '\\';
                    }

                    if (cell != null && cell.X != null && cell.Y != null)
                    {
                        Buffer.Goto(cell.X, cell.Y);
                        Buffer.Write("&B^A" + DisplayBeam);
                    }

                    var SparkyBeam = cell.GetRandomLocalAdjacentCell();
                    Buffer.Goto(SparkyBeam.X, SparkyBeam.Y);
                    Buffer.Write("&A" + SparkySparkyChars.GetRandomElement());
                }

                _TextConsole.DrawBuffer(Buffer);
                // System.Threading.Thread.Sleep(180);

                CombatJuice.cameraShake(Stat.Random(0.500f, 1.00f), Async: true);

                EMPMutationPart.EMP(TargetCell, 3, 1, true);
                TargetCell.TemperatureChange(150, null, true);
            }
        }
    }
}