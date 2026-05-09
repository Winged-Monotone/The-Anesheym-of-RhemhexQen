using System;
using XRL.Rules;

namespace XRL.World.Parts
{
    [Serializable]
    public class TerrainRandomizerBeviddSulfuricDeserts : IPart
    {
        public TerrainRandomizerBeviddSulfuricDeserts()
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
                render.Tile = "terrain/SulfuricWastland" + Stat.Random(1, 5) + ".png";
                int num = Stat.Random(1, 2);
                if (num == 1)
                {
                    render.DetailColor = "o";
                }
                else if (num == 2)
                {
                    render.DetailColor = "O";
                }

                int num2 = Stat.Random(1, 4);
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