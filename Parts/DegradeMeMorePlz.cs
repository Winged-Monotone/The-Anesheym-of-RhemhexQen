using XRL.World.Effects;

namespace XRL.World.Parts
{
    public class DegradeMeMorePlz : IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                || ID == EquippedEvent.ID
                || ID == UnequippedEvent.ID;
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            base.Register(Object, Registrar);
            
            // this registers for ApplyEffectEvent when the part is added if the object is equipped.
            // For example, when a save is loaded.
            if (Object.Equipped.IsValid())
            {
                Registrar.Register(Object.Equipped,ApplyEffectEvent.ID);
            }
        }

        public override bool HandleEvent(EquippedEvent E)
        {
            E.Actor.RegisterEvent(this, ApplyEffectEvent.ID);
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(UnequippedEvent E)
        {
            E.Actor.UnregisterEvent(this, ApplyEffectEvent.ID);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ApplyEffectEvent E)
        {
            if (E.Effect is Shamed fx)
            {
                ParentObject.Equipped.ApplyEffect(new Spurred(fx.Duration));
                return false;
            }
            
            return base.HandleEvent(E);
        }
    }
}