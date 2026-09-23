using System;
using System.Collections.Generic;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.Rules;
using XRL.World.Effects;
using XRL.World.Parts.Skill;


namespace XRL.World.Parts
{
    [Serializable]
    public class wmPusBurst : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == OnDeathRemovalEvent.ID;
        }

        public override bool HandleEvent(OnDeathRemovalEvent E)
        {
            if (E.Dying == ParentObject && !E.Dying.IsFrozen())
            {
                LiquidRelatedMethods.wmLiquidBurst(ParentObject, "viscouspuss-1000", true, true, 150, 25, "*");
            }

            return base.HandleEvent(E);
        }
    }
}