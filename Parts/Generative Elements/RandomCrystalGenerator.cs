using System;
using XRL.Rules;

namespace XRL.World.Parts
{
    [Serializable]
    public class RandomCrystalGenerator : IPart
    {
        public RandomCrystalGenerator()
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
                render.Tile = "creatures/sw_crystal" + Stat.Random(1, 4) + ".bmp";
                int num = Stat.Random(1, 3);
                if (num == 1)
                {
                    render.ColorString = "&M";
                }
                else if (num == 2)
                {
                    render.ColorString = "&C";
                }
                else if (num == 3)
                {
                    render.ColorString = "&y";
                }

                if (Stat.Random(0, 1) == 0)
                {
                    render.ColorString = render.ColorString.ToLower();
                }

                int num2 = Stat.Random(1, 5);
                if (num2 == 1)
                {
                    render.RenderString = ",";
                }

                if (num2 == 2)
                {
                    render.RenderString = ".";
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