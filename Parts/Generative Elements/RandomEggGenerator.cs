using System;
using XRL.Rules;

namespace XRL.World.Parts
{
    [Serializable]
    public class RandomEggGenerator : IPart
    {
        public RandomEggGenerator()
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
                render.Tile = "WingyTextures/creatures/buggeggsac" + Stat.Random(1, 3) + ".png";
                int num = Stat.Random(1, 3);
                if (num == 1)
                {
                    render.TileColor = "&K";
                }
                else if (num == 2)
                {
                    render.TileColor = "&b";
                }
                else if (num == 3)
                {
                    render.TileColor = "&M";
                }

                int num2 = Stat.Random(1, 4);
                if (num2 == 1)
                {
                    render.RenderString = "o";
                }

                if (num2 == 2)
                {
                    render.RenderString = "O";
                }

                if (num2 == 3)
                {
                    render.RenderString = "ù";
                }

                if (num2 == 4)
                {
                    render.RenderString = "ú";
                }

                this.ParentObject.RemovePart(this);
            }

            return true;
        }
    }
}