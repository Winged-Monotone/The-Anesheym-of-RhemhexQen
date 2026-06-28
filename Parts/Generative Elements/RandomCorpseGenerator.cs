using System;
using XRL.Rules;

namespace XRL.World.Parts
{
    [Serializable]
    public class RandomCorpseGenerator : IPart
    {
        public RandomCorpseGenerator()
        {
            // base.Name = "RandomCorpseGenerator";
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

        private static string[][] randomTiles = new string[][]
        {
            new string[]
            {
                "WingyTextures/Retextures/sw_crystalcorspe2.png",
                "WingyTextures/Retextures/sw_crystalcorspe1.png",
                "WingyTextures/Retextures/sw_crystalcorspe8.png",
            },

            new string[]
            {
                "WingyTextures/Retextures/sw_crystalcorspe7.png",
                "WingyTextures/Retextures/sw_crystalcorspe5.png",
            },

            new string[]
            {
                "WingyTextures/Retextures/sw_crystalcorspe9.png",
                "WingyTextures/Retextures/sw_crystalcorspe4.png",
                "WingyTextures/Retextures/sw_crystalcorspe6.png",
            }
        };


        public override bool FireEvent(Event E)
        {
            Render render = this.ParentObject.GetPart("Render") as Render;
            if (E.ID == "ObjectCreated")
            {
                this.ParentObject.RemovePart(this);

                if (Stat.Random(1, (1000)) == 1)
                {
                    render.Tile = "WingyTextures/Retextures/sw_crystalcorspe3.png";
                }
                else if (Stat.Random(1, (500)) == 1)
                {
                    render.Tile = "WingyTextures/Retextures/sw_crystalcorspe10.png";
                }
                else
                {
                    render.Tile = randomTiles.GetRandomElement().GetRandomElement();
                }
            }

            return true;
        }
    }
}