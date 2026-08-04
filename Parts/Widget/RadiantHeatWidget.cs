using XRL.Rules;

namespace XRL.World.Parts
{
    public class RadiantHeatWidget : IPart
    {
        
        public bool Steam;
        
        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == EndTurnEvent.ID;
            }

            return true;
        }


        public override bool HandleEvent(EndTurnEvent E)
        {
            var Rando = Stat.Random(1, 100);
            
            if (Steam && !currentCell.HasObject("Steam") && Rando == 24)
            {
                currentCell.AddObject("Steam");
            }
            
        var OpenLiquid = currentCell.GetOpenLiquidVolume();

        if (OpenLiquid != null && OpenLiquid.Temperature <= 40)
        {
            currentCell.TemperatureChange(5);
        }

        return base.HandleEvent(E);
        }
    }
}