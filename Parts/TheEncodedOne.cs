using System.Collections.Generic;
using HarmonyLib;
using XRL.World.Parts.Mutation;
using System;
using ConsoleLib.Console;
using Genkit;
using UnityEngine;
using XRL.Rules;
using XRL.UI.ObjectFinderClassifiers;
using XRL.World.Anatomy;
using XRL.World.Capabilities;
using XRL.World.Effects;
using XRL.World.Parts.Skill;
using LifeDrain = XRL.World.Parts.Mutation.LifeDrain;


namespace XRL.World.Parts
{
    public partial class TheEncodedOne : IScribedPart
    {
        public void DanceDestroyDerail()
        {
            var MehRandomAction = Stat.Random(1, 3);

            if (MehRandomAction == 1 && !ParentObject.HasEffect<DisableMeh>())
            {
                var eLocation = CellListParticular.GetRandomElement();
                var eCell = ParentObject.CurrentZone.GetCell(eLocation);

                ParentObject.CellTeleport(eCell);
                ParentObject.ApplyEffect(new DisableMeh());
            }
            else if (MehRandomAction == 2)
            {
                ParentObject.ApplyEffect(new ChargingSuperAttack() { Duration = 2, radius = Stat.Random(1, 14) });
            }
        }

        public override bool Render(RenderEvent E)
        {
            E.WantsToPaint = true;

            return base.Render(E);
        }

        public override void OnPaint(ScreenBuffer buffer)
        {
            var Z = ParentObject.CurrentZone;
            if (!Z.IsActive())
            {
                return;
            }

            foreach (var L in WarningCells)
            {
                var C = Z.GetCell(L);
                if (!C.IsVisible() || C == ParentObject.CurrentCell)
                {
                    continue;
                }

                var CHR = buffer[C];

                CHR.TileBackground = CHR.Background = The.Color.DarkRed;
                CHR.TileForeground = CHR.Detail = The.Color.Red;
            }


            base.OnPaint(buffer);
        }

        public List<Cell> GetRing(int? radius = null)
        {
            return ParentObject.CurrentCell.PickRing(ParentObject.CurrentCell.Point, radius ?? ringStage, Range: 1);
        }


        public void SuperDuperUltraAttack(int? radius = null)
        {
            WarningCells.Clear();

            var MehCell1 = GetRing(radius);

            List<Cell> ListCell4 = new List<Cell>();

            var FullZone = ParentObject.CurrentZone.GetCells();


            foreach (var C in ParentObject.CurrentCell.IterateAdjacent(7))
            {
                if (!C.IsVisible())
                {
                    continue;
                }

                ListCell4.Add(C);
            }


            if (MehCell1 != null && PhaseFlame)
                foreach (var C in MehCell1)
                {
                    C?.Flameburst();
                    C?.GetCombatTarget()
                        ?.TakeDamage("4d6".RollCached(), Attributes: "Heat", Message: "from %t flames.");
                    
                    if (ParentObject.CurrentZone.CountObjects("Blazing Firefly") <= BlazingFireFlyLimit)
                    {
                        if (C != null && C.IsVisible() && Stat.Random(1, 100) < 10)
                        {
                            var BlazingFireFly = C.AddObject("Blazing Firefly");
                        }
                    }
                }
            if (Checkerboarding && PhaseFlame)
            {
                foreach (var C in FullZone)
                {
                    if (C.X % 2 == 0 && C.Y % 2 == 0 && C.IsVisible())
                    {
                        C?.Flameburst();
                        C?.GetCombatTarget()
                            ?.TakeDamage("4d6".RollCached(), Attributes: "Heat", Message: "from %t flames.");
                    }
                }
            }
            else if (MehCell1 != null && PhaseFrost)
                foreach (var C in MehCell1)
                {
                    RemoveAdds(C, "Blazing Firefly");
                    C?.GetCombatTarget()?.Push(ParentObject.GetDirectionToward(C?.GetCombatTarget()), 1000, 2);
                    C?.TileParticleBlip("Creatures/sw_crystal1.bmp", "&b", "B", 3);
                    var gameObject = C?.AddObject("Ice Crystal");

                    if (gameObject.IsValid())
                    {
                        gameObject.ApplyEffect(new ExplodingIceCrystal());
                    }
                }

            if (Checkerboarding && PhaseFrost)
            {
                foreach (var C in FullZone)
                {
                    RemoveAdds(C, "Blazing Firefly");
                    if (C.X % 2 == 0 && C.Y % 2 == 0 && C.IsVisible())
                    {
                        C?.GetCombatTarget()?.Push(ParentObject.GetDirectionToward(C?.GetCombatTarget()), 1000, 2);
                        C?.TileParticleBlip("Creatures/sw_crystal1.bmp", "&b", "B", 3);
                        var gameObject = C?.AddObject("Ice Crystal");

                        if (gameObject.IsValid())
                        {
                            gameObject.ApplyEffect(new ExplodingIceCrystal());
                        }
                    }
                }
            }
            else if (PhaseShock && !ParentObject.CurrentZone.HasObject("Spherical Static"))
            {
                foreach (var L in CellListParticular)
                {
                    var C = ParentObject.CurrentZone.GetCell(L);

                    if (!C.HasObject("Spherical Static"))
                    {
                        var Obj = C.AddObject("Spherical Static");

                        var objP = Obj.GetPart<Temporary>();
                        objP.Duration = Stat.Random(21, 84);
                    }
                }
            }
            else if (ListCell4 != null && PhaseUnreal)
            {
                foreach (var C in ListCell4)
                {
                    if (C.IsVisible() && Stat.Random(1, 100) < 10)
                    {
                        var fungibility = Stat.Random(1, 2);
                        var laughs = Stat.Random(1, 10);

                        C.TelekinesisBlip();

                        if (ParentObject.CurrentZone.CountObjects("FalseEncodedEater") <= FalseMehCloneLimit)
                        {
                            var FalseEater = C.AddObject("FalseEncodedEater");

                            if (fungibility == 1)
                            {
                                FalseEater.Brain.Allegiance.Calm = true;
                                FalseEater.Brain.Allegiance.Hostile = false;
                                
                                if (laughs == 1)
                                {
                                    SoundManager.PlaySound("mehlaughter" + Stat.Random(1, 2), 10.0f, 1.0f);
                                    AddPlayerMessage("{{red|" + FalseEater.DisplayName + " is laughing at you ...}}");
                                }
                            }
                            else if (fungibility == 2)
                            {
                                if (FalseEater.IsValid())
                                {
                                    FalseEater.AddPart<NoHostility>();
                                    FalseEater.Brain.Allegiance.Calm = true;
                                    FalseEater.Brain.Allegiance.Hostile = false;
                                    FalseEater.Brain.Wanders = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (FullZone != null && PhaseDerv)
                foreach (var C in FullZone)
                {
                    RemoveAdds(C, "FalseEncodedEater");
                }
            else if (PhaseEsper)
            {
                for (int i = 0; i < 4; i++)
                {
                    var eLocation = CellListParticular.GetRandomElement();
                    var eCell = ParentObject.CurrentZone.GetCell(eLocation);

                    if (!eCell.HasObjectWithBlueprint("Singularity"))
                        eCell.AddObject("Singularity");
                }
            }
            else if (PhaseHarbinger)
            {
                if (FullZone != null && PhaseDerv)
                    foreach (var C in FullZone)
                    {
                        RemoveAdds(C, "FalseEncodedEater");
                    }
                
                var DeterminePhaseAction = Stat.Random(1, 4);

                if (DeterminePhaseAction == 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var eLocation = CellListParticular.GetRandomElement();
                        var eCell = ParentObject.CurrentZone.GetCell(eLocation);

                        if (!eCell.HasObjectWithBlueprint("Singularity"))
                            eCell.AddObject("Singularity");
                    }
                }

                if (DeterminePhaseAction == 2)
                {
                    var DeterminedCell = ListCell4.GetRandomElement();

                    if (DeterminedCell != null)
                    {
                        ParentObject.Physics.ApplyDischarge(DeterminedCell.GetRandomLocalAdjacentCell(), DeterminedCell,
                            2, "7d7".Roll(), null,
                            null, ParentObject.GetNearestVisibleObject(), null);
                    }
                }

                if (DeterminePhaseAction == 3)
                {
                    var C = ListCell4.GetRandomElement();

                    foreach (var Cell in C.PickRing(C.Point, Stat.Random(3, 7), 1))
                    {
                        Cell?.GetCombatTarget()
                            ?.Push(ParentObject.GetDirectionToward(Cell?.GetCombatTarget()), 1000, 2);
                        var gameObject = Cell?.AddObject("Ice Crystal");

                        if (gameObject.IsValid())
                        {
                            gameObject.ApplyEffect(new ExplodingIceCrystal());
                        }
                    }
                }

                if (DeterminePhaseAction == 4)
                {
                    var C = ListCell4.GetRandomElement();

                    foreach (var Cell in C.PickRing(C.Point, Stat.Random(3, 7), 1))
                    {
                        Cell?.Flameburst();
                        Cell?.GetCombatTarget()
                            ?.TakeDamage("4d6".RollCached(), Attributes: "Heat", Message: "from %t flames.");
                    }
                }

                ringStage = 0;
                ParentObject.UseEnergy(1000);
            }

            if (!ParentObject.Brain.Mobile)
                ParentObject.Brain.Mobile = true;

            if (!ParentObject.HasPart<Teleportation>())
                ParentObject.GetPart<Mutations>().AddMutation(new Teleportation());

            Checkerboarding = false;
        }


        public void InitiateTeleEater()
        {
            var bossEruptCell = ParentObject.CurrentZone.GetCell(x: 43, y: 11);
            ParentObject.CellTeleport(bossEruptCell);

            var Chair = bossEruptCell.GetFirstObjectPart<Chair>();

            if (!PhaseUnreal && !PhaseDerv)
            {
                if (Chair != null)
                {
                    Chair.SitDown(ParentObject);
                }

                ParentObject.Brain.Mobile = false;
                if (ParentObject.HasPart<Teleportation>())
                    ParentObject.GetPart<Mutations>().RemoveMutation(ParentObject.GetPart<Teleportation>());
            }
            else
            {
                if (Chair != null)
                {
                    Chair.SitDown(ParentObject);
                }

                if (ParentObject.HasPart<Teleportation>())
                    ParentObject.GetPart<Mutations>().RemoveMutation(ParentObject.GetPart<Teleportation>());
            }
        }

        // harbinger be like ... fuck you ...

        public void HarbingerProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.RemoveMutation(ParentObject.GetPart<SunderMind>());
            bossMutations.RemoveMutation(ParentObject.GetPart<MassMind>());

            bossMutations.AddMutation(new Disintegration());
            ParentObject.SetIntProperty("MutationBonus_Disintegration", -10);
            
            bossMutations.AddMutation(new QuantumFugue());
            bossMutations.AddMutation(new TimeDilation());

            ParentObject.RemoveEffect<MemberOfPsychicBattle>();
            ParentObject.RemovePart<SunderMind>();

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 100);

            ShiftBlades();
        }

        public void EsperWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();
            var bossSkills = ParentObject.GetPart<Skills>();

            bossMutations.RemoveMutation(ParentObject.GetPart<Battoujutsu>());

            bossMutations.AddMutation(new WillForce());
            bossMutations.AddMutation(new MassMind());
            bossMutations.AddMutation(new MentalMirror());
            bossMutations.AddMutation(new RepellingForce());

            bossMutations.AddMutation(new SunderMind());
            ParentObject.SetIntProperty("MutationBonus_SunderMind", -10);


            bossSkills.AddSkill(new Tactics());
            bossSkills.AddSkill(new Tactics_Camouflage());
            bossSkills.AddSkill(new Tactics_Charge());
            bossSkills.AddSkill(new Tactics_DeathFromAbove());
            bossSkills.AddSkill(new Tactics_Juke());
            bossSkills.AddSkill(new Tactics_Hurdle());
            bossSkills.AddSkill(new Tactics_Throwing());

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 0);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 0);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 0);

            if (!ParentObject.HasPart<Teleportation>())
            {
                bossMutations.AddMutation(new Teleportation());
            }

            ShiftBlades();
        }

        public void DervishWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();
            var bossSkills = ParentObject.GetPart<Skills>();

            bossMutations.RemoveMutation(ParentObject.GetPart<LoversGaze>());
            bossMutations.RemoveMutation(ParentObject.GetPart<LifeDrain>());
            bossMutations.RemoveMutation(ParentObject.GetPart<Confusion>());

            bossMutations.AddMutation(new HeightenedSpeed());
            bossMutations.AddMutation(new HeightenedAgility());
            bossMutations.AddMutation(new TwoHearted());
            bossMutations.AddMutation(new WillForce());
            bossMutations.AddMutation(new Battoujutsu());

            bossSkills.AddSkill(new Acrobatics());
            bossSkills.AddSkill(new Acrobatics_Dodge());
            bossSkills.AddSkill(new Acrobatics_Jump());
            bossSkills.AddSkill(new Acrobatics_SwiftReflexes());
            bossSkills.AddSkill(new Acrobatics_Tumble());

            bossSkills.AddSkill(new ShortBlades());
            bossSkills.AddSkill(new ShortBlades_Bloodletter());
            bossSkills.AddSkill(new ShortBlades_Hobble());
            bossSkills.AddSkill(new ShortBlades_PointedCircle());
            bossSkills.AddSkill(new ShortBlades_Puncture());
            bossSkills.AddSkill(new ShortBlades_Rejoinder());
            bossSkills.AddSkill(new ShortBlades_Jab());

            bossSkills.AddSkill(new Multiweapon_Fighting());
            bossSkills.AddSkill(new Multiweapon_Expertise());
            bossSkills.AddSkill(new Multiweapon_Flurry());
            bossSkills.AddSkill(new Multiweapon_Mastery());

            bossSkills.AddSkill(new Endurance_ShakeItOff());
            bossSkills.AddSkill(new Endurance_Weathered());

            bossSkills.AddSkill((new Discipline_Conatus()));

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 25);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 25);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 25);

            ShiftBlades();
        }

        public void PsyOpWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.RemoveMutation(ParentObject.GetPart<ForceWall>());
            bossMutations.RemoveMutation(ParentObject.GetPart<ForceBubble>());
            bossMutations.RemoveMutation(ParentObject.GetPart<LightManipulation>());
            bossMutations.RemoveMutation(ParentObject.GetPart<ElectromagneticPulse>());

            bossMutations.AddMutation(new LoversGaze());
            bossMutations.AddMutation(new Confusion());
            bossMutations.AddMutation(new LifeDrain());

            ParentObject.SetIntProperty("MutationBonus_LifeDrain", -7);

            StatShifter.SetStatShift(ParentObject, "HeatResistance", -50);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", -50);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", -50);

            ShiftBlades();

            var ChannelRifle = ParentObject.Body.FindObjectByBlueprint("V77 Channel Rifle");

            ChannelRifle?.UnequipAndRemove();
        }

        public void ShockWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.RemoveMutation(ParentObject.GetPart<EatersInterdiction>());
            bossMutations.RemoveMutation(ParentObject.GetPart<Cryokinesis>());
            bossMutations.RemoveMutation(ParentObject.GetPart<FrostWebs>());
            bossMutations.RemoveMutation(ParentObject.GetPart<FreezeBreath>());

            bossMutations.AddMutation(new ForceWall());
            bossMutations.AddMutation(new ForceBubble());
            bossMutations.AddMutation(new LightManipulation());
            bossMutations.AddMutation(new ElectromagneticPulse());

            ParentObject.SetIntProperty("MutationBonus_ForceWall", -8);
            ParentObject.SetIntProperty("MutationBonus_ForceBubble", -7);

            ParentObject.SetIntProperty("MutationBonus_LightManipulation", -7);

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 25);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 25);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 100);

            ParentObject.Physics.FreezeTemperature = -99999;

            GameObject ChannelRifle = GameObjectFactory.create("V77 Channel Rifle");

            ParentObject.ForceEquipObject(ChannelRifle, "Missile Weapon");

            ShiftBlades();
            ParentObject.Inventory.AddObject(("Antimatter Cell"));
        }

        public void FrostWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.RemoveMutation(ParentObject.GetPart<Pyrokinesis>());
            bossMutations.RemoveMutation(ParentObject.GetPart<FlamingRay>());
            bossMutations.RemoveMutation(ParentObject.GetPart<HeatAbsorption>());
            bossMutations.RemoveMutation(ParentObject.GetPart<EatersKindle>());

            bossMutations.AddMutation(new EatersInterdiction());
            bossMutations.AddMutation(new Cryokinesis());
            bossMutations.AddMutation(new FrostWebs());
            bossMutations.AddMutation(new FreezeBreath());

            StatShifter.SetStatShift(ParentObject, "HeatResistance", -25);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 100);

            ShiftBlades();

            var Flamethrower = ParentObject.Body.FindObjectByBlueprint("Flamethrower");

            Flamethrower?.UnequipAndRemove();
        }

        public void ScorchWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.AddMutation(new Pyrokinesis());
            bossMutations.AddMutation(new FlamingRay());
            bossMutations.AddMutation(new HeatAbsorption());
            bossMutations.AddMutation(new EatersKindle());

            StatShifter.SetStatShift(ParentObject, "ColdResistance", -25);
            StatShifter.SetStatShift(ParentObject, "HeatResistance", 100);


            GameObject eqFlameThrower = GameObjectFactory.create("Flamethrower");

            eqFlameThrower.LiquidVolume.Volume = 1;
            ParentObject.ForceEquipObject(eqFlameThrower, "Missile Weapon");

            ShiftBlades();
            PhaseFlame = true;
        }

        public void ShiftBlades()
        {
            var WieldingList = ParentObject.Body.GetEquippedObjects();
            var eBody = ParentObject.Body;

            foreach (var Limb in eBody.LoopParts())
            {
                if (Limb.Type == "Hand")
                {
                    if (Limb.Equipped?.Blueprint != "Prismatic Mantis Blade")
                    {
                        GameObject eqMantisBlades = GameObjectFactory.create("Prismatic Mantis Blade");

                        AddPlayerMessage("The Encoded One's " + eqMantisBlades.it + " redeploys itself.");
                        ParentObject.ForceEquipObject(eqMantisBlades, Limb);
                    }
                }
            }

            foreach (var O in WieldingList)
            {
                if (O.Blueprint == "Prismatic Mantis Blade")
                {
                    AdaptiveMantisBlades(O);
                }
            }
        }

        public void AdaptiveMantisBlades(GameObject Object)
        {
            var pMeleeWeapon = Object.GetPart<MeleeWeapon>();

            if (PhaseFlame)
            {
                Object.DisplayName = "{{blaze|prismatic}} mantis-blade";
                pMeleeWeapon.Attributes = "Fire";
            }

            if (PhaseFrost)
            {
                Object.DisplayName = "{{icy|prismatic}} mantis-blade";
                pMeleeWeapon.Attributes = "Cold";
            }

            if (PhaseShock)
            {
                Object.DisplayName = "{{overloaded|prismatic}} mantis-blade";
                pMeleeWeapon.Attributes = "Electric";
            }

            if (PhaseUnreal)
            {
                Object.DisplayName = "{{psychalflesh|prismatic}} mantis-blade";
                pMeleeWeapon.Attributes = "Drain";
            }

            if (PhaseDerv)
            {
                Object.DisplayName = "{{psionic|prismatic}} mantis-blade";
                pMeleeWeapon.Attributes = "Psionic";
            }

            if (PhaseEsper)
            {
                Object.DisplayName = "{{phase-harmonic|prismatic}} mantis-blade";
                Object.AddPart<OmniphaseObject>();
                pMeleeWeapon.Attributes = "Mental";
            }

            if (PhaseHarbinger)
            {
                Object.DisplayName = "{{extradimensional|prismatic}} mantis-blade";
                Object.AddPart<OmniphaseObject>();
                pMeleeWeapon.Attributes = "Mental,Psionic,Drain";
            }
        }

        public void MehSaysSomething()
        {
            var TalkFactor = Stat.Random(1, 100);

            var textpre = "You hear Mehrashir say, {{red|\"";
            var textpost = "\"}}";

            if (PhaseFlame && TalkFactor < 30)
            {
                var text = ScrotumFire.PeekOne();
                AddPlayerMessage(textpre + text + textpost);
                ParentObject.ParticleText(text);
            }

            if (PhaseFrost && TalkFactor < 30)
            {
                var text = ScrotumFrost.PeekOne();
                AddPlayerMessage(textpre + text + textpost);
                ParentObject.ParticleText(text);
            }

            if (PhaseShock && TalkFactor < 30)
            {
                var text = ScrotumShock.PeekOne();
                AddPlayerMessage(textpre + text + textpost);
                ParentObject.ParticleText(text);
            }

            if (PhaseUnreal && TalkFactor < 30)
            {
                var text = ScrotumTrickery.PeekOne();
                AddPlayerMessage(textpre + text + textpost);
                ParentObject.ParticleText(text);
            }
        }
        
        public void RemoveAdds(Cell C, string ObjectBluePrint)
        {
            if (C.HasObjectWithBlueprint(ObjectBluePrint))
            {
                var Objects = C.GetObjectsInCell();
                foreach (var O in Objects)
                {
                    if (O.Blueprint == ObjectBluePrint)
                    {
                        O.TeleportSwirl();
                        O.Obliterate();
                    }
                }
            }
        }
        
        
    }
}