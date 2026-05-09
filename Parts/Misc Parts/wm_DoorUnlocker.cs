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
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttemptDoorUnlock")
            {
                GameObject gameObjectParameter = E.GetGameObjectParameter("Actor");
                if (!The.Game.HasQuest("The Great Egg-Hunt Begins"))
                {
                    return gameObjectParameter.ShowFailure("The gates are sealed.");
                }

                if (!The.Game.HasFinishedQuest("The Great Egg-Hunt Begins"))
                {
                    return gameObjectParameter.ShowFailure(
                        "The gates are secured shut until you fulfill your promise to the Anesheym.");
                }

                if (ParentObject.TryGetPart<Door>(out var Part) && Part.Locked)
                {
                    Part.Unlock();
                    Part.AttemptOpen();
                }

                ParentObject.RemovePart(this);
            }

            return base.FireEvent(E);
        }
    }
}