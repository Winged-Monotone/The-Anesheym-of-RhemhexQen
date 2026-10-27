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
                 "{{|bright royal purple|Nanochimeric Polymer}}\n" +
                 "30% chance to nullify and then convert half of heat, cold or electric damage into health.\n\n" +
                 "{{|bright royal purple|Acidic Debaser}}\n" +
                 "All acid damage is reduced by half.\n\n" +
                 "{{|bright royal purple|Nanoglass Saccharnite}}\n" +
                 "30% chance to apply the Daze (DC 16 Toughness Saving Throw) effect to adjacent enemies if struck by a light-based weapon, has light-obfuscating properties.\n\n" +
                 "{{|bright royal purple|Robust Material}}\n" +
                 "15% chance to disarm a target (DC 15 Strength Saving Throw) attacking you so long as they are using blunt weapons and it's not a natural appendage." +
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

        public override bool HandleEvent(BeforeTookDamageEvent E)
        {
            var Equipper = E.Actor.EquippedProperlyBy();
            var DamageValue = E.Damage.Amount;

            if (Equipper ! == null)
            {
                if (ContainsElementalDamage(E))
                {
                    if (30.in100() && Equipper.IsValid())
                    { 
                        Equipper.Heal(DamageValue/2, Message: true, false, false);
                        AddPlayerMessage("Your " + E.Actor.DisplayName + " glows warmly as it absorbs elemental energy from an attack! You are healed for " + DamageValue + " points!");
                        return false;
                    }
                }
                else if (E.Damage.IsAcidDamage())
                {
                    E.Damage.Amount /= 2;
                }
                else if (E.Damage.IsLightDamage())
                    if (E.Source.IsCombatObject() && E.Source.DistanceTo(Equipper) == 2 && 30.in100())
                    {
                        if (E.Source.MakeSave("Toughness", 16 - (E.Source.DistanceTo(Equipper)), Equipper, null))
                        {
                            E.Source.ApplyEffect(new Dazed());
                            AddPlayerMessage("A blinding light flashes from your " + E.Actor.DisplayName +
                                             ", dazing your attacker!");
                        }
                        if (E.Projectile.HasPropertyOrTag("Light"))
                        {
                            // <part Name="ItemElements" Elements="glass::5" />
                        }
                    }
                    else if (E.Damage.IsBludgeoningDamage() && HasPrimaryNonNaturalWeapon(E))
                    {
                        if (E.Source.IsCombatObject() && E.Source.DistanceTo(Equipper) == 2 && 30.in100())
                        {
                            Disarming.Disarm(E.Source, Equipper, 15, Weapon: E.Source.GetPrimaryWeapon());
                        }
                    }
            }
            
            return base.HandleEvent(E);
        }

        private static bool ContainsElementalDamage(BeforeTookDamageEvent E)
        {
            return E.Damage.IsColdDamage() && E.Damage.IsHeatDamage() && E.Damage.IsElectricDamage();
        }

        private static bool HasPrimaryNonNaturalWeapon(BeforeTookDamageEvent E)
        {
            if (!E.Source.Equipped.IsNatural())
                return true;
            
            return false;
            
        }
    }
}