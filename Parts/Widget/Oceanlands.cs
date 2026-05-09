using System;


namespace XRL.World.Parts
{
    [Serializable]
    public class Oceanlands
    {
        public Oceanlands()
        {
        }

        public bool BuildZone(Zone Z)
        {
            try
            {
                Zone parentZone = Z;
                var ZoneCells = Z.GetCells();

                foreach (var c in ZoneCells)
                {
                    c.AddObject("SaltyWaterExtraDeepPool");
                }
            }
            catch
            {
            }

            return true;
        }
    }
}