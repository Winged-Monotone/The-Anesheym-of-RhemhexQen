// XRL.World.Parts.AloePorta

using System;
using System.Collections.Generic;
using HistoryKit;
using XRL.Rules;
using XRL.World;

namespace XRL.World.Parts
{
    [Serializable]
    public class GraspingAloeCrystna : IActivePart
    {
        public int Cooldown = 200;
        public bool CooldownOnToggle = true;

        public GraspingAloeCrystna()
        {
            WorksOnCellContents = false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("EndTurn");
            eventRegistrar.Register("ObjectEnteredCell");
            eventRegistrar.Register("ObjectCreated");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                var AdjacentCellContents = ParentObject.CurrentCell.GetAdjacentCells(2);

                foreach (var C in AdjacentCellContents)
                {
                    if (C.HasCombatObject() && CooldownOnToggle == false)
                    {
                        ParentObject.Brain.PickLine(2, AllowVis.OnlyVisible, Attacker: C.GetCombatTarget());
                        var target = C.GetCombatTarget();
                        target.DirectMoveTo(ParentObject.CurrentCell);
                        CooldownOnToggle = true;
                    }

                    if (Cooldown >= 0 && CooldownOnToggle == true)
                    {
                        Cooldown--;
                    }

                    if (Cooldown <= 0 && CooldownOnToggle == true)
                    {
                        Cooldown = 100;
                        CooldownOnToggle = false;
                    }
                }
            }

            return base.FireEvent(E);
        }
    }
}