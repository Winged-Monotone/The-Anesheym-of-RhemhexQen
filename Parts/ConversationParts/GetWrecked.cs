// XRL.World.Conversations.Parts.StartFight
using XRL;
using XRL.World;
using XRL.World.AI;
using XRL.World.Conversations;

namespace XRL.World.Conversations.Parts
{
    public class GetWrecked : IConversationPart
    {

        public override bool WantEvent(int ID, int Propagation)
        {
            if (!base.WantEvent(ID, Propagation) && ID != GetChoiceTagEvent.ID)
            {
                return ID == EnteredElementEvent.ID;
            }
            return true;
        }

        public override bool HandleEvent(EnteredElementEvent E)
        {
            var Dir = The.Speaker.GetDirectionToward(The.Listener);
            var Level = The.Speaker.Level;
            The.Listener.Push(Dir, Level * 1000, 4);
            CombatJuice.punch(The.Speaker, The.Listener);
            
            
            return base.HandleEvent(E);
        }
    }
}