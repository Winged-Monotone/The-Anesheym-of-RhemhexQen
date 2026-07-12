using UnityEngine;
using XRL;
using XRL.World.Text.Attributes;

namespace Wingytone.Colors
{
    [HasModSensitiveStaticCache]
    public static class CustomColors
    {
        public const char CrimsonChar = 'A';
        
        public const char DarkerCrimsonChar = 'a';
        
        public const char DarkestCrimsonChar = 'l';
        
        public const char GoldChar = '\u1065';
        public const string GoldString = "\u1065";

        public const char SicklyYellowChar = '\u1055';
        
        public const char RoyalPurpleChar = 'p';

        public const char LightRoyalPurpleChar = 'P';
        
        public const char TextPinkChar = 'N';
        
        public const char TextDarkPinkChar = 'n';
        
        public const char PinkChar = 'I';
        
        public const char DarkPinkChar = 'i';
        
#if !BUILD_2_0_211_40
        public static void ExpandFastColorMap()
        {
            var map = ConsoleLib.Console.ColorUtility.ColorMap;
            var fastMap = ConsoleLib.Console.ColorUtility.FastColorMap;
            var max = fastMap.Length - 1;
            foreach (var (chr, _) in map)
            {
                if (chr > max)
                {
                    max = chr;
                }
            }
            if (max >= fastMap.Length)
            {
                fastMap = new Color[max + 1];
                foreach (var (chr, color) in map)
                {
                    fastMap[chr] = color;
                }

                ConsoleLib.Console.ColorUtility.FastColorMap = fastMap;
            }
        }
#endif
        
        [ModSensitiveCacheInit]
        public static void Init()
        {
            var ColorMap = ConsoleLib.Console.ColorUtility.ColorMap;
            
            ColorMap[CrimsonChar] = new Color(0.86f,0.20f,0.20f);

            ColorMap[DarkerCrimsonChar] = new Color(0.56f,0.20f,0.20f);
            
            ColorMap[DarkestCrimsonChar] = new Color(0.37f,0.20f,0.20f);
            
            ColorMap[GoldChar] = new Color(0.81f,0.6f,0.08f);
            
            ColorMap[SicklyYellowChar] = new Color(0.81f,0.89f,0.55f);
            
            ColorMap[RoyalPurpleChar] = new Color(0.29f,0.09f,0.60f);
            ColorMap[LightRoyalPurpleChar] = new Color(0.38f,0.19f,0.85f);

            ColorMap[PinkChar] = new Color(0.97f,0.78f,0.81f);
            ColorMap[DarkPinkChar] = new Color(0.75f,0.61f,0.63f);
            
            ColorMap[TextPinkChar] = new Color(1f,0.56f,0.64f);
            ColorMap[TextDarkPinkChar] = new Color(0.72f,0.59f,0.47f);
            
            ExpandFastColorMap();
        }
    }
}