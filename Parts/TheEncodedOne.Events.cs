using XRL.Rules;
using XRL.World.Effects;
using XRL.World.Parts.Mutation;

namespace XRL.World.Parts
{
    public partial class TheEncodedOne
    {
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
                E.Base.Append(
                    "{{red|▒▒▒▒▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒                  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒ ▒▒▒▒▒ ▒▒ ▒▒▒ ▒▒ ▒▒                          ▒▒▒▒ ▒▒ ▒▒ ▒▒ ▒▒ ▒▒ ▒\n▒▒▒▒▒ ▒▒ ▒▒▒ ▒▒ ▒▒ ▒▒           ▒▒▒▒▒▒            ▒▒▒▒ ▒▒ ▒▒ ▒▒ ▒▒ ▒\n▒ ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒            ▒▒▒▒▒▒▒▒▒▒▒          ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒▒ ▒▒ ▒▒▒▒▒▒           ▒▒▒▒     ▒▒▒             ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒▒▒▒▒▒▒ ▒▒           ▒▒▒▒▒  ▒  ▒▒▒   ▒           ▒▒ ▒▒ ▒▒ ▒▒▒\n▒▒  ▒ ▒▒▒▒▒▒ ▒▒▒           ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒            ▒▒▒▒ ▒▒ ▒▒ ▒\n▒▒▒▒▒▒ ▒  ▒▒▒▒             ▒▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒              ▒▒▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒▒▒▒▒▒▒▒              ▒▒▒▒ ▒▒ ▒▒ ▒▒▒▒▒▒                 ▒▒▒▒▒▒▒\n▒ ▒▒▒▒▒▒▒▒▒ ▒               ▒▒▒▒▒▒ ▒▒ ▒▒ ▒                    ▒▒ ▒▒\n▒▒ ▒ ▒▒▒ ▒▒▒▒▒                ▒▒▒▒▒▒▒▒▒▒▒                    ▒▒▒ ▒▒\n▒▒▒▒▒▒▒ ▒▒▒▒▒▒▒▒                ▒▒▒▒▒▒▒                  ▒  ▒▒▒▒▒▒▒▒\n▒▒▒▒▒▒ ▒▒▒ ▒▒▒▒▒▒                                    ▒▒▒▒▒▒▒▒▒ ▒▒▒▒▒\n▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒                            ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n▒▒ ▒▒ ▒▒ ▒▒  ▒▒▒▒▒▒▒▒▒▒                    ▒▒▒▒▒▒▒▒▒▒▒ ▒▒▒▒ ▒▒▒▒ ▒▒\n▒▒▒▒▒ ▒▒▒▒▒▒▒▒ ▒▒▒ ▒▒▒▒▒▒▒▒▒▒  ▒▒ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ ▒▒ ▒▒▒ ▒▒▒▒▒}}");
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
                SoundManager.PlayUISound("mehdies", 0.30f);
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
                    SoundManager.PlayMusic("Destiny", VolumeAttenuation: 0.50f);
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
                            !target.IsPlayerLed() && targets.PartyLeader != ThePlayer && !targets.HasTag("Essential"))
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