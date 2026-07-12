using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.Rules;

namespace Wingytone.Systems
{
    public class EggSpawner : IGameSystem
    {
        public override void Register(XRLGame Game, IEventRegistrar Registrar)
        {
            Registrar.Register(AfterZoneBuiltEvent.ID);
            base.Register(Game, Registrar);
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
                   
               return ;
           }
        }
    }
}