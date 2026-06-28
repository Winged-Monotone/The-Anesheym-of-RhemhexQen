using System.Collections.Generic;
using ConsoleLib.Console;
using XRL.UI;
using XRL.World.Parts;

namespace XRL.World.Conversations.Parts
{
    public class wm_GiveEggT1 : IConversationPart
    {
        public static bool IsEgg(GameObject Object)
        {
            if (!Object.HasPropertyOrTag("EggHuntItemT1"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public override bool WantEvent(int ID, int Propagation)
        {
            return base.WantEvent(ID, Propagation)
                   || ID == GetChoiceTagEvent.ID
                   || ID == EnterElementEvent.ID
                ;
        }

        public override bool HandleEvent(GetChoiceTagEvent E)
        {
            E.Tag = "{{g|[give {{yellow|strange}} eggs]}}";
            return false;
        }

        public override bool HandleEvent(EnterElementEvent E)
        {
            var objects = The.Player.Inventory.GetObjects(IsEgg);
            if (objects.Count == 0)
            {
                Popup.Show("You have no eggs to give.");
                return false;
            }

            var artifact = Popup.PickGameObject("Choose an egg to give.", objects, AllowEscape: true);
            if (artifact == null) return false;
            artifact.SplitStack(1, OwningObject: The.Player);

            if (!The.Player.FireEvent(Event.New("CommandRemoveObject", "Object", artifact)))
            {
                Popup.Show("You can't give that object.");
                return false;
            }

            return base.HandleEvent(E);
        }
    }
}