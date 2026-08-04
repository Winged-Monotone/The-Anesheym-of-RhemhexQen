using XRL.Rules;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    public class ChargingSuperAttack : Effect
    {
        public int radius;

        public override bool UseStandardDurationCountdown()
        {
            return true;
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.HasEffect<ChargingSuperAttack>())
            {
                return false;
            }
            
            return base.Apply(Object);
        }

        public override void Applied(GameObject Object)
        { 
            var EOhook = Object.GetPart<TheEncodedOne>();

            Object.Brain.Mobile = false;
            EOhook.WarningCells.Clear();
            foreach (var C in EOhook.GetRing(radius))
            {
                EOhook.WarningCells.Add(C.Location);
            }
            base.Applied(Object);
        }

        public override void Remove(GameObject Object)
        {
            var EOhook = Object.GetPart<TheEncodedOne>();

            var Violence = Stat.Random(1, 7);
            var randomradius = Stat.Random(1, 7);
            
                EOhook.SuperDuperUltraAttack(radius);
                EOhook.SuperDuperUltraAttack(radius + 2);
                EOhook.SuperDuperUltraAttack(radius + 3);
                
            Object.Brain.Mobile = true;
            base.Remove(Object);
        }
    }
}