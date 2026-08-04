using System;
using System.Collections.Generic;
using System.Linq;
using Genkit;
using XRL.Rules;
using XRL.UI;
using XRL.World.Parts;
using UnityEngine;
using System.Diagnostics;
using XRL.Core;
using ConsoleLib.Console;
using HistoryKit;
using Qud.API;
using System.Text;
using XRL;
using XRL.Annals;
using XRL.Language;
using XRL.World;
using XRL.World.Capabilities;
using XRL.World.Effects;
using XRL.World.Parts.Skill;
using XRL.World.Skills.Cooking;
using XRL.World.ZoneBuilders;
using XRL.World.ZoneBuilders.Utility;


namespace XRL.World.ZoneBuilders
{
    public class VillagesOfCrystholts : ZoneBuilderSandbox
    {
        public class PopulationLayout
        {
            public bool hasStructure;

            public Zone zone;

            public InfluenceMapRegion originalRegion;

            public Rect2D innerRect;

            public InfluenceMap map;

            public int seed;

            public List<Location2D> inside = new List<Location2D>();

            public List<Location2D> insideWall = new List<Location2D>();

            public List<Location2D> insideCorner = new List<Location2D>();

            public List<Location2D> outside = new List<Location2D>();

            public List<Location2D> outsideWall = new List<Location2D>();

            public List<Location2D> outsideCorner = new List<Location2D>();

            public Location2D lastPosition;

            public InfluenceMapRegion region
            {
                get
                {
                    if (map.Regions.Count <= seed)
                    {
                        return map.Regions[0];
                    }

                    return map.Regions[seed];
                }
            }

            public Location2D position
            {
                get
                {
                    if (lastPosition != null)
                    {
                        return lastPosition;
                    }

                    if (map.Seeds.Count <= seed)
                    {
                        UnityEngine.Debug.LogError("Couldn't get a seed:" + seed);
                        return null;
                    }

                    return map.Seeds[seed];
                }
                set
                {
                    lastPosition = value;
                    map.Seeds[seed] = value;
                }
            }

            public PopulationLayout(Zone zone, InfluenceMapRegion region, Rect2D innerRect)
            {
                this.zone = zone;
                map = region.map;
                seed = region.Seed;
                this.innerRect = innerRect;
                position = this.innerRect.Center.location;
                originalRegion = region;
            }
        }

        public Grid<Color4> grid;
        public InfluenceMap regions;
        public string region;
        public List<Rect2D> maskingAreas;
        private List<PopulationLayout> buildings = new List<PopulationLayout>();
        public const string wallType = "Rhem Black Marble";
        public const string floorType = "BlackMarbleWalkway";
        public string villagerBaseFaction = "Anesheym";

        public string ResolvePopulationTableName(string tablePrefix)
        {
            if (PopulationManager.HasTable(tablePrefix + "_Faction_" + villagerBaseFaction))
            {
                return tablePrefix + "_Faction_" + villagerBaseFaction;
            }

            if (PopulationManager.HasTable(tablePrefix + "_" + region))
            {
                return tablePrefix + "_" + region;
            }

            return tablePrefix + "_*Default";
        }

        public bool BuildZone(Zone zone)
        {
            string seed = XRLCore.Core.Game.GetWorldSeed().ToString();
            string text = PopulationManager.RollOneFrom(ResolvePopulationTableName("BugVillages_BuildingStyle"))
                .Blueprint;
            InfluenceMapRegion RegionValue = new InfluenceMapRegion(GetSeededRange(seed), regions);
            Rect2D Rect = GridTools.MaxRectByArea(RegionValue.GetGrid()).Translate(RegionValue.BoundingBox.UpperLeft)
                .ReduceBy(1, 1);
            PopulationLayout populationLayout = new PopulationLayout(zone, RegionValue, Rect);
            getWfcBuildingTemplate("hivehut3").ShuffleInPlace();
            if (text.StartsWith("wfc,") && !getWfcBuildingTemplate(text.Split(',')[1]).Any((ColorOutputMap t) =>
                    t.extrawidth <= Rect.Width && t.extraheight <= Rect.Height))
            {
                text = "bughivehuts";
            }

            buildings.Add(populationLayout);
            if (text.StartsWith("bughivehuts"))
            {
                foreach (ColorOutputMap item in getWfcBuildingTemplate("hivehut3"))
                {
                    int num5 = item.width / 2;
                    int num6 = item.height / 2;
                    if (item.extrawidth > populationLayout.innerRect.Width ||
                        item.extraheight > populationLayout.innerRect.Height)
                    {
                        continue;
                    }

                    for (int m = 0; m < item.width; m++)
                    {
                        for (int n = 0; n < item.height; n++)
                        {
                            Cell cell = zone.GetCell(populationLayout.position.X - num5 + m,
                                populationLayout.position.Y - num6 + n);
                            if (cell != null)
                            {
                                if (ColorExtensionMethods.Equals(item.getPixel(m, n), ColorOutputMap.BLACK))
                                {
                                    cell.AddObject(wallType);
                                }
                                else if (ColorExtensionMethods.Equals(item.getPixel(m, n), ColorOutputMap.RED))
                                {
                                    populationLayout.position = Location2D.Get(m, n);
                                }
                            }
                        }
                    }

                    populationLayout.hasStructure = true;
                    break;
                }
            }

            // grid = new Grid<Color4>(200, 40);
            // grid.fromWFCTemplate("hivehut3", 4, int.MaxValue);
            // grid.mirrorHorizontal();
            // for (int x = 0; x < 80; x++)
            // {
            //     for (int y = 0; y < 25; y++)
            //     {
            //         if (grid.get(x, y) == Color4.black)
            //         {
            //             zone.GetCell(x, y).ClearWalls();
            //             zone.GetCell(x, y).AddObject(wallType, null, null, null, null);
            //             Debug.Log("x: " + x + " " + " " + "y: " + y + " [Part 1]");
            //         }
            //     }
            // }
            // zone.GetCell(1, 1).AddObject("Crystallands", null, null, null, null);
            // // InfluenceMap influenceMap = ZoneBuilderSandbox.GenerateInfluenceMap(zone, new List<Point>(), InfluenceMapSeedStrategy.RandomPointFurtherThan4, 100, null, Options.GetOption("OptionDrawInfluenceMaps", "No") == "Yes", null);
            // // if (ZoneTemplateManager.HasTemplates("Rhem Black Marble"))
            // // {
            // //     ZoneTemplateManager.Templates["Rhem Black Marble"].Execute(zone, influenceMap);
            // // }
            return true;
        }
    }
}