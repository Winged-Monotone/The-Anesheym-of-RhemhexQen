// XRL.World.Parts.LifeGate

using XRL;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    public class GenopticonDoorUnlock : IPart
    {
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("AttemptDoorUnlock");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttemptDoorUnlock")
            {
                GameObject gameObjectParameter = E.GetGameObjectParameter("Actor");
                
                if (!gameObjectParameter.Inventory.HasObject("Genopticon Security Credentials"))
                {
                    return gameObjectParameter.ShowFailure("The door refuses to open. Requires Genopticon Security Credentials.");
                }
                
                if (ParentObject.TryGetPart<Door>(out var Part) && Part.Locked)
                {
                    Part.Unlock();
                    Part.AttemptOpen();
                    gameObjectParameter.ShowSuccess("The door responds to your Genopticon Security Credentials and opens");
                }

                ParentObject.RemovePart(this);
            }

            return base.FireEvent(E);
        }
    }
}