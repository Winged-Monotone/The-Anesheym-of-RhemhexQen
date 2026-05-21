using System;
using XRL.Core;

namespace XRL.World.Parts
{
    [Serializable]
    public class ArshiraCompanionActivator : IPart
    {
        
        
        public static void ArshJoin()
        {
            GameObject player = XRLCore.Core.Game.Player.Body;
            Zone zone = XRLCore.Core.Game.ZoneManager.ActiveZone;
            GameObject arsh = zone.FindObject("Arshiranandurah");

            arsh.Brain.BecomeCompanionOf(player);
            XRLCore.Core.Game.SetBooleanGameState("ArshiraCompanion", true);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return WantEvent(ID, cascade)
                   || ID == EnterCellEvent.ID
                   || ID == AfterConversationEvent.ID
                   || ID == BeforeDieEvent.ID;
        }
        public override bool HandleEvent(BeforeDieEvent E)
        {
            if (The.Game.GetStringGameState("GameMode") == "Classic")
            {
                if (E.Dying == ParentObject)
                {
                    
                }
            }
            if (The.Game.GetStringGameState("GameMode") == "Roleplay")
            {
                if (E.Dying == ParentObject)
                {
                    
                }
            }

            if (The.Game.GetStringGameState("GameMode") == "Wander")
            {
                if (E.Dying == ParentObject)
                {
                    
                }
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AfterConversationEvent E)
        {
            if (E.Conversation.ID == "JoinMe")
            {
                if (!ParentObject.InSamePartyAs(The.Game.Player.Body))
                    ArshJoin();
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnterCellEvent E)
        {
            GameObject player = XRLCore.Core.Game.Player.Body;
            Zone zone = XRLCore.Core.Game.ZoneManager.ActiveZone;
            var LuliFound =
                ParentObject.CurrentCell.AnyLocalAdjacentCell(cell => cell.HasObjectWithBlueprint("Lulihart"));
            if (LuliFound)
            {
                ParentObject.ParticleText("...");
            }

            return true;
        }
    }
}