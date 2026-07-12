using XRL.Rules;

namespace XRL.World.Effects
{
    public class ExplodingIceCrystal : Effect
    {
        public int PrimeDuration = Stat.Random(2, 3);

        public int DetonateDuration = Stat.Random(1, 7);

        public bool Primed = false;

        public ExplodingIceCrystal()
        {
            DisplayName = "Unstable";
            Duration = 1;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            if (PrimeDuration >= 1)
                PrimeDuration -= 1;

            if (PrimeDuration == 0 && !Primed)
            {
                var PrimeChance = Stat.Random(1, 4);
                PrimeDuration -= 1;

                if (PrimeChance == 1)
                {
                    Primed = true;
                    Object.TileParticleBlip(Object.Render.Tile, "&b", "B", 3 );
                    Object.Render.DetailColor = "b";
                    Object.Render.TileColor = "&B";
                }
                else
                {
                    Object.TileParticleBlip(Object.Render.Tile, "&b", "B", 3 );
                    Object.Obliterate();
                }
            }
            else if (Primed)
            {
                --DetonateDuration;
                if (DetonateDuration <= 0)
                {
                    Object.ShatterSplatter();
                    var AdjCell = Object.CurrentCell.GetAdjacentCells();

                    foreach (var C in AdjCell)
                    {
                        var target = C.GetCombatTarget();
                        target?.TakeDamage("8d8".Roll(), Attributes: "Cold", Message: "from %t ice-shards.");
                    }

                    CombatJuice.playPrefabAnimation(Object.CurrentCell.Location, "Impacts/ImpactVFXNeutronImpact",
                        async: true);
                    Object.Obliterate();
                }
            }

            return base.HandleEvent(E);
        }
    }
}