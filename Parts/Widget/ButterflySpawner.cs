using XRL.Rules;

namespace XRL.World.Parts
{
    public class ButterflySpawner : IPart
    {
        public int ButterflyMaximum = Stat.Random(1,3);

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == BeforeZoneBuiltEvent.ID;
            }

            return true;
        }


        public override bool HandleEvent(BeforeZoneBuiltEvent E)
        {
            
            var eZone = E.Zone.GetCells();

            for (int i = 0; i < ButterflyMaximum; i++)
            {
                var zCell = eZone.GetRandomElement();
                if (zCell != null)
                {
                    zCell.AddObject("Glimmerdust Butterfly");
                }
            }
            
            return base.HandleEvent(E);
        }
    }
}