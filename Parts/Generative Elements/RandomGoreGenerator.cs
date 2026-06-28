using System;
using XRL.Rules;

namespace XRL.World.Parts
{
    [Serializable]
    public class RandomGoreGenerator : IPart
    {
        public RandomGoreGenerator()
        {
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("ObjectCreated");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "ObjectCreated")
            {
                Render render = this.ParentObject.GetPart("Render") as Render;
                render.Tile = "WingyTextures/amalgampustules" + Stat.Random(1, 3) + ".png";
            }

            return true;
        }
    }
}