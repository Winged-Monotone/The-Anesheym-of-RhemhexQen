using XRL.World.Effects;
using XRL.World.Capabilities;

namespace XRL.World.Parts
{
    public class KeraciteChestArmorLightProperties : IPart
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
                 "there is 30% chance that whenever bleed or poisoned is applied to you, its effects will be reversed and you'll instead gain hit points from these effects.\n\n" +
                 "{{|bright royal purple|Nanoaccelerated Reflexes}}\n" +
                 "When being attacked by an enemy, there is a 30% chance that your DV will be calculated as if it is doubled.\n\n" +
                 "{{|bright royal purple|Nanoglass Quantum-refractions}}\n" +
                 "Whenever an attack misses you, there is a 30% chance that you create a quantum-fugue of yourself in an adjacent space aroun1d you. (You can only create a maximum of 2 fugues at a time.) \n\n" +
                 "{{|bright royal purple|Glitterweave Rejuvenation}}\n" +
                 "Whenever an enemy misses you and your Juke ability is on cooldown, there is a 50% chance that your Juke ability will be instantly refreshed.\n\n" +
                 "{{|bright royal purple|Glitterweave Impulse Flux}}\n" +
                 "Whenever you avoid a projectile, there is a 30% chance you will juke for free.\n\n" +
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