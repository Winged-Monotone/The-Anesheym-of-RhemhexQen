using System;
using XRL.Rules;
using XRL.World;

namespace XRL.World.Parts
{
    [Serializable]
    public class ButterflySwarmPack : IPart
    {
        public bool Created = false;
        
        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade) 
                || ID == EnteredCellEvent.ID;
        }

        public override bool HandleEvent(EnteredCellEvent E)
        {

            var Density = Stat.Random(1, 3);

            
            if (!Created && E.Object == ParentObject && E.Object.IsValid())
            {
                Created = true;
                
                var Cells = E.Object.CurrentCell.GetAdjacentCells(Radius: Density);
                
                    foreach (var Cell in Cells)
                    {
                        var Chance = 15.in100();
                        if (Chance)
                        {
                            var Butterling = Cell.AddObject("Glimmerling Butterflies");
                            Butterling.Brain.SetPartyLeader(ParentObject);
                        }
                    }
            }
            return base.HandleEvent(E);
        }
    }
}