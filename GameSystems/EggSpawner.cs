using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.Rules;
using XRL.World.Conversations.Parts;

namespace Wingytone.Systems
{
    public class EggSpawner : IGameSystem
    {
        public override void Register(XRLGame Game, IEventRegistrar Registrar)
        {
            Registrar.Register(AfterZoneBuiltEvent.ID);
            Registrar.Register(GenericQueryEvent.ID);
            Registrar.Register(ZoneActivatedEvent.ID);
            Registrar.Register(QuestStepFinishedEvent.ID);
            base.Register(Game, Registrar);
        }

        public override bool HandleEvent(QuestStepFinishedEvent E)
        {
            if (E.Step.ID == "LSDefeatNielst")
            {
                EndCrimsonTide(The.Player.CurrentZone);
            }
            
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GenericQueryEvent E)
        {
            if (E.Query == "ActivateSpiralBorer")
            {
                if (E.Subject.CurrentZone.wX == 33 && E.Subject.CurrentZone.wY == 3)
                {
                    E.Subject.Fail("Your spiral-borer refuses to activate.");
                    return false;
                }
            }

            return base.HandleEvent(E);
        }

        public static void EndCrimsonTide(Zone Z)
        {
            // wmNielstSlain

            if (!The.Game.HasIntGameState("wmNielstSlain"))
            {
                return;
            }

            foreach (var Obj in Z.IterateObjects())
            {
                if (Obj.LiquidVolume != null && Obj.LiquidVolume.ComponentLiquids.Remove("putrifiedvitae", out var value))
                {
                    Obj.LiquidVolume.ComponentLiquids["calcifiedvitae"] = value;
                    Obj.LiquidVolume.Update();
                }
            }
        }

        public override bool HandleEvent(ZoneActivatedEvent E)
        {
            if (E.Zone.DisplayName.Contains("Long Sigh"))
            {
                if (The.Game.HasQuest("ShahStartLongSigh") &&
                    !The.Game.HasFinishedQuestStep("ShahStartLongSigh", "LSEnterTheLongSigh"))
                {
                    The.Game.FinishQuestStep("ShahStartLongSigh", "LSEnterTheLongSigh");
                }
            }

            EndCrimsonTide(E.Zone);
            
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AfterZoneBuiltEvent E)
        {
            if (!E.Zone.IsWorldMap() && E.Zone.ZoneWorld == "JoppaWorld")
            {
                PlaceEggs(E.Zone);
            }

            return base.HandleEvent(E);
        }

        public void PlaceEggs(Zone zone)
        {
            if (25.in100())
            {
                return;
            }

            var PopEggs = PopulationManager.RollOneFrom("DynamicInheritsTable:BaseStrangeEgg:Tier" + zone.NewTier);
            var PopEggsBluePrint = PopEggs.Blueprint;
            int placeStyle = Stat.Random(1, 3);

            if (placeStyle == 1) // in chest
            {
                foreach (GameObject GO in zone.GetObjectsWithPart("Container").Shuffle())
                {
                    if (GO.HasPart<Inventory>())
                    {
                        GO.Inventory.AddObject(PopEggsBluePrint);

                        GO.SetImportant(true, true);
                        // MetricsManager.LogInfo("Placed in Chest.");
                        // MetricsManager.LogInfo("At Cell Location:" + GO.GetCurrentCell());
                        return;
                    }
                }
            }

            if (placeStyle == 1 || placeStyle == 2) // in creature
            {
                foreach (GameObject GO in zone.GetObjectsWithPart("Brain").Shuffle())
                {
                    if (GO.HasPart<Inventory>() && GO.HasPart<Combat>() && !GO.IsTemporary)
                    {
                        GO.Inventory.AddObject(PopEggsBluePrint);

                        GO.SetImportant(true, true);

                        // MetricsManager.LogInfo("Placed on Mob.");
                        // MetricsManager.LogInfo("At Cell Location:" + GO.GetCurrentCell());
                        return;
                    }
                }
            }

            // on flor or if other placement failed
            foreach (Cell C in zone.GetEmptyReachableCells().Shuffle())
            {
                C.AddObject(PopEggsBluePrint);

                return;
            }
        }
    }
}