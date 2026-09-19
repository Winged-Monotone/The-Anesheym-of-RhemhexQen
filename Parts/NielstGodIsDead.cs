using System.Linq;
using XRL.World;

namespace XRL.World.Parts
{
    public class NielstGodIsDead :IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
            || ID == BeginTakeActionEvent.ID
            || ID == GetDisplayNameEvent.ID
            || ID == ZoneActivatedEvent.ID;
        }

        public bool NielstIsDead()
        {
            return The.Game.HasIntGameState("wmNielstSlain");
        }
        
        public override bool HandleEvent(ZoneActivatedEvent E)
        {
            if (!NielstIsDead())
            {
                return true;
            }

            ParentObject.Render.TileColor = "&K";
            ParentObject.Render.DetailColor = "y";

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeginTakeActionEvent E)
        {
            if (!NielstIsDead())
            {
                return true;
            }
            
            return false;
        }
        
        public override bool HandleEvent(GetDisplayNameEvent E)
        {
            if (!NielstIsDead())
            {
                return true;
            }

            if (E.Object == ParentObject)
            {
                E.AddAdjective("{{pink|dormant}}");
            }
            
            return base.HandleEvent(E);
        }
    }
}