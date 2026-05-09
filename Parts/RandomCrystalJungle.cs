using Genkit;
using System;
using XRL.Rules;


namespace XRL.World.Parts
{
    [Serializable]
    public class RandomCrystalJungle : IPart
    {
        public string text = "";

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == ZoneBuiltEvent.ID;
            }

            return true;
        }

        public override bool HandleEvent(ZoneBuiltEvent E)
        {
            SetupJungle();
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "ZoneLoaded")
            {
                SetupJungle();
            }

            return base.FireEvent(E);
        }

        private void SetupJungle()
        {
            Render pRender = ParentObject.Render;
            int num5 = Stat.Random(1, 3);
            if (num5 == 1)
            {
                text = "a";
            }

            if (num5 == 2)
            {
                text = "b";
            }

            if (num5 == 3)
            {
                text = "c";
            }

            pRender.Tile = "crystholts" + Stat.Random(1, 3) + ".png";
            pRender.DetailColor = "y";
            if (ParentObject.Physics.CurrentCell.X > 0 && ParentObject.Physics.CurrentCell.X < 79 &&
                ParentObject.Physics.CurrentCell.Y > 0 && ParentObject.Physics.CurrentCell.Y < 24)
            {
                bool flag = ParentObject.Physics.CurrentCell.X % 2 == 0;
                if (GetSeededRange(ParentObject.Physics.CurrentCell.X / 2 + "," + ParentObject.Physics.CurrentCell.Y, 1,
                        4) == 1)
                {
                    Cell cellFromDirection = ParentObject.Physics.CurrentCell.GetCellFromDirection("E");
                    Cell cellFromDirection2 = ParentObject.Physics.CurrentCell.GetCellFromDirection("W");
                    if (cellFromDirection2 != null && cellFromDirection != null &&
                        ((flag && cellFromDirection.HasObjectWithBlueprint("TerrainJungle")) ||
                         (!flag && cellFromDirection2.HasObjectWithBlueprint("TerrainJungle"))))
                    {
                        if (flag)
                        {
                            pRender.Tile = "crystholtsleft" + Stat.Random(1, 2) + ".png";
                        }
                        else
                        {
                            pRender.Tile = "crystholtsright" + Stat.Random(1, 2) + ".png";
                        }
                    }
                }
            }

            if (Stat.Random(1, 5) <= 2)
            {
                int num6 = Stat.Random(1, 6);
                if (num6 == 1)
                {
                    pRender.TileColor = "&b";
                }

                if (num6 == 2)
                {
                    pRender.TileColor = "&B";
                }

                if (num6 == 3)
                {
                    pRender.TileColor = "&c";
                }

                if (num6 == 4)
                {
                    pRender.TileColor = "&C";
                }

                if (num6 == 5)
                {
                    pRender.TileColor = "&M";
                }

                if (num6 == 6)
                {
                    pRender.TileColor = "&m";
                }
            }

            ParentObject.RemovePart(this);
        }

        public static int GetSeededRange(string Seed, int Low, int High)
        {
            return new Random(Hash.String(Seed)).Next(Low, High);
        }
    }
}