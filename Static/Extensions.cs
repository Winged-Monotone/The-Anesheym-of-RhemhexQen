using XRL.Messages;
using XRL.World;
using XRL.World.Effects;

namespace Wingytone
{
    public static class Extensions
    {
        public static void BlindingFlashArmorEffect(this BeforeTookDamageEvent E, int Difficulty, int Chance, int Distance)
        {
            var Equipper = E.Actor.EquippedProperlyBy();

            if (Equipper ! == null)
            {

                if (E.Damage.IsLightDamage())
                    if (E.Source.IsCombatObject() && E.Source.DistanceTo(Equipper) == Distance && Chance.in100())
                    {
                        if (E.Source.MakeSave("Toughness", Difficulty - (E.Source.DistanceTo(Equipper)), Equipper, null))
                        {
                            E.Source.ApplyEffect(new Dazed());
                            MessageQueue.AddPlayerMessage("A blinding light flashes from your " + E.Actor.DisplayName +
                                                          ", dazing your attacker!");
                        }
                        if (E.Projectile.HasPropertyOrTag("Light"))
                        {
                            // <part Name="ItemElements" Elements="glass::5" />
                        }
                    }
            }
        }
        
        public static void ReduceAcidDamage(this BeforeTookDamageEvent E, int DamageReductionMultiplier)
        {
            var Equipper = E.Actor.EquippedProperlyBy();

            if (Equipper ! == null)
            {

                if (E.Damage.IsAcidDamage())
                {
                    E.Damage.Amount /= DamageReductionMultiplier;
                }
            }
        }

        public static void AbsorbElementalDamage(this BeforeTookDamageEvent E, int Chance)
        {
            
            //
            var Equipper = E.Actor.EquippedProperlyBy();
            var DamageValue = E.Damage.Amount;

            if (Equipper ! == null)
            {
                if (ContainsElementalDamage(E))
                {
                    if (Chance.in100() && Equipper.IsValid())
                    {
                        Equipper.Heal(DamageValue / 2, Message: true, false, false);
                        MessageQueue.AddPlayerMessage("Your " + E.Actor.DisplayName +
                                                      " glows warmly as it absorbs elemental energy from an attack! You are healed for " +
                                                      DamageValue + " points!");
                    }
                }
            }
        }

        public static bool ContainsElementalDamage(this BeforeTookDamageEvent E)
        {
            return E.Damage.IsColdDamage() && E.Damage.IsHeatDamage() && E.Damage.IsElectricDamage();
        }

        public static bool HasPrimaryNonNaturalWeapon(this BeforeTookDamageEvent E)
        {
            if (!E.Source.Equipped.IsNatural())
                return true;
            
            return false;
            
        }

    }
}