using System;
using XRL.Rules;

namespace XRL.World.Parts
{
    [Serializable]
    public class RandomColorGenerator : IPart
    {
        public RandomColorGenerator()
        {
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            Object.RegisterPartEvent(this, "ObjectCreated");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "ObjectCreated")
            {
                Render render = this.ParentObject.GetPart("Render") as Render;
                int num = Stat.Random(1, 10);
                if (num == 1)
                {
                    render.ColorString = "&R";
                }
                else if (num == 2)
                {
                    render.ColorString = "&O";
                }
                else if (num == 3)
                {
                    render.ColorString = "&W";
                }
                else if (num == 4)
                {
                    render.ColorString = "&G";
                }
                else if (num == 5)
                {
                    render.ColorString = "&C";
                }
                else if (num == 6)
                {
                    render.ColorString = "&c";
                }
                else if (num == 7)
                {
                    render.ColorString = "&B";
                }
                else if (num == 8)
                {
                    render.ColorString = "&b";
                }
                else if (num == 9)
                {
                    render.ColorString = "&M";
                }
                else if (num == 10)
                {
                    render.ColorString = "&m";
                }

                this.ParentObject.RemovePart(this);
            }

            return true;
        }
    }
}