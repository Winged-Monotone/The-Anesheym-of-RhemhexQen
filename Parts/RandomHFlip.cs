using XRL.UI;
using XRL.World.Parts;
using XRL.Rules;

namespace XRL.World.Parts
{
    public class RandomHFlip : IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            var chance = Stat.Random(1, 2);

            if (chance == 1)
            {
                ParentObject.Render.HFlip = true;
            }
            
            return base.HandleEvent(E);
        }
    }
}