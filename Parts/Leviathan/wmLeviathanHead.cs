using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using XRL;
using XRL.UI.ObjectFinderClassifiers;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;


namespace XRL.World.Parts.Mutation
{
    [Serializable]
    
    public class wmLeviathanHead : IPart
    {
        public bool Spawn;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == OnDeathRemovalEvent.ID
                   || ID == EnteredCellEvent.ID;
        }

        public override bool HandleEvent(EnteredCellEvent E)
        {
            if (Spawn == true)
            {
                return base.HandleEvent(E);
            }
            
            Spawn = true;

            var Cell =  ParentObject.CurrentCell.GetRandomLocalAdjacentCell();
            var Deer = ParentObject.CurrentCell.GetDirectionFromCell(Cell);

            for (int i = 0; i < 8 && Cell != null; i++)
            {
                Cell.AddObject("LeviathanTail");
                Cell = Cell.GetCellFromDirection(Deer);
            }
            
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(OnDeathRemovalEvent E)
        {
            if (E.Dying == ParentObject && !E.Dying.IsFrozen())
            {
                Physics.ApplyExplosion(ParentObject.CurrentCell, 150);
                ParentObject.CurrentCell.TemperatureChange(25);
                ParentObject.CurrentCell.Splash("*");
                foreach (Cell C in ParentObject.CurrentCell.GetAdjacentCells())
                {
                    C.AddObject("SulfuricGas50");
                    C.AddObject("SulfurPuddle");
                }
            }

            return base.HandleEvent(E);
        }
    }
}