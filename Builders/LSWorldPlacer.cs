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
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845651", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-one.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845651", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845651", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845651", "L-386-GMF-1st Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845651", "ZoneTierOverride", "5");
            
            // 2nd Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845652", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-two.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845652", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845652", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845652", "L-386-GMF-2nd Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845652", "ZoneTierOverride", "5");

            // 3rd Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845653", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-three.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845653", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845653", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845653", "L-386-GMF-3rd Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845653", "ZoneTierOverride", "5");

            // 4th Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845654", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-four.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845654", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845654", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845654", "L-386-GMF-4th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845654", "ZoneTierOverride", "5");

            // 5th-A Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845655", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-Five-A.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845655", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845655", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845655", "L-386-GMF-5th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845655", "ZoneTierOverride", "5");

            // 5th Floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845656", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-five.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845656", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845656", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845656", "L-386-GMF-5th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845656", "ZoneTierOverride", "5");

            // 6th floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845657", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-six.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845657", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "thelongsigh"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845657", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845657", "L-386-GMF-6th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845657", "ZoneTierOverride", "5");

            // 7th floor
            
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845658", ZoneBuilderBlueprint.Get(nameof(MapBuilder), "FileName", "L-386-GMF-Floor-seven.rpm"));
            ZM.AddZoneBuilderOverride("JoppaWorld.33.3.1.1.29845658", ZoneBuilderBlueprint.Get(nameof(Music), "Track", "Reprisal"));
            ZM.SetZoneDisplayName("JoppaWorld.33.3.1.1.29845658", "The Long Sigh");
            ZM.SetZoneNameContext("JoppaWorld.33.3.1.1.29845658", "L-386-GMF-7th Floor");
            ZM.SetZoneProperty("JoppaWorld.33.3.1.1.29845658", "ZoneTierOverride", "5");

        }
    }
}