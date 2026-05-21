using UnityEngine;
using XRL;

namespace Wingytone.Colors
{
    [HasModSensitiveStaticCache]
    public static class CustomColors
    {
        public const char DarkerCrimsonChar = '\u1075';
        public const string DarkerCrimsonString = "\u1075";
        
        public const char GoldChar = '\u1065';
        public const string GoldString = "\u1065";
        
        [ModSensitiveCacheInit]
        public static void Init()
        {
            var ColorMap = ConsoleLib.Console.ColorUtility.ColorMap;
            ColorMap['A'] = new Color(0.86f,0.07f,0.23f);
            ColorMap['a'] = new Color(0.56f,0.05f,0.15f);
            ColorMap[DarkerCrimsonChar] = new Color(0.37f,0.13f,0.18f);
            
            ColorMap[GoldChar] = new Color(0.81f,0.6f,0.08f);
            

        }
    }
}