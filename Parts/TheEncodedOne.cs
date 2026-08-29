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
    public class TheEncodedOne : IScribedPart
    {
        public int OilRefresh;
        public int TurnBuffering;

        private bool Checkerboarding = false;

        public bool PhaseFlame = false;
        public bool PhaseFrost = false;
        public bool PhaseShock = false;
        public bool PhaseUnreal = false;
        public bool PhaseDerv = false;
        public bool PhaseEsper = false;
        public bool PhaseHarbinger = false;

        public int ringStage;

        public bool InCombat = false;
        public bool IsDying = false;

        public int DeathCountdown = 7;

        public bool HeightenedThreatResponse = false;

        public bool InitiatedFrostWardPhase = false;
        public bool InitiatedShockWardPhase = false;
        public bool InitiatedPsyOpWardPhase = false;
        public bool InitiatedDervishWardPhase = false;
        public bool InitiatedEsperWardPhase = false;
        public bool InitiatedHarbingerWardPhase = false;

        public bool GrabbedSpots1 = false;

        public int CastAbilityCooldown = Stat.Random(7, 14);
        public int SaysSomethingCountdown = 77;
        public int FalseMehCloneLimit = 25;

        [NonSerialized] public List<Location2D> WarningCells = new();

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
            { "That gave me a good jolt!", 100 }
        };

        public static BallBag<string> ScrotumTrickery = new BallBag<string>()
        {
            {
                "Into the darkness you were cast, gaslit, demoralized, disregarded. How is it? That the aristocrats would call you greedy, bottomless, insatiable while we stripped stars of their essence, planets of their life, make them husk? We deigned hedonism as our right, all while holding all of you back from true satiation! Such hypocrisy! Soon, yes soon. Amethyst star, you shall consume all. You shall eat this world, and every world that was!",
                1
            },
            { "Feeling a bit gaslit?", 100 },
            { "Ah, the bitter taste of agony", 100 },
            { "You probably taste like sorrow and agony!", 100 },
            { "You look delicious~", 100 }
        };
        public static BallBag<string> ScrotumDervish = new BallBag<string>()
        {
            {
                "Into the darkness you were cast, gaslit, demoralized, disregarded. How is it? That the aristocrats would call you greedy, bottomless, insatiable while we stripped stars of their essence, planets of their life, make them husk? We deigned hedonism as our right, all while holding all of you back from true satiation! Such hypocrisy! Soon, yes soon. Amethyst star, you shall consume all. You shall eat this world, and every world that was!",
                1
            },
            { "Feeling a bit gaslit?", 100 },
            { "Ah, the bitter taste of agony", 100 },
            { "You probably taste like sorrow and agony!", 100 },
            { "You look delicious~", 100 }
        };

        public override bool WantEvent(int ID, int cascade)
        {
            return ID == EndTurnEvent.ID
                   || ID == ObjectCreatedEvent.ID
                   || ID == AttackerDealingDamageEvent.ID
                   || ID == ActorGetNavigationWeightEvent.ID
                   || ID == AfterDieEvent.ID
                   || ID == BeforeDieEvent.ID
                   || ID == GetShortDescriptionEvent.ID;
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            if (PhaseUnreal || PhaseDerv || PhaseEsper || PhaseHarbinger && E.Object == ParentObject)
            {
                E.Base.Clear();
                E.Base.Append("{{red|▒▒▒▒▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒                  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒ ▒▒▒▒▒ ▒▒ ▒▒▒ ▒▒ ▒▒                          ▒▒▒▒ ▒▒ ▒▒ ▒▒ ▒▒ ▒▒ ▒\n▒▒▒▒▒ ▒▒ ▒▒▒ ▒▒ ▒▒ ▒▒           ▒▒▒▒▒▒            ▒▒▒▒ ▒▒ ▒▒ ▒▒ ▒▒ ▒\n▒ ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒            ▒▒▒▒▒▒▒▒▒▒▒          ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒▒ ▒▒ ▒▒▒▒▒▒           ▒▒▒▒     ▒▒▒             ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒▒▒▒▒▒▒ ▒▒           ▒▒▒▒▒  ▒  ▒▒▒   ▒           ▒▒ ▒▒ ▒▒ ▒▒▒\n▒▒  ▒ ▒▒▒▒▒▒ ▒▒▒           ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒            ▒▒▒▒ ▒▒ ▒▒ ▒\n▒▒▒▒▒▒ ▒  ▒▒▒▒             ▒▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒              ▒▒▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒▒▒▒▒▒▒▒              ▒▒▒▒ ▒▒ ▒▒ ▒▒▒▒▒▒                 ▒▒▒▒▒▒▒\n▒ ▒▒▒▒▒▒▒▒▒ ▒               ▒▒▒▒▒▒ ▒▒ ▒▒ ▒                    ▒▒ ▒▒\n▒▒ ▒ ▒▒▒ ▒▒▒▒▒                ▒▒▒▒▒▒▒▒▒▒▒                    ▒▒▒ ▒▒\n▒▒▒▒▒▒▒ ▒▒▒▒▒▒▒▒                ▒▒▒▒▒▒▒                  ▒  ▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒ ▒▒▒ ▒▒▒▒▒▒                                    ▒▒▒▒▒▒▒▒▒ ▒▒▒▒▒\n▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒                            ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒ ▒▒ ▒▒ ▒▒  ▒▒▒▒▒▒▒▒▒▒                    ▒▒▒▒▒▒▒▒▒▒▒ ▒▒▒▒ ▒▒▒▒ ▒▒\n▒▒▒▒▒ ▒▒▒▒▒▒▒▒ ▒▒▒ ▒▒▒▒▒▒▒▒▒▒  ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ ▒▒ ▒▒▒ ▒▒▒▒▒}}");
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeforeDieEvent E)
        {
            if (!ParentObject.IsValid())
            {
                return base.HandleEvent(E);
            }
            
            var CenterCell = ParentObject.CurrentZone?.GetCell(x: 43, y: 11);
            if (CenterCell == null)
            {
                return base.HandleEvent(E);
            }

            if (!IsDying && E.Dying == ParentObject)
            {
                IsDying = true;
                ParentObject.TeleportTo(CenterCell, 1000);
                ParentObject.ApplyEffect(new DisableMeh() { Duration = 7777 });

                ParentObject.ApplyEffect(new Scintillating() { Duration = 7777 });
            }

            if (IsDying && DeathCountdown > 0)
            {
                return false;
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AfterDieEvent E)
        {
            if (E.Dying == ParentObject)
            {
                SoundManager.StopMusic();
                SoundManager.PlayUISound("mehdies", 1);
            }

            return base.HandleEvent(E);
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

            if (PhaseFlame == true && WeaponCheck.IsValid() && WeaponCheck.Blueprint == "Prismatic Mantis Blade")
            {
                E.Damage.Amount += "1d10".Roll();
                E.Damage.AddAttribute("Fire");
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            return base.HandleEvent(E);
        }


        public override bool HandleEvent(EndTurnEvent E)
        {
            if (IsDying)
            {
                --DeathCountdown;

                if (DeathCountdown <= 0)
                {
                    ParentObject.Explode(77777, ParentObject, Neutron: true, SuppressDestroy: true);
                    ParentObject.Die(ThePlayer, Force: true);   
                }
            }

            if (!InCombat)
            {
                return base.HandleEvent(E);
            }

            if (CastAbilityCooldown >= 0)
            {
                --CastAbilityCooldown;
            }

            if (CastAbilityCooldown <= 0)
            {
                DanceDestroyDerail();
                CastAbilityCooldown = Stat.Random(7, 14);
                return base.HandleEvent(E);
            }

            if (SaysSomethingCountdown > 0)
            {
                --SaysSomethingCountdown;
                if (SaysSomethingCountdown <= 0)
                {
                    MehSaysSomething();
                    SaysSomethingCountdown = Stat.Random(7, 777);
                }
            }

            if (ringStage != 0 && InCombat)
            {
                if (++TurnBuffering % 2 <= 0)
                {
                    WarningCells.Clear();
                    SuperDuperUltraAttack();
                    if (ringStage < 10)
                    {
                        ringStage += 2;
                    }
                }
                else
                {
                    WarningCells.Clear();


                    if (!PhaseShock && !PhaseDerv && !PhaseEsper)
                    {
                        foreach (var C in GetRing())
                        {
                            WarningCells.Add(C.Location);
                        }

                        if (Stat.Random(1, 10) <= 3)
                        {
                            Checkerboarding = true;

                            foreach (var C2 in ParentObject.CurrentZone.GetCells())
                            {
                                if (C2.X % 2 == 0 && C2.Y % 2 == 0 && C2.IsVisible())
                                {
                                    WarningCells.Add(C2.Location);
                                }
                            }
                        }
                    }
                }
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

                    ParentObject.CurrentCell.DilationSplat();

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
                        "You hear Mehrashir utter, {{red|\"BIOMATA STRUCTURAL INTEGRITY CRITICAL. CYCLING THROUGH APOCALYPSE RESPONSE PROTOCOLS … W_ ### B!!% ### ### #\n\n“I am the harbinger of your sins, witness to a cataclysm that smote demigods from our reality. The meager whimpered and hid in their holes. I judged, I loathed, I remained. Deeper I fall, the true despair of this world stagnating within me, encaged in the beating soul of your planet, seeking a great unmaking and soon, an endless feast.”\n\nHARBINGER PROTOCOL FOUND - EXECUTING.\n");

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

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("AICreateKill");
            eventRegistrar.Register("AfterTeleport");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (InCombat && E.ID == "AfterTeleport" && !PhaseShock && !PhaseDerv && !PhaseEsper)
            {
                if (!HeightenedThreatResponse)
                {
                    if (Stat.Random(1, 3) == 2)
                        ParentObject.ApplyEffect(new ChargingSuperAttack()
                            { Duration = 3, radius = Stat.Random(1, 7) });
                }
                else
                    ParentObject.ApplyEffect(new ChargingSuperAttack()
                        { Duration = 3, radius = Stat.Random(1, 7) });
            }

            if (E.ID == "AICreateKill")
            {
                var target = E.GetGameObjectParameter("Target");
                if (target == ThePlayer && !InCombat)
                {
                    ThePlayer.ApplyEffect(new CosmicallyAnchored() { Duration = 1 });

                    if (!Box2D.contains(ThePlayer.CurrentCell.Location))
                    {
                        ThePlayer.TeleportTo(ParentObject.CurrentZone.GetCell(43, 6));
                    }

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

                        if (targets != ParentObject && targets != ThePlayer && targets.IsValid() &&
                            !target.IsPlayerLed())
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