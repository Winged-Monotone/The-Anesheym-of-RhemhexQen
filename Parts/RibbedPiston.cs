using XRL.World.Tinkering;

namespace XRL.World.Parts
{
    public class RibbedPiston : IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                || ID == CellChangedEvent.ID
                || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(CellChangedEvent E)
        {
            if (E.NewCell.IsValid() && !E.NewCell.HasPart<ModMassivelyOverloaded>() && E.NewCell.Blueprint == "Entropic Transducer")
            {
                ItemModding.ApplyModification(ParentObject, nameof(ModMassivelyOverloaded));
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            if (1.in100())
            {
                ItemModding.ApplyModification(ParentObject, nameof(ModGigantic));
            }
            return base.HandleEvent(E);
        }
    }
}