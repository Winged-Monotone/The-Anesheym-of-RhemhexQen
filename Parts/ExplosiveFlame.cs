using XRL.World;

namespace XRL.World.Parts
{
    public class ExplosiveFlame : IPart
    {
        public GameObject Owner;

        public ExplosiveFlame()
        {
            
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return ID == OnDestroyObjectEvent.ID;
        }

        public override bool HandleEvent(OnDestroyObjectEvent E)
        {
            E.Object.Explode(50000, Owner, SuppressDestroy: true);
            return base.HandleEvent(E);
        }
    }
}