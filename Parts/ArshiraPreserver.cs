using XRL.World;
using XRL.World.Effects;

namespace XRL.World.Parts
{
    public class ArshiraPreserver : IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                
                || ID == BeforeDieEvent.ID;
        }

        public override bool HandleEvent(BeforeDieEvent E)
        {
            if (The.Game.gameMode == "Roleplay" && E.Dying == ParentObject)
            {
                ParentObject.ApplyEffect(new wmIncapacitated(Duration: 50, 20));
                ParentObject.hitpoints = 20;
                
                return false;
            }
            return base.HandleEvent(E);
        }
    }
}