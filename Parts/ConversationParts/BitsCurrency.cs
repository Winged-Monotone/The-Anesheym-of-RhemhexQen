using System.Collections.Generic;
using XRL.UI;
using XRL.UI.ObjectFinderClassifiers;
using XRL.World.Parts;
using XRL.World.Tinkering;

namespace XRL.World.Conversations.Parts
{
    public class BitsCurrency : IConversationPart
    {
        public int SaileBitGoal
        {
            get => The.Game.GetIntGameState("SaileBitGoal");
            set => The.Game.SetIntGameState("SaileBitGoal", value);
        }
        
        public override bool WantEvent(int ID, int Propagation)
        {
            if (!base.WantEvent(ID, Propagation) && ID != GetChoiceTagEvent.ID)
            {
                return ID == EnterElementEvent.ID;
            }
            return true;
        }

        public override bool HandleEvent(GetChoiceTagEvent E)
        {
            E.Tag = "{{g|[give bits]}}";
            return false;
        }
        

        public override bool HandleEvent(EnterElementEvent E)
        {
            var Locker = The.Listener.RequirePart<BitLocker>();
            
            
            var MainOptions = new List<string>();
            var ListInt = new List<int>();
            var BitValues = new List<int>();
            
            foreach (var Bit in Locker.BitStorage)
            {
                if (Bit.Value > 0)
                {
                    var BitTier = BitType.GetBitTier(Bit.Key);
                    var BitValue = BitTier * 10;
                    
                    MainOptions.Add(BitType.BitMap[Bit.Key].Description);
                    ListInt.Add(Bit.Value);
                    BitValues.Add(BitValue);
                }
            }

            var result = Popup.PickSeveral(
                Title: "Select bits to give",
                Options: MainOptions,
                Stacks: ListInt,
                    AllowEscape: true);
                if (result.IsNullOrEmpty())
                {
                    // The user escaped out of the menu, or chose "Cancel"
                    return false;
                }

                foreach (var Bit in result)
                {
                    var Name = MainOptions[Bit.Selected][0];
                    var Value = BitValues[Bit.Selected];
                    SaileBitGoal += Value * Bit.Amount;
                    Locker.UseBits(Name, Bit.Amount);
                }

                if (SaileBitGoal >= 100)
                {
                    The.Game.CompleteQuest("GetBitsGetBitsGetBits");
                }
            return base.HandleEvent(E);
        }
    }
}