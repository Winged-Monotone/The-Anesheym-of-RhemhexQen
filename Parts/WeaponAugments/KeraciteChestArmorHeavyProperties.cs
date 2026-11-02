using XRL.World.Effects;
using XRL.World.Capabilities;

namespace XRL.World.Parts
{
    public class KeraciteChestArmorHeavyProperties : IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                   || ID == EquippedEvent.ID
                   || ID == UnequippedEvent.ID
                   || ID == GetShortDescriptionEvent.ID;
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            base.Register(Object, Registrar);
            
            // this registers for BeforeTookDamageEvent when the part is added if the object is equipped.
            // For example, when a save is loaded.
            if (Object.Equipped.IsValid())
            {
                Registrar.Register(Object.Equipped,BeforeTookDamageEvent.ID);
            }
        }
        
        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.Append
                ("\n\n" +
                 "{{|bright royal purple|Nanoglass Saccharnite}}\n" +
                 "30% chance to apply the Daze (DC 16 Toughness Saving Throw) effect to adjacent enemies if struck by a light-based weapon, has light-obfuscating properties.\n\n" +
                 "{{|bright royal purple|Robust Material}}\n" +
                 "\nCannot be destroyed via acid, disintegration, or other effects. Cannot be rusted." +
                 "\n");
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(EquippedEvent E)
        {
            E.Actor.RegisterEvent(this, BeforeTookDamageEvent.ID);
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(UnequippedEvent E)
        {
            E.Actor.UnregisterEvent(this, BeforeTookDamageEvent.ID);
            return base.HandleEvent(E);
        }
    }
}