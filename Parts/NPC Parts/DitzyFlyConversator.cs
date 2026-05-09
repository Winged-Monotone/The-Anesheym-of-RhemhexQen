using System;
using System.Collections.Generic;
using System.Threading;
using ConsoleLib.Console;
using UnityEngine;
using XRL.Core;
using XRL.Language;
using XRL.Rules;
using XRL.UI;
using XRL.World.Conversations.Parts;

namespace XRL.World.Parts
{
    [Serializable]
    public class DitzyFlyConversator : IPart
    {
        public GameObject Owner;

        public bool PassedInitialConversation = false;

        public DitzyFlyConversator()
        {
        }


        public override bool WantEvent(int ID, int cascade)
        {
            return ID == BeginConversationEvent.ID
                   || ID == AfterConversationEvent.ID
                   || base.WantEvent(ID, cascade);
        }


        public override bool HandleEvent(BeginConversationEvent E)
        {
            SoundManager.PlayMusic("DitzySong", Crossfade: true);

            return true;
        }

        public override bool HandleEvent(AfterConversationEvent E)
        {
            SoundManager.PlayMusic("melodyforcity", Crossfade: true);

            return true;
        }
    }
}