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
    public class wmSulfurBurst : IPart
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
                Physics.ApplyExplosion(ParentObject.CurrentCell, 150);
                ParentObject.CurrentCell.TemperatureChange(25);
                ParentObject.CurrentCell.Splash("*");
                foreach (Cell C in ParentObject.CurrentCell.GetAdjacentCells())
                {
                    C.AddObject("SulfuricGas50");
                    C.AddObject("SulfurPuddle");
                }
            }

            return base.HandleEvent(E);
        }
    }
}