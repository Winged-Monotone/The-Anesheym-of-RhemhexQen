using XRL.Rules;

namespace XRL.World.Parts
{
    public class RadiantAllMapHeatWidget : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == AfterZoneBuiltEvent.ID;
            }

            return true;
        }


        public override bool HandleEvent(AfterZoneBuiltEvent E)
        {
            var eZone = E.Zone.GetCells();


            foreach (Cell C in eZone)
            {
                if (!C.HasObject("NoSteamRadiantHeat"))
                    C.AddObject("NoSteamRadiantHeat");
            }

            return base.HandleEvent(E);
        }
    }
}