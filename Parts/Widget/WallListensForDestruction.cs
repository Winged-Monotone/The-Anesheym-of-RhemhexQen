namespace XRL.World.Parts
{
    public class WallListensForDestruction : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == LeftCellEvent.ID;
            }

            return true;
        }


        public override bool HandleEvent(LeftCellEvent E)
        {
            if (E.Object.IsWall())
            {
                E.Cell.AddObject("OmniForcefield");
            }
            return base.HandleEvent(E);
        }
        
        
    }
}