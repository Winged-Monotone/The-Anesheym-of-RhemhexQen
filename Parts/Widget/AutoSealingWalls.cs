using XRL.Rules;
using XRL.World.Effects;

namespace XRL.World.Parts
{
    public class AutoSealingWalls : IPart
    {
        
        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == ZoneBuiltEvent .ID;
            }

            return true;
        }


        public override bool HandleEvent(ZoneBuiltEvent  E)
        {
            foreach (var Object in E.Zone.IterateObjects())
            {
                if (Object.IsWall())
                {
                    Object.AddPart<WallListensForDestruction>();
                    Object.ForceApplyEffect(new Omniphase(Effect.DURATION_INDEFINITE, "Widget", false));
                }
            }
        return base.HandleEvent(E);
        }
    }
}