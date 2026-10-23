using XRL.Rules;
using mwPyrokinetics = XRL.World.Parts.Mutation.Pyrokinesis;
using mwFlamingRay = XRL.World.Parts.Mutation.FlamingRay;


namespace XRL.World.Parts 
{
    public class AmbientHeatDeath : IPart
    {
        public int Duration = 50;
        
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            var eZone = ParentObject.GetCurrentZone();
            var CombatObjects = eZone.GetObjects();

            var WarningChance = 5.in100();

            if (WarningChance)
            {
                XDidY(ParentObject, "raising", "the ambient temperature in the room!", ColorAsBadFor: ThePlayer);
            }

            foreach (var O in CombatObjects)
            {
                if (O.IsValid() && O.IsCombatObject() && O.Blueprint != ("EncodedEater"))
                {
                    O.Physics.Temperature += 50;
                }
            }

            --Duration;
            
            return base.HandleEvent(E);
        }
        
        
    }
}