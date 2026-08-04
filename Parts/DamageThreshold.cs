namespace XRL.World.Parts
{
    public class DamageThreshold : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return ID == BeforeApplyDamageEvent.ID 
                   || ID == BeforeDestroyObjectEvent.ID;
        }

        public override bool HandleEvent(BeforeApplyDamageEvent E)
        {

            var Threshhold = 777;

            if (E.Damage.Amount > Threshhold)
            {
                E.Damage._Amount = Threshhold;
            }

            return base.HandleEvent(E);
        }
        
        // public override bool HandleEvent(BeforeDestroyObjectEvent E)
        // {
        //
        //     var Threshhold = 777;
        //     
        //     // if (E.Damage.Amount > Threshhold)
        //     // {
        //     //     E.Damage._Amount = Threshhold;
        //     // }
        //     
        //     return base.HandleEvent(E);
        // }
    }
}