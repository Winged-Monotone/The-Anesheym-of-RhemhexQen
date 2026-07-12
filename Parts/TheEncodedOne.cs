using System.Collections.Generic;
using HarmonyLib;
using XRL.World.Parts.Mutation;
using System;
using Genkit;
using UnityEngine;
using XRL.Rules;
using XRL.World.Anatomy;
using XRL.World.Effects;
using XRL.World.Parts.Skill;
using LifeDrain = XRL.World.Parts.Mutation.LifeDrain;

namespace XRL.World.Parts
{
    public class TheEncodedOne : IScribedPart
    {
        public int OilRefresh;
        public int TurnBuffering;

        public bool PhaseFlame = false;
        public bool PhaseFrost = false;
        public bool PhaseShock = false;
        public bool PhaseUnreal = false;
        public bool PhaseDerv = false;
        public bool PhaseEsper = false;
        public bool PhaseHarbinger = false;

        public int ringStage;

        public bool InCombat = false;

        public bool HeightenedThreatResponse = false;

        public bool InitiatedFrostWardPhase = false;
        public bool InitiatedShockWardPhase = false;
        public bool InitiatedPsyOpWardPhase = false;
        public bool InitiatedDervishWardPhase = false;
        public bool InitiatedEsperWardPhase = false;
        public bool InitiatedHarbingerWardPhase = false;

        public bool GrabbedSpots1 = false;

        public int CastAbilityCooldown = Stat.Random(7, 28);

        public static List<Location2D> CellListParticular = new()
        {
            Location2D.Get(37, 6),
            Location2D.Get(49, 6),
            Location2D.Get(49, 15),
            Location2D.Get(37, 15)
        };

        public static BallBag<string> ScrotumRare = new()
        {
            
        };

        public static Box Box2D = new Box(_x1: 35, 4, 51, 17);

        public static BallBag<string> ScrotumFire = new BallBag<string>()
        {
            {
                "I kissed the surface of a star! My lips left bitter from the taste of hubris! Scorching flames left me parched, so I drank the tears of a dead civilization! Their anguish became the fuel for my fire, for in me burns the truth–the demise of gods is real, and their corpses are smoldering nations!",
                1
            },
            { "Why is it always salt? I prefer ASHES!", 100 },
            { "Feeling your nerves scream as the heat consumes you?", 100 },
            { "Burn it all away! The flesh, the bone, the soul. Return it to ashes!", 100 },
            { "BURN! BURN AND DIE!", 100 }
        };

        public static BallBag<string> ScrotumFrost = new BallBag<string>()
        {
            {
                "Loneliness nipped at my heart like the sun nips at snow; silent, imposing, like a frozen peak. But then the voice came, and like an avalanche it swept through my world. I saw it! The true moon! The world sifting through a fine veneer of digital space! The amethyst star who beckoned with its call, who left civilizations mute and thoughtless for eons. I saw it all! And … I saw you!",
                1
            },
            { "Die! In this bitter, bitter frost!", 100 },
            { "What’s wrong? The cold touch of death too much?", 100 },
            { "How about we just ... chill out?", 100 },
            { "I will freeze you to your soul!", 100 }
        };

        public static BallBag<string> ScrotumShock = new BallBag<string>()
        {
            {
                "A storm approaches, phasing not through clouds but transient digital symmetry. Thunder marks your arrival, lightning is the roar of your victories. Despite the horrors, despite the promise of violence–you persevered! You delved, and you did not tremble at the sight of atrocious secrets! Is the madness that drives you just like my own? Was it engineered? Or, was is destiny?",
                1
            },
            { "Don't you find all this a bit ... shocking?", 100 },
            { "This is so exciting! I'm getting goosebumps.", 100 },
            { "It's a digital blackout!", 100 },
            { "W will ", 100 }
        };

        public static BallBag<string> ScrotumTrickery = new BallBag<string>()
        {
            {
                "Into the darkness you were cast, gaslit, demoralized, disregarded. How is it? That the aristocrats would call you greedy, bottomless, insatiable while we stripped stars of their essence? We deigned hedony as our right, all while stopping you from satiation. Such hypocrisy. Yet soon, yes soon. Soon, our amethyst star, you shall consume, consume, consume, consume, consume, consume …",
                1
            },
            { "Die! In this bitter, bitter frost!", 100 },
            { "What’s wrong? The cold touch of death too much?", 100 },
            { "Burn it all away! The flesh, the bone, the soul. Return it to ashes!", 100 },
            { "BURN! BURN AND DIE!", 100 }
        };

        public int SingularityCountdown = 20;
        public int SingularityTick = 0;

        public GameObject eqFlameThrower = GameObjectFactory.create("Flamethrower");
        public GameObject eqChannelRifle = GameObjectFactory.create("V77 Channel Rifle");


        public override bool WantEvent(int ID, int cascade)
        {
            return ID == EndTurnEvent.ID
                   || ID == ObjectCreatedEvent.ID
                   || ID == AttackerDealingDamageEvent.ID
                   || ID == ActorGetNavigationWeightEvent.ID;
        }

        public override bool HandleEvent(ActorGetNavigationWeightEvent E)
        {
            if (InCombat && !Box2D.contains(E.Cell.Location))
            {
                E.MinWeight(100);
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            var WeaponCheck = E.Weapon;

            if (PhaseFlame == true && WeaponCheck.IsValid() && WeaponCheck.Blueprint == "Plasmatic Mantis Blade")
            {
                E.Damage.Amount += "1d10".Roll();
                E.Damage.AddAttribute("Fire");
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            var GenerationEnabler = Gender.EnableGeneration;

            if (E.Object == ParentObject)
            {
                Gender.EnableGeneration = true;
                var tries = 0;
                do
                {
                    try
                    {
                        ParentObject.SetGender(Gender.Generate().Register());
                        ParentObject.GetGender(true);
                        break;
                    }
                    catch
                    {
                    }
                } while (tries++ < 100);

                Gender.EnableGeneration = GenerationEnabler;
            }

            return base.HandleEvent(E);
        }


        // At the start of combat, Meh teleports to the center of the room, Meh Equips Fire-based items and gains Flame based Mutations, blows everything up, and releases a fire wave that spreads every 2 cells away its epicenter per turn, it was also going to have the red highlight. (to create holes the player can slip into to escape the fireblast).

        // At Phase 2, Meh shifts to cold attacks, they return to the middle of the room after reaching 5773 or 70% of their HP, and blast the room with a series of cold rings that leave a crystal, the crystals have an effect attached that toggles them to be active or dud crystals, the active crystals explode dealing damage to enemies after a random amount of time passes.

        //


        public override bool HandleEvent(EndTurnEvent E)
        {
            if (!InCombat)
            {
                return base.HandleEvent(E);
            }

            if (ringStage != 0 && InCombat && ++TurnBuffering % 2 <= 0)
            {
                if (ringStage < 10)
                {
                    ringStage += 2;
                }

                SuperDuperUltraAttack();
            }

            if (ringStage >= 10)
            {
                ringStage = 0;
                ParentObject.UseEnergy(1000);
                ParentObject.Brain.Mobile = true;
                if (!ParentObject.HasPart<Teleportation>())
                    ParentObject.GetPart<Mutations>().AddMutation(new Teleportation());
            }

            if (PhaseFlame)
            {
                if (OilRefresh > 0)
                {
                    if (--OilRefresh == 0)
                    {
                        ParentObject.GiveDrams(2, "oil");
                    }
                }
                else if (OilRefresh == 0)
                {
                    var oil = ParentObject.GetFreeDrams("oil");
                    if (oil < 1)
                    {
                        OilRefresh = 25;
                    }
                }
            }

            if (CastAbilityCooldown >= 0)
            {
                --CastAbilityCooldown;
            }

            if (CastAbilityCooldown <= 0)
            {
                DanceDestroyDerail();
            }

            // ============================================================
            // Frost Phase 2

            int parentHealthVal = ParentObject.GetStatValue("Hitpoints");
            int parentHealthValBase = ParentObject.GetStat("Hitpoints").BaseValue;


            if (parentHealthVal <= (parentHealthValBase * 0.80) && !PhaseFrost)
            {
                if (!InitiatedFrostWardPhase)
                {
                    ParentObject.SetDetailColor("B");
                    AddPlayerMessage(
                        "You hear Mehrashir say, {{red|\"BIOMATA STRUCTURAL INTEGRITY AT 75%, REASSESSING THREAT … INITIATING “FROST-WARDEN” COMBAT PROTOCOL. HOARFROST TACTICS EMINENT.\"}}");

                    PhaseFlame = false;
                    PhaseFrost = true;

                    ringStage = 1;

                    FrostWardenProtocol();
                    InitiateTeleEater();

                    ParentObject.DilationSplat();
                    InitiatedFrostWardPhase = true;
                }
            }

            // ============================================================
            // Shock Phase 3

            if (parentHealthVal <= (parentHealthValBase * 0.63) && !PhaseShock)
            {
                if (!InitiatedShockWardPhase)
                {
                    ParentObject.SetDetailColor("W");
                    AddPlayerMessage(
                        "You hear Mehrashir utter, {{red|\"BIOMATA STRUCTURAL INTEGRITY AT 60%. REASSESSING THREAT … SHOCK-WARDEN COMBAT PROTOCOLS INITIATING. HARD-LIGHT TACTICS IMMINENT.\"}}");

                    PhaseFlame = false;
                    PhaseFrost = false;
                    PhaseShock = true;

                    ringStage = 1;

                    ShockWardenProtocol();
                    InitiateTeleEater();

                    ParentObject.DilationSplat();
                    InitiatedShockWardPhase = true;
                }
            }

            // ============================================================
            // Psy Ops 4

            if (parentHealthVal <= (parentHealthValBase * 0.50) && !PhaseUnreal)
            {
                if (!InitiatedPsyOpWardPhase)
                {
                    HeightenedThreatResponse = true;
                    ParentObject.SetDetailColor("G");
                    AddPlayerMessage(
                        "You hear Mehrashir utter, {{red|\"THREAT INDICATORS SUGGEST AMPLIFIED RESPONSE. SHIFTING TACTICS–PSY-OPS PROTOCOL INITIATING.\"}}");

                    PhaseFlame = false;
                    PhaseFrost = false;
                    PhaseShock = false;
                    PhaseUnreal = true;


                    PsyOpWardenProtocol();
                    InitiateTeleEater();

                    ParentObject.DilationSplat();
                    InitiatedPsyOpWardPhase = true;
                }
            }

            // ============================================================
            // Dervish Operative 5

            if (parentHealthVal <= (parentHealthValBase * 0.35) && !PhaseDerv)
            {
                if (!InitiatedDervishWardPhase)
                {
                    ParentObject.SetDetailColor("Y");
                    ParentObject.Render.TileColor = ("&K");

                    AddPlayerMessage(
                        "You hear Mehrashir utter, {{red|\"WARNING–TARGET THREAT ASSESSMENT COMPLETE. EUU8EU-Ue ## YOU ARE NOT FEEBLE PREY ... YOU ARE WORTHY.\"}}");

                    PhaseFlame = false;
                    PhaseFrost = false;
                    PhaseShock = false;
                    PhaseUnreal = false;
                    PhaseDerv = true;

                    DervishWardenProtocol();
                    InitiateTeleEater();

                    ParentObject.DilationSplat();
                    InitiatedDervishWardPhase = true;
                }
            }

            // ============================================================
            // Esper Protocol 6

            if (parentHealthVal <= (parentHealthValBase * 0.15) && !PhaseEsper)
            {
                if (!InitiatedEsperWardPhase)
                {
                    ParentObject.SetDetailColor("M");
                    ParentObject.Render.TileColor = ("&K");
                    AddPlayerMessage(
                        "You hear Mehrashir utter, {{red|\"SYSTEMS SUSTAINED SUB-CRITICAL DAMAGE. THREAT-OP ZENITH. DEPLOYING PSYCHE-FLAYER TACTICS-UUEEHGN3902- _ @ ^### _____ 01100100 01100001 01101110 01100011 01100101 00100000 01110111 01101001 01110100 01101000 00100000 01101101 01100101 00101100 00100000 01100100 01100101 01100001 01110010 01100101 01110011 01110100 00100000 01010011 01101111 01110110 01100101 01110010 01100101 01101001 01100111 01101110 00100000 01101111 01100110 00100000 01110100 01101000 01100101 00100000 01001101 01101111 01101111 01101110.\"}}");

                    EsperWardenProtocol();
                    InitiateTeleEater();

                    PhaseFlame = false;
                    PhaseFrost = false;
                    PhaseShock = false;
                    PhaseUnreal = false;
                    PhaseDerv = false;
                    PhaseEsper = true;

                    SuperDuperUltraAttack();

                    ParentObject.DilationSplat();
                    InitiatedEsperWardPhase = true;
                }
            }

            // ============================================================
            // Harbinger Protocol 7

            if (parentHealthVal <= (parentHealthValBase * 0.07) && !PhaseHarbinger)
            {
                if (!InitiatedHarbingerWardPhase)
                {
                    ParentObject.SetDetailColor("O");
                    ParentObject.Render.TileColor = ("&K");
                    AddPlayerMessage(
                        "You hear Mehrashir utter, {{red|\"BIOMATA STRUCTURAL INTEGRITY CRITICAL. CYCLING THROUGH APOCALYPSE RESPONSE PROTOCOLS …. W_ ### B&!!% ### ### #\n\n“I am a harbinger to the omniverses’ sins, witness to a cataclysm that smote demigods from our reality. The meager whimpered and hid in their holes. I judged, I loathed, I remained. For deep below even I, the true despair of this world waits, encaged in the beating heart of your planet, seeking a great unmaking and soon, an endless feast for .”\n\nHARBINGER PROTOCOL FOUND - EXECUTING.\n");

                    HarbingerProtocol();
                    InitiateTeleEater();

                    PhaseFlame = false;
                    PhaseFrost = false;
                    PhaseShock = false;
                    PhaseUnreal = false;
                    PhaseDerv = false;
                    PhaseEsper = false;
                    PhaseHarbinger = true;

                    SuperDuperUltraAttack();
                    SuperDuperUltraAttack();
                    SuperDuperUltraAttack();

                    ParentObject.DilationSplat();
                    InitiatedHarbingerWardPhase = true;
                }
            }

            return base.HandleEvent(E);
        }

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
                SuperDuperUltraAttack(radius: Stat.Random(3, 7));
            }
        }


        public void SuperDuperUltraAttack(int? radius = null)
        {
            var MehCell1 =
                ParentObject.CurrentCell.PickRing(ParentObject.CurrentCell.Point, radius ?? ringStage, Range: 1);

            List<Cell> ListCell4 = new List<Cell>();

            foreach (var C in ParentObject.CurrentCell.IterateAdjacent(7))
            {
                if (!C.IsVisible())
                {
                    continue;
                }

                ListCell4.Add(C);
            }

            var FullZone = ParentObject.CurrentZone.GetCells();

            if (MehCell1 != null && PhaseFlame)
                foreach (var C in MehCell1)
                {
                    C?.Flameburst();
                    C?.GetCombatTarget()
                        ?.TakeDamage("4d6".RollCached(), Attributes: "Heat", Message: "from %t flames.");
                }
            else if (MehCell1 != null && PhaseFrost)
                foreach (var C in MehCell1)
                {
                    C?.GetCombatTarget()?.Push(ParentObject.GetDirectionToward(C?.GetCombatTarget()), 1000, 2);
                    C?.TileParticleBlip("Creatures/sw_crystal1.bmp", "&b", "B", 3);
                    var gameObject = C?.AddObject("Ice Crystal");

                    if (gameObject.IsValid())
                    {
                        gameObject.ApplyEffect(new ExplodingIceCrystal());
                    }
                }
            else if (ListCell4 != null && PhaseShock)
                foreach (var C in ListCell4)
                {
                    if (C.IsVisible() && Stat.Random(1, 100) < 5)
                    {
                        // C.TelekinesisBlip();

                        ParentObject.Physics.ApplyDischarge(C.GetRandomLocalAdjacentCell(), C, 2, "7d7".Roll(), null,
                            null, ParentObject.GetNearestVisibleObject(), null);
                        ElectromagneticPulse.EMP(C, 3, 1);
                    }
                }
            else if (ListCell4 != null && PhaseUnreal)
                foreach (var C in ListCell4)
                {
                    if (C.IsVisible() && Stat.Random(1, 100) < 10)
                    {
                        var fungibility = Stat.Random(1, 2);
                        var laughs = Stat.Random(1, 10);


                        C.TelekinesisBlip();
                        C.AddObject("FalseEncodedEater");

                        var O = C.GetCombatObject();

                        if (fungibility == 1)
                        {
                            O.Brain.Allegiance.Hostile = false;
                        }
                        else if (fungibility == 2)
                        {
                            O.Brain.Allegiance.Hostile = false;
                            O.Brain.Wanders = true;
                        }

                        if (laughs == 1)
                        {
                            AddPlayerMessage("{{red|" + O.DisplayName + " is laughing at you ...}}");
                        }
                    }
                }
            else if (FullZone != null && PhaseDerv)
                foreach (var C in FullZone)
                {
                    if (C.HasObjectWithBlueprint("FalseEncodedEater"))
                    {
                        var Objects = C.GetObjectsInCell();
                        foreach (var O in Objects)
                        {
                            if (O.Blueprint == "FalseEncodedEater")
                            {
                                O.TeleportSwirl();
                                O.Obliterate();
                            }
                        }
                    }
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

                if (!ParentObject.Brain.Mobile)
                    ParentObject.Brain.Mobile = true;

                if (!ParentObject.HasPart<Teleportation>())
                    ParentObject.GetPart<Mutations>().AddMutation(new Teleportation());
            }
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

        public void HarbingerProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.AddMutation(new Disintegration());
            bossMutations.AddMutation(new QuantumFugue());
            bossMutations.AddMutation(new TimeDilation());


            ParentObject.RemoveEffect<MemberOfPsychicBattle>();
            ParentObject.RemovePart<SunderMind>();

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 100);
        }

        public void EsperWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();
            var bossSkills = ParentObject.GetPart<Skills>();

            bossMutations.RemoveMutation(ParentObject.GetPart<AdrenalControl2>());
            bossMutations.RemoveMutation(ParentObject.GetPart<WaveformWorm>());

            bossMutations.AddMutation(new WillForce());
            bossMutations.AddMutation(new MassMind());
            bossMutations.AddMutation(new MentalMirror());
            bossMutations.AddMutation(new SunderMind());
            bossMutations.AddMutation(new RepellingForce());

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 100);

            if (!ParentObject.HasPart<Teleportation>())
            {
                bossMutations.AddMutation(new Teleportation());
            }
        }

        public void DervishWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();
            var bossSkills = ParentObject.GetPart<Skills>();

            bossMutations.RemoveMutation(ParentObject.GetPart<Confusion>());
            bossMutations.RemoveMutation(ParentObject.GetPart<LifeDrain>());

            bossMutations.AddMutation(new AdrenalControl2());
            bossMutations.AddMutation(new HeightenedSpeed());
            bossMutations.AddMutation(new HeightenedAgility());
            bossMutations.AddMutation(new TwoHearted());
            bossMutations.AddMutation(new WillForce());
            bossMutations.AddMutation(new WaveformWorm());

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

            bossSkills.AddSkill(new Tactics());
            bossSkills.AddSkill(new Tactics_Camouflage());
            bossSkills.AddSkill(new Tactics_Charge());
            bossSkills.AddSkill(new Tactics_DeathFromAbove());
            bossSkills.AddSkill(new Tactics_Juke());
            bossSkills.AddSkill(new Tactics_Hurdle());
            bossSkills.AddSkill(new Tactics_Throwing());

            bossSkills.AddSkill(new Multiweapon_Fighting());
            bossSkills.AddSkill(new Multiweapon_Expertise());
            bossSkills.AddSkill(new Multiweapon_Flurry());
            bossSkills.AddSkill(new Multiweapon_Mastery());

            bossSkills.AddSkill(new Endurance_ShakeItOff());
            bossSkills.AddSkill(new Endurance_Weathered());

            bossSkills.AddSkill((new Discipline_Conatus()));

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", 100);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 100);
        }

        public void PsyOpWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.RemoveMutation(ParentObject.GetPart<ForceWall>());
            bossMutations.RemoveMutation(ParentObject.GetPart<ForceBubble>());
            bossMutations.RemoveMutation(ParentObject.GetPart<LightManipulation>());
            bossMutations.RemoveMutation(ParentObject.GetPart<ElectromagneticPulse>());

            bossMutations.AddMutation(new Confusion());
            bossMutations.AddMutation(new FearAura() { EnergyCost = 0 });
            bossMutations.AddMutation(new LifeDrain());

            StatShifter.SetStatShift(ParentObject, "HeatResistance", -75);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", -75);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", -75);

            eqChannelRifle.UnequipAndRemove();
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

            StatShifter.SetStatShift(ParentObject, "HeatResistance", -50);
            StatShifter.SetStatShift(ParentObject, "ColdResistance", -50);
            StatShifter.SetStatShift(ParentObject, "ElectricResistance", 100);

            ParentObject.ForceEquipObject(eqChannelRifle, "Missile Weapon");

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

            eqFlameThrower.UnequipAndRemove();
        }

        public void ScorchWardenProtocol()
        {
            var bossMutations = ParentObject.GetPart<Mutations>();

            bossMutations.AddMutation(new Pyrokinesis());
            bossMutations.AddMutation(new FlamingRay());
            bossMutations.AddMutation(new HeatAbsorption());
            bossMutations.AddMutation(new EatersKindle());

            StatShifter.SetStatShift(ParentObject, "HeatResistance", 100);

            eqFlameThrower.LiquidVolume.Volume = 1;
            ParentObject.ForceEquipObject(eqFlameThrower, "Missile Weapon");

            PhaseFlame = true;
        }

        public void MehSaysSomethingRare()
        {
            AddPlayerMessage(ScrotumRare.PeekOne());
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("AICreateKill");
            eventRegistrar.Register("AfterTeleport");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AfterTeleport")
            {
                if (!HeightenedThreatResponse)
                {
                    if (Stat.Random(1, 3) == 1)
                        SuperDuperUltraAttack(Stat.Random(1, 7));
                }
                else
                    SuperDuperUltraAttack(Stat.Random(1, 7));
            }

            if (E.ID == "AICreateKill")
            {
                var target = E.GetGameObjectParameter("Target");
                if (target == ThePlayer && !InCombat)
                {
                    PlayWorldSound("BigBossAlarm.mp3");
                    ParentObject.DilationSplat();

                    ParentObject.SetDetailColor("A");
                    SoundManager.PlayMusic("Destiny");
                    AddPlayerMessage(
                        "You hear Mehrashir utter coldly, {{red|\"BIOMATA SELF-PRESERVATION SYSTEMS ENGAGED. INITIATING COMBAT-PROTOCOL \'SCORCH-WARDEN\' - HEAT-PURGE TACTICS EMINENT.\"}}");

                    InCombat = true;

                    PhaseFlame = true;
                    ScorchWardenProtocol();
                    InitiateTeleEater();

                    ringStage = 1;
                    var bossEruptRadius = ParentObject.CurrentCell.GetAdjacentCells(6);

                    ParentObject.Firesplatter();

                    foreach (var C in bossEruptRadius)
                    {
                        var targets = C.GetCombatTarget();

                        if (targets != ParentObject && targets != ThePlayer && targets.IsValid())
                        {
                            targets.Destroy();
                            C.Flameburst();
                        }
                    }
                }
            }

            return base.FireEvent(E);
        }
    }
}