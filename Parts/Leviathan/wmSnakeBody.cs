using System;
using System.Collections.Generic;
using System.Linq;
using Battlehub.UIControls;
using ConsoleLib.Console;
using XRL;
using XRL.UI.ObjectFinderClassifiers;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;


namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class wmSnakeBody : IPart
    {
        public GameObject Owner;
        public GameObject Item;


        public int Segments = 24;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == OnDeathRemovalEvent.ID;
        }

        public override bool AllowStaticRegistration() => true;

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            if (Owner.IsValid())
            {
                Registrar.Register(Owner, EnteringCellEvent.ID);
            }
        }

        public void SetOwner(GameObject obj)
        {
            Owner = obj;
            obj.RegisterEvent(this, EnteringCellEvent.ID);
        }

        public override bool HandleEvent(EnteringCellEvent E)
        {
            var exit = Owner.CurrentCell;
            if (exit == null) return true;

            var enter = E.Cell;
            if (enter == null || enter == ParentObject.CurrentCell) return true;
            if (enter.IsAdjacentTo(ParentObject.CurrentCell)) return true;

            ParentObject.SystemMoveTo(exit, 0, true);
            return true;
        }
    }
}