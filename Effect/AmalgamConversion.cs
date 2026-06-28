using XRL.Rules;

namespace XRL.World.Effects
{
    public class AmalgamConversion : Effect
    {
        public int CycleDuration = 1500;
        public int InfectionPhases = 4;
        
        
        public AmalgamConversion()
        {
            base.DisplayName = "{{Red|Vitamosis}}";
        }
        
        public override string GetDetails()
        {
            return
                "A skin slinker has slithered under your skin and is releasing enzymes, slowly transforming you into an amalgam.";
        }
        
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            if( Object == The.Player)
                --CycleDuration;
            else
            {
                CycleDuration -= 150;
            }
            if (CycleDuration == 0)
            {
                CycleDuration = 1500;
                --InfectionPhases;
                if (InfectionPhases <= 0)
                {
                    Object.ReplaceWith("Amalgam" + Stat.Random(1, 4));
                    if (Object == The.Player)
                    {
                        The.Player.Die(Reason: "You succumb to the amalgam surge.");
                    }
                    else
                    {
                        Object.ReplaceWith("Amalgam1");
                        XDidY(Object, "flesh unravels as it transforms", "into an amalgam", SubjectPossessedBy: Object);
                    }
                }
            }
            return base.HandleEvent(E);
        }
        
        
    }
}