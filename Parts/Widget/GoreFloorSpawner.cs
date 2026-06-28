namespace XRL.World.Parts
{
    public class GoreFloorSpawner : IPart
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
                if (!C.HasObjectWithPart("Floor"))
                    C.AddObject("Gore Mess");
            }

            return base.HandleEvent(E);
        }
    }
}