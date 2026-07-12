using UnityEngine.UIElements;
using XRL.World;
using XRL;
using XRL.UI.ObjectFinderClassifiers;
using XRL.Wish;
using XRL.World.Effects;

namespace XRL.World.Parts
{
    public class NoGodModeForU : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == EndTurnEvent.ID;
        }
        
        public override bool HandleEvent(EndTurnEvent E)
        {

            if (ParentObject.IsHostileTowards(ThePlayer) && The.Core.IDKFA)
            {
                The.Core.IDKFA = false;
                ThePlayer.ApplyEffect(new Shamed(120));
                "The encoded being snaps =subject.possessive= fingers and revokes your invulnerability.".StartReplace()
                    .AddObject(ParentObject).EmitMessage(UsePopup: true);
            }
            
            return base.HandleEvent(E);
        }
    }
}