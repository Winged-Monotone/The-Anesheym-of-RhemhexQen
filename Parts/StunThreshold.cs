using XRL.World.Effects;
using StunningForce = XRL.World.Parts.Mutation.StunningForce;

namespace XRL.World.Parts
{
    public class StunThreshold : IPart
    {
        public int StunCap;

        public override bool WantEvent(int ID, int cascade)
        {
            return ID == BeginTakeActionEvent.ID
                   || ID == EndTurnEvent.ID
                   || base.WantEvent(ID, cascade);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            if (StunCap > 0)
            {
                --StunCap;
            }
            
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeginTakeActionEvent E)
        {
            if (E.Object == ParentObject)
            {
                if (ParentObject.HasEffect<Stun>())
                {
                    StunCap += 3;

                    if (StunCap > 7)
                    {
                        // trying to stun lock me? fuck you ...
                        AddPlayerMessage(ParentObject.DisplayName + " has absorbed your concussive attacks!");
                        StunningForce.Concussion(ParentObject.CurrentCell, ParentObject, 10, 4, 1, Stun: false);
                        ParentObject.RemoveEffect<Stun>();
                    }
                }
            }
            
            
            return base.HandleEvent(E);
        }
    }
}