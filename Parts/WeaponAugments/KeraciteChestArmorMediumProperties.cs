using Wingytone;
using XRL.World.Effects;
using XRL.World.Capabilities;

namespace XRL.World.Parts
{
    public class KeraciteChestArmorMediumProperties : IPart
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
                ("{{orange|\n\n" +
                 "{{|bright royal purple|Nanochimeric Polymer}}\n" +
                 "30% chance to nullify and then convert half of heat, cold or electric damage into health.\n\n" +
                 "{{|bright royal purple|Nanoglass Reflections}}\n" +
                 "Whenever an attack misses you, there is a 30% chance that you create a holographic distraction of yourself in an adjacent space around you. (You can only create a maximum of 2 holograms at a time.) \n\n" +
                 "{{|bright royal purple|Keracite Rejuvenation}}\n" +
                 "Whenever an enemy misses you and your Juke ability is on cooldown, there is a 30% chance that your Juke ability will be instantly refreshed.\n\n" +
                 "{{|bright royal purple|Dashing Appearance}}\n" +
                 "Treat all mercantile trades as if you had +6 Ego.\n\n" +
                 "\nCannot be destroyed via acid, disintegration, or other effects. Cannot be rusted." +
                 "\n}}");
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

        public override bool HandleEvent(BeforeTookDamageEvent E)
        {
            
            return base.HandleEvent(E);
        }
        
    }
}