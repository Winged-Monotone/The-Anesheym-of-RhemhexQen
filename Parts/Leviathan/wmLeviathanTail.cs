namespace XRL.World.Parts.Mutation
{
    public class wmLeviathanTail : IPart
    {
        public GameObject Lead;
        
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            if (Lead.IsValid())
            {
                Registrar.Register(Lead, EnteringCellEvent.ID);
            }
        }

        public void SetLead(GameObject obj)
        {
            Lead = obj;
            obj.RegisterEvent(this, EnteringCellEvent.ID);
        }
        
        public override bool HandleEvent(EnteringCellEvent E)
        {
            var exit = Lead.CurrentCell;
            if (exit == null) return true;

            var enter = E.Cell;
            if (enter == null || enter == ParentObject.CurrentCell) return true;
            if (enter.IsAdjacentTo(ParentObject.CurrentCell)) return true;

            ParentObject.SystemMoveTo(exit, 0, true);
            return true;
        }
    }
}