using System;
using XRL.Rules;
using XRL.UI;
using XRL.World;

namespace XRL.World.Parts
{
    [Serializable]
    public class Crystallands : IPart
    {
        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == EnteredCellEvent.ID;
            }

            return true;
        }

        public override bool HandleEvent(EnteredCellEvent E)
        {
            try
            {
                if (!Options.DisableFloorTextureObjects)
                {
                    Zone parentZone = E.Cell.ParentZone;
                    for (int i = 0; i < parentZone.Width; i++)
                    {
                        for (int j = 0; j < parentZone.Height; j++)
                        {
                            PaintCell(parentZone.GetCell(i, j));
                        }
                    }
                }
            }
            catch
            {
            }

            return true;
        }

        public static void PaintCell(Cell C)
        {
            int num = Stat.Random(1, 10);
            if (num <= 1)
            {
                C.PaintColorString = "&C";
                C.PaintTile = DirtPicker.GetRandomGrassTile();
            }
            else if (num <= 2)
            {
                C.PaintColorString = "&B";
                C.PaintTile = DirtPicker.GetRandomGrassTile();
            }
            else if (num == 3)
            {
                C.PaintColorString = "k";
                C.PaintTile = DirtPicker.GetRandomGrassTile();
            }
            else
            {
                C.PaintColorString = "k";
                C.PaintTile = "Tiles/tile-dirt1.png";
            }

            int num2 = Stat.Random(1, 5);
            if (num2 == 1)
            {
                C.PaintRenderString = ".";
            }

            if (num2 == 2)
            {
                C.PaintRenderString = ",";
            }

            if (num2 == 3)
            {
                C.PaintRenderString = "`";
            }

            if (num2 == 4)
            {
                C.PaintRenderString = "'";
            }
        }
    }
}