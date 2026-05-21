using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using XRL;
using XRL.Rules;
using XRL.UI.ObjectFinderClassifiers;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;


namespace XRL.World.Parts
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
            // AddPlayerMessage("SEQ 1");

            if (Spawn)
            {
                return base.HandleEvent(E);
            }
            
            // AddPlayerMessage("SEQ 2");

            Spawn = true;
            
            // AddPlayerMessage("SEQ 3");


            var Cell =  ParentObject.CurrentCell.GetRandomLocalAdjacentCell();
            var Deer = ParentObject.CurrentCell.GetDirectionFromCell(Cell);
            
            // AddPlayerMessage("SEQ 4");


            var previous = ParentObject;
            
            // AddPlayerMessage("SEQ 5");

            var snakeLength = Stat.Random(8,18);

            for (int i = 0; i < snakeLength && Cell != null; i++)
            {
                // AddPlayerMessage("SEQ 6");
                
               var Objtail =  Cell.AddObject("LeviathanTail");
               var TailPart = new wmLeviathanTail();
               
               TailPart.SetLead(previous);
               previous = Objtail;

               Objtail.AddPart(TailPart);
               
                Cell = Cell.GetCellFromDirection(Deer);
                
                // AddPlayerMessage("SEQ 7");
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