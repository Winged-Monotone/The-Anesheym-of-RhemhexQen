namespace XRL.World.ZoneBuilders
{
    public class BeviddCrystalForest : ZoneBuilderSandbox
    {
        public bool BuildZone(Zone Z)
        {
            ZoneTemplateManager.Templates["BeviddCrystalForestTemplate"].Execute(Z);
            return true;
        }
    }
}