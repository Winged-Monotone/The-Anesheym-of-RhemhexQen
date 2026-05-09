using System;
using System.Collections.Generic;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.Rules;
using XRL.World.Effects;
using XRL.World.Parts.Skill;
using HistoryKit;


namespace XRL.World.Parts
{
    [Serializable]
    public class wmDitzySinging : IPart
    {
        public int DitzyRestVoice = Stat.Random(20, 200);
        public int DitzyRestVoiceTest = 1;

        public bool DitzyIsSinging = false;

        public int NumberofNotes = Stat.Random(6, 12);


        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            if (DitzyIsSinging == false && DitzyRestVoice > 0)
            {
                --DitzyRestVoice;
            }
            else if (DitzyIsSinging == false && DitzyRestVoice == 0)
            {
                DitzyIsSinging = true;
            }
            else if (DitzyIsSinging == true)
            {
                if (NumberofNotes > 0)
                {
                    var NotePause = Stat.Random(1, 2);

                    if (NotePause > 1)
                    {
                        ParentObject.PlayWorldSound("ditzyahh", Volume: 1.30f, PitchVariance: 0.15f, Pitch: 1.5f);

                        var NoteType = Stat.Random(1, 2);
                        if (NoteType == 1)
                        {
                            ParentObject.ParticleText("{{G|" + "\x0D" + "}}", IgnoreVisibility: true);
                        }
                        else
                        {
                            ParentObject.ParticleText("{{G|" + "\x0E" + "}}", IgnoreVisibility: true);
                        }

                        --NumberofNotes;

                        if (NumberofNotes == 0)
                        {
                            DitzyIsSinging = false;
                            DitzyRestVoice = Stat.Random(20, 200);
                            NumberofNotes = Stat.Random(6, 12);
                        }
                    }
                }
            }

            return base.HandleEvent(E);
        }
    }
}