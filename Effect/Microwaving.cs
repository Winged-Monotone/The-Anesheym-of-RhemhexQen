namespace XRL.World.Effects
{
    public class Microwaving : Effect
    {

        public int Stacks = 0;
        
        public Microwaving()
        {
            base.DisplayName = "{{red|thermalizing}}";
            Duration = 1;
        }
        
        public override string GetDetails()
        {
            return
                "Microwaves are cooking you alive.";
        }
        
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {

            if (!Object.MakeSave("Toughness", 8 + Stacks))
            {
                Stacks += 1;
                Object.TakeDamage(1 * Stacks, "from %t {{f|fire}}", "Fire", "You were cooked to death.");
            }
            else
            {
                --Stacks;
                --Duration;
            }
            return base.HandleEvent(E);
        }
        
    }
}