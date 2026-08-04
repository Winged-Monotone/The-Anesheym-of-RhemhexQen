// XRL.World.Parts.LifeGate

using XRL;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    public class LongSighSecurityDoorUnlock : IPart
    {
        public int SecurityCredentialsLevel;

        public bool IsAuthorized(GameObject IDObject)
        {
            if (IDObject.GetBlueprint().TryGetTag("LongSighClearance", out var value) &&
                int.TryParse(value, out var result) && result >= SecurityCredentialsLevel)
            {
                return true;
            }

            return false;
        }
        
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("AttemptDoorUnlock");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttemptDoorUnlock")
            {
                GameObject eActor = E.GetGameObjectParameter("Actor");
                
                if (!eActor.HasObjectInInventory(IsAuthorized))
                {
                    return eActor.ShowFailure("The door refuses to open. Requires proper security credentials.");
                }
                if (ParentObject.TryGetPart<Door>(out var Part) && Part.Locked)
                {
                    Part.Unlock();
                    Part.AttemptOpen();
                    eActor.ShowSuccess("The door responds to your security credentials and opens");
                }

                ParentObject.RemovePart(this);
            }

            return base.FireEvent(E);
        }
    }
}