namespace XRL.World.Parts
{
    public class NoHostility : IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                   || ID == GetFeelingEvent.ID;
        }

        public override bool HandleEvent(GetFeelingEvent E)
        {
            E.Feeling = 0;
            return false;
        }
        
        
    }
}