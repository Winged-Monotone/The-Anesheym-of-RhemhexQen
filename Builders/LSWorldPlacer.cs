using XRL;
using XRL.World;
using XRL.World.WorldBuilders;
using XRL.World.ZoneBuilders;

namespace Wingytone.WorldBuiler
{
    //The game code instantiates an instance of this class during the JoppaWorld generation process
    [JoppaWorldBuilderExtension]
    public class YourJoppaWorldBuilderExtension : IJoppaWorldBuilderExtension
    {
        public override void OnBeforeBuild(JoppaWorldBuilder builder)
        {

        }

        public override void OnAfterBuild(JoppaWorldBuilder builder)
        {
            var ZM = The.ZoneManager;
            
            // 1St Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845651", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh1"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845651", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845651", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845651", "G-007-GMF-1st Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845651", "ZoneTierOverride", "5");
            
            // 2nd Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845652", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh2"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845652", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845652", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845652", "G-007-GMF-2nd Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845652", "ZoneTierOverride", "5");

            // 3rd Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845653", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh3"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845653", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845653", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845653", "G-007-GMF-3rd Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845653", "ZoneTierOverride", "5");

            // 4th Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845654", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh4"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845654", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845654", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845654", "G-007-GMF-4th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845654", "ZoneTierOverride", "5");

            // 5th-A Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845655", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh5a"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845655", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845655", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845655", "G-007-GMF-5th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845655", "ZoneTierOverride", "5");

            // 5th Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845656", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh5"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845656", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845656", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845656", "G-007-GMF-5th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845656", "ZoneTierOverride", "5");

            // 6th floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845657", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh6"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845657", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845657", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845657", "G-007-GMF-6th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845657", "ZoneTierOverride", "5");

            // 7th floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845658", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "LongSigh7"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845658", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "Reprisal"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845658", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845658", "G-007-GMF-7th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845658", "ZoneTierOverride", "5");

        }
    }
}