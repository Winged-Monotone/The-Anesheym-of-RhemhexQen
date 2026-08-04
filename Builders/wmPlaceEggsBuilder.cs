// using System;
// using HistoryKit;
// using XRL.World.Parts;
//
// namespace XRL.World.ZoneBuilders
// {
//     public class wmPlaceEggsBuilder
//     {
//         public string egg;
//         public string Blueprint;
//         public int rarity;

//         public void BuildEggs(Zone Z)
//         {
//             switch (Z.NewTier)
//             {
//                 case 0:
//                     rarity = 1;
//                     egg = "strangeeggt1";
//                     break;
//                 case 1:
//                     rarity = 1;
//                     egg = "strangeeggt1";
//                     break;
//                 case 2:
//                     rarity = 2;
//                     egg = "strangeeggt2";
//                     break;
//                 case 3:
//                     rarity = 3;
//                     egg = "strangeeggt3";
//                     break;
//                 case 4:
//                     rarity = 4;
//                     egg = "strangeeggt4";
//                     break;
//                 case 5:
//                     rarity = 5;
//                     egg = "strangeeggt5";
//                     break;
//                 case 6:
//                     rarity = 6;
//                     egg = "strangeeggt6";
//                     break;
//                 case 7:
//                     rarity = 500;
//                     egg = "strangeeggt7";
//                     break;
//                 case 8:
//                     rarity = 10000;
//                     egg = "strangeeggt8";
//                     break;
//             }
//         }
//
//
//         public bool BuildZone(Zone Z)
//         {
//             // MetricsManager.LogInfo("Builder is Egging on the Eggs.");
//
//             BuildEggs(Z);
//
//             // MetricsManager.LogInfo("Grabbing Egg-Tier Complete.");
//
//             int placeStyle = Rules.Stat.Random(1, 2);
//
//             if (placeStyle == 1) // in chest
//             {
//                 foreach (GameObject GO in Z.GetObjectsWithPart("Container").Shuffle())
//                 {
//                     if (GO.HasPart<Inventory>())
//                     {
//                         BuildEggs(Z);
//
//                         GO.Inventory.AddObject(egg);
//
//                         GO.SetImportant(true, true);
//                         // MetricsManager.LogInfo("Placed in Chest.");
//                         // MetricsManager.LogInfo("At Cell Location:" + GO.GetCurrentCell());
//                         return true;
//                     }
//                 }
//             }
//
//             if (placeStyle == 1 || placeStyle == 2) // in creature
//             {
//                 foreach (GameObject GO in Z.GetObjectsWithPart("Brain").Shuffle())
//                 {
//                     if (GO.HasPart<Inventory>() && GO.HasPart<Combat>() && !GO.IsTemporary)
//                     {
//                         BuildEggs(Z);
//
//                         GO.Inventory.AddObject(egg);
//
//                         GO.SetImportant(true, true);
//
//                         // MetricsManager.LogInfo("Placed on Mob.");
//                         // MetricsManager.LogInfo("At Cell Location:" + GO.GetCurrentCell());
//                         return true;
//                     }
//                 }
//             }
//
//             // on flor or if other placement failed
//             foreach (Cell C in Z.GetEmptyReachableCells().Shuffle())
//             {
//                 BuildEggs(Z);
//
//                 if (Rules.Stat.Random(0, rarity) <= 0)
//                     C.AddObject(egg);
//
//                 // MetricsManager.LogInfo("Placed on Ground.");
//                 // MetricsManager.LogInfo("At Cell Location:" + C.ToString());
//                 return true;
//             }
//
//             return true;
//         }
//     }
// }