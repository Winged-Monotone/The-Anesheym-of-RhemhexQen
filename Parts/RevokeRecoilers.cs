namespace XRL.World.Parts
{
    public class RevokeRecoilers : IPart
    {
        
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                   || ID == EnteringZoneEvent.ID ;
        }

        public override bool HandleEvent(EnteringZoneEvent E)
        {
            if (NeutronFluxDetonation.NFDGoesBoom && E.Actor.IsPlayer())
            {
                AddPlayerMessage(
                    "{{red|Your recoiler returns an error.");
                
                return false;
            }

            return base.HandleEvent(E);
        }
    }
}