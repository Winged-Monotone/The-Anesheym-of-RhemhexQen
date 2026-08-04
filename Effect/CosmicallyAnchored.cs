using XRL.World.Parts;

namespace XRL.World.Effects
{
    public class CosmicallyAnchored : Effect
    {
        public CosmicallyAnchored()
        {
            DisplayName = "{{starry|cosmically anchored}}";
        }

        public override string GetDetails()
        {
            string text = "An unknown force is holding you to this strata, you cannot leave.";
            
            return text;
        }

        public override void Applied(GameObject Object)
        {
            AddPlayerMessage("{{red|You are being cosmically anchored! You can't leave this area!}}");

            base.Applied(Object);
        }

        public override void Remove(GameObject Object)
        {
            AddPlayerMessage("{{green|You are no longer being held here by some form of cosmic anchoring.}}");
            base.Remove(Object);
        }

        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                   || ID == EnteringCellEvent.ID
                   || ID == EnteringZoneEvent.ID
                   || ID == EndTurnEvent.ID;
        }
        
        public override bool HandleEvent(EndTurnEvent E)
        {
            if (!Object.GetCurrentZone().HasObject("EncodedEater"))
            {
                AddPlayerMessage(
                    "{{green|You are no longer cosmically anchored.}}");
                Duration = 0;
                Object.RemoveEffect(this);
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnteringZoneEvent E)
        {
            if (E.Cell.ParentZone.ZoneID != "JoppaWorld.33.3.1.1.29845658")
            {
                AddPlayerMessage(
                    "{{red|You are being held here by some form of cosmic anchoring, you can't leave this stratum!}}");
                return false;
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnteringCellEvent E)
        {
            if (E.Object.IsPlayer() && !TheEncodedOne.Box2D.contains(E.Cell.Location))
            {
                if (E.Type == "Teleporting")
                {
                    AddPlayerMessage("{{red|You can't recoil while cosmically anchored!}}");
                }
                else
                    AddPlayerMessage("{{red|You are being held here by some form of cosmic anchoring.}}");

                return false;
            }
            
            return base.HandleEvent(E);
        }
    }
}