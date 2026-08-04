using Genkit;
using XRL.Rules;
using XRL.World;
using EMPMutationPart = XRL.World.Parts.Mutation.ElectromagneticPulse;

namespace XRL.World.Parts
{
    public class SphericalStaticBurst : IPart
    {
        public int Destination = -1;
        public int EMPBuffer = 3;

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == SingletonEvent<EndTurnEvent>.ID
                       || ID == BeforeDestroyObjectEvent.ID;
            }

            return true;
        }

        public override bool HandleEvent(BeforeDestroyObjectEvent E)
        {
            
            EMPMutationPart.EMP(ParentObject.CurrentCell, Stat.Random(3,7), 2);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            
            if (!ParentObject.IsValid() || ParentObject.IsNowhere())
            {
                return base.HandleEvent(E);
            }

            var Locations = TheEncodedOne.CellListParticular;

            if (Destination == -1)
            {
                var MinDis = int.MaxValue;
                var MinLoc = Location2D.Invalid;
                var CurLoc = ParentObject.CurrentCell.Location;
                
                foreach (var L in Locations)
                {
                    var Dist = CurLoc.Distance(L);
                    
                    if (Dist < MinDis)
                    {
                        MinDis = Dist;
                        MinLoc = L;
                    }
                }

                Destination = Locations.IndexOf(MinLoc) + 1;
                Destination = Destination % Locations.Count;
            }

            var DesLoc = Locations[Destination];

            if (currentCell.Location == DesLoc)
            {
                Destination++;
                Destination = Destination % Locations.Count;
            }

            ParentObject.Move(ParentObject.GetDirectionToward(DesLoc), true, true,true);

            --EMPBuffer;
            var ElectroBustChance = Stat.Random(1, 3);
            if (EMPBuffer <= 0)
            {
                if (ElectroBustChance < 1)
                {
                    EMPMutationPart.EMP(ParentObject.CurrentCell, 2, 2, true);
                    EMPBuffer = 3;
                }
                else if (ElectroBustChance == 1)
                {
                    var AdjCells = ParentObject.CurrentCell.GetRandomLocalAdjacentCellAtRadius(Stat.Random(1, 7));
                    if (ParentObject.IsValid())
                        ParentObject.Discharge(AdjCells, Damage: "5d5".Roll(), Voltage: 3);
                }
            }

            return base.HandleEvent(E);
        }
    }
}