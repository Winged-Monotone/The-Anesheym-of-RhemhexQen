// XRL.World.Parts.LifeGate

using XRL;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    public class wm_DoorUnlocker : IPart
    {
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("AttemptDoorUnlock");
            Registrar.Register("CanAttemptDoorUnlock");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "CanAttemptDoorUnlock")
            {
                GameObject Opener = E.GetGameObjectParameter("Opener");
                E.GetParameter("Door");
                if (ParentObject.Brain != null)
                {
                    return true;
                }
                if (Opener == null)
                {
                    return false;
                }
                if (!Opener.IsPlayer())
                {
                    return false;
                }
            }
            if (E.ID == "AttemptDoorUnlock")
            {
                GameObject Opener = E.GetGameObjectParameter("Actor");
                
                if (!The.Game.HasQuest("Egghunt"))
                {
                    return Opener.ShowFailure("The gates are sealed.");
                }

                if (!The.Game.HasFinishedQuest("Egghunt"))
                {
                    return Opener.ShowFailure(
                        "The gates are secured shut until you fulfill your promise to the Anesheym.");
                }

                if (ParentObject.TryGetPart<Door>(out var Part) && Part.Locked)
                {
                    Part.ParentObject.SetIntProperty("NoClose", 1);
                    Part.Unlock();
                    Part.WasLocked = false;
                    AddPlayerMessage("The gates swing wide.");
                    Part.AttemptOpen();
                }
            }

            return base.FireEvent(E);
        }
    }
}