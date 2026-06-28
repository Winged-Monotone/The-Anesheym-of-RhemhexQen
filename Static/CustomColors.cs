using UnityEngine;
using XRL;
using XRL.World.Text.Attributes;

namespace Wingytone.Colors
{
    [HasModSensitiveStaticCache]
    public static class CustomColors
    {
        public const char DarkerCrimsonChar = '\u1075';
        public const string DarkerCrimsonString = "\u1075";
        
        public const char GoldChar = '\u1065';
        public const string GoldString = "\u1065";

        public const char SicklyYellowChar = '\u1055';
        public const string SicklyYellowString = "\u1055";
        
        public const char RoyalPurpleChar = '\u1056';
        public const string RoyalPurpleString = "\u1056";

        public const char LightRoyalPurpleChar = '\u1057';
        public const string LightRoyalPurpleString = "\u1057";
        
        public const char TextPinkChar = 'N';
        public const string TextPinkString = "\u1058";
        
        public const char TextDarkPinkChar = 'n';
        public const string TextDarkPinkString = "\u1059";
        
        public const char PinkChar = 'I';
        public const string PinkString = "\u1060";
        
        public const char DarkPinkChar = 'i';
        public const string DarkPinkString = "\u1061";
        
// #if !BUILD_2_0_211_40
//         public static void ExpandFastColorMap()
//         {
//             var map = ConsoleLib.Console.ColorUtility.ColorMap;
//             var fastMap = ConsoleLib.Console.ColorUtility.FastColorMap;
//             var max = fastMap.Length - 1;
//             foreach (var (chr, _) in map)
//             {
//                 if (chr > max)
//                 {
//                     max = chr;
//                 }
//             }
//             if (max >= fastMap.Length)
//             {
//                 fastMap = new Color[max + 1];
//                 foreach (var (chr, color) in map)
//                 {
//                     fastMap[chr] = color;
//                 }
//
//                 ConsoleLib.Console.ColorUtility.FastColorMap = fastMap;
//             }
//         }
// #endif
        
        [ModSensitiveCacheInit]
        public static void Init()
        {
            var ColorMap = ConsoleLib.Console.ColorUtility.ColorMap;
            ColorMap['A'] = new Color(0.86f,0.20f,0.20f);
            ColorMap['a'] = new Color(0.56f,0.20f,0.20f);
            ColorMap[DarkerCrimsonChar] = new Color(0.37f,0.20f,0.20f);
            
            ColorMap[GoldChar] = new Color(0.81f,0.6f,0.08f);
            
            ColorMap[SicklyYellowChar] = new Color(0.81f,0.89f,0.55f);
            
            ColorMap[RoyalPurpleChar] = new Color(0.29f,0.09f,0.60f);
            ColorMap[LightRoyalPurpleChar] = new Color(0.38f,0.19f,0.85f);

            ColorMap[PinkChar] = new Color(0.97f,0.78f,0.81f);
            ColorMap[DarkPinkChar] = new Color(0.75f,0.61f,0.63f);
            
            ColorMap[TextPinkChar] = new Color(1f,0.56f,0.64f);
            ColorMap[TextDarkPinkChar] = new Color(0.72f,0.59f,0.47f);
            
            // ExpandFastColorMap();
        }
    }
}