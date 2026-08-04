using XRL.World.Effects;

namespace XRL.World.Parts
{
    public class FreezeThreshold : IPart
    {
        public int OverallDuration = 0;
        
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                   || ID == EndTurnEvent.ID;
        }


        public override bool HandleEvent(EndTurnEvent E)
        {
            var Frozen = ParentObject;
            
            if (Frozen.IsValid() && Frozen.HasEffect<Frozen>())
            { 
                    AddPlayerMessage(ParentObject.Its + "thermal-regulation vents begin to glow, the ice is melting faster!");
                    
                    ++OverallDuration;
                    
                    ParentObject.Physics.Temperature += 1000 + (OverallDuration*1000);
            }
            
            return base.HandleEvent(E);
        }
    }
}