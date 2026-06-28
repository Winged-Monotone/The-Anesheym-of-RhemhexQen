using XRL.World;

namespace XRL.World.Parts
{
    public class ObservableAmalgam : IPart
    {
        
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("AfterLookedAt");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AfterLookedAt")
            {
                if (!The.Player.HasPart<ObservedAmalgam>())
                {
                    ThePlayer.AddPart<ObservedAmalgam>();
                    ThePlayer.ShowSuccess("You note your observation of a most horrid monstrosity.");
                }
            }
            return base.FireEvent(E);
        }
    }
    
}