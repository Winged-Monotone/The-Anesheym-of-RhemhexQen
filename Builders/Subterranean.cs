using XRL.Rules;
using XRL.World;
using XRL.World.ZoneBuilders.Utility;

namespace XRL.World.ZoneBuilders
{
    public class Subterranean : ZoneBuilderSandbox
    {
        public bool BuildZone(Zone Z)
        {
            NoiseMap noiseMap = new NoiseMap(Z.Width, Z.Height, 10, 1, 1, Stat.Random(30, 60), Stat.Random(200, 350), Stat.Random(500, 800), 0, 10, 0, 1, null, 5);
            
            
            for (int X = 0; X < 80; X++)
            for (int Y = 0; Y < 25; Y++)
            {
                var Cell = Z.GetCell(X, Y);
                if (noiseMap.Noise[X, Y] < 0.001)
                    continue;
                if (!Cell.IsSolid(ForFluid: true))
                {
                    Cell.AddObject("OceanicWater");
                }
            }
                
            
            return true;
        }
    }
}