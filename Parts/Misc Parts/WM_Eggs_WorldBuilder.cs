using System;
using System.Linq;
using HistoryKit;
using XRL.Names;
using XRL.World.Parts;
using XRL.World.ZoneBuilders;
using Genkit;

namespace XRL.World.WorldBuilders
{
    [JoppaWorldBuilderExtension]
    public class WM_Eggs_WorldBuilder : IJoppaWorldBuilderExtension
    {
        public string egg;
        public string Blueprint;
        public bool AddEggs = true;

        public override void OnAfterBuild(JoppaWorldBuilder Builder)
        {
            for (int i = 0; i < 80000; i++)
            {
                var randomZoneId = ZoneID.Assemble("JoppaWorld",
                    new Location2D(Rules.Stat.Random(0, 239), Rules.Stat.Random(0, 74)));

                The.ZoneManager.AddZonePostBuilder(randomZoneId, "wmPlaceEggsBuilder");
                // MetricsManager.LogInfo($"Spawning eggs in {randomZoneId}");
            }
        }
    }
}