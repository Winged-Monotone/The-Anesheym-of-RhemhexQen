using XRL;
using XRL.UI.ObjectFinderClassifiers;
using XRL.Wish;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;

namespace Wingytone.Wishes
{
    [HasWishCommand]
    public static class WingyWishes
    {
        [WishCommand("wingygotols")]
        public static void GoToLongSigh()
        {
            The.Player.DirectMoveTo(The.ZoneManager.GetZone("JoppaWorld.33.3.1.1.29845651").GetPassableCells(The.Player)
                .GetRandomElement());
            The.Core.IDKFA = true;
            The.Core.Calm = true;
            
            var muts = The.Player.GetPart<Mutations>();
            XRL.UI.Popup.Suppress = true;
            muts.AddMutation("LightManipulation", 10);
            XRL.UI.Popup.Suppress = false;
        }
        
        [WishCommand("wingygotoremfd")]
        public static void GoToRhemFrontDoor()
        {
            The.Player.DirectMoveTo(The.ZoneManager.GetZone("JoppaWorld.33.3.1.1.10").GetPassableCells(The.Player)
                .GetRandomElement());
            The.Core.IDKFA = true;
            The.Core.Calm = true;
        }
        
        // [WishCommand("wingygotoremfd")]
        // public static void ProgressLSQuest()
        // {
        //
        // }
        
    }
}