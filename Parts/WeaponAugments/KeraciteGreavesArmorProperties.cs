namespace XRL.World.Parts
{
    public class KeraciteGreavesArmorProperties : IPart
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
                Registrar.Register(Object.Equipped,EnteredCellEvent.ID);
            }
        }
        
        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.Append
                ("{{Orange|Kinetic Nanomodulation}}" +
                 "\nMovement regenerates energy to all carried cells with exception of liquid-bearing cells.\n\n" +
                 "{{Orange|Nanite Acceleration}}" +
                 "\n+30 to quickness.\n\n" +
                 "Cannot be destroyed via acid, disintegration, or other effects. Cannot be rusted.\n");
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(EquippedEvent E)
        {
            StatShifter.SetStatShift(E.Actor, "Quickness", -30);
            E.Actor.RegisterEvent(this, EnteredCellEvent.ID);
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(UnequippedEvent E)
        {
            StatShifter.RemoveStatShifts();
            E.Actor.UnregisterEvent(this, EnteredCellEvent.ID);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnteredCellEvent E)
        {
            var Equipper = E.Actor.EquippedProperlyBy();

            if (E.Actor == Equipper)
            {
                var eInv = Equipper.Inventory.GetObjects();

                foreach (var O in eInv)
                {
                    if (O.HasPart<EnergyCell>())
                    {
                        var oCharge = O.GetPart<EnergyCell>();
                        oCharge.Charge += 5;
                    }
                }
            }
            return base.HandleEvent(E);
        }
    }
}