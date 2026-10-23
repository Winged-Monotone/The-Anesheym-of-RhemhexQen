using System;

namespace XRL.World.Parts
{
    public class KeraciteGauntletsArmorProperties : IPart
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
            

            if (Object.Equipped.IsValid())
            {
                Registrar.Register(Object.Equipped,AttackerDealingDamageEvent.ID);
                Registrar.Register(Object.Equipped,AttackerDealtDamageEvent.ID);
            }
        }
        
        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.Append
                ("{{Orange|Impact Nanomodulation}}" +
                 "\nStriking enemies with melee weapons while wearing these gauntlets recharges cells in your inventory. If not wielding weapons, dealing attacks with your fist with this item equipped has a 30% chance of adding a random elemental effect to your attacks as well as increasing its damage by 5%.\n\n" +
                 "{{Orange|Nanite Acceleration}}" +
                 "\n+20% to offhand strike chance.\n\n" +
                 "{{Orange|Defensibly Responsible}}" +
                 "\n10% chance to block an attack.\n\n" +
                 "Cannot be destroyed via acid, disintegration, or other effects. Cannot be rusted.\n\n");
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(EquippedEvent E)
        {
            E.Actor.RegisterEvent(this, AttackerDealingDamageEvent.ID);
            E.Actor.RegisterEvent(this, AttackerDealtDamageEvent.ID);
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(UnequippedEvent E)
        {
            StatShifter.RemoveStatShifts();
            E.Actor.UnregisterEvent(this, AttackerDealingDamageEvent.ID);
            E.Actor.UnregisterEvent(this, AttackerDealtDamageEvent.ID);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AttackerDealtDamageEvent E)
        {
            var Equipper = E.Actor.EquippedProperlyBy();

            if (E.Actor == Equipper && E.Damage.Amount < 0)
            {
                var eInv = Equipper.Inventory.GetObjects();

                foreach (var O in eInv)
                {
                    if (O.HasPart<EnergyCell>())
                    {
                        var oCharge = O.GetPart<EnergyCell>();
                        oCharge.Charge += 15;
                    }
                }
            }
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            var Equipper = E.Actor.EquippedProperlyBy();

            if (E.Actor == Equipper && E.Damage.Amount < 0)
            {
                var DamageValue = E.Damage.Amount;
                var ValueIncrease = DamageValue * 0.05;
                var newDamageAdd = (int)Math.Round(ValueIncrease);
                var NewDamageInstance = new Damage(newDamageAdd);
                // create a new method that chooses a random element
                // NewDamageInstance.
                E.Damage.Amount += (int)Math.Round(ValueIncrease);
            }
            
            return base.HandleEvent(E);
        }
        
        
    }
}