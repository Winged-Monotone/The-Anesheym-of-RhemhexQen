using XRL.Core;
using XRL.Rules;
using XRL.UI;

namespace XRL.World.Effects
{
    public class FalseHologram : Effect
    {
        public static readonly int ICON_COLOR_PRIORITY;

        public int FlickerFrame;

        public int FrameOffset;

        public FalseHologram()
        {
            Duration = 1;
        }

        public override bool Render(RenderEvent E)
        {
            string text = null;
            int num = (XRLCore.CurrentFrame + FrameOffset) % 200;
            if (FlickerFrame > 0 || Stat.RandomCosmetic(1, 200) == 1)
            {
                E.Tile = null;
                if (FlickerFrame == 0)
                {
                    E.RenderString = "_";
                }
                else if (FlickerFrame == 1)
                {
                    E.RenderString = "-";
                }
                else if (FlickerFrame == 2)
                {
                    E.RenderString = "|";
                }

                if (num < 8)
                {
                    text = "&C";
                }
                else
                {
                    text = "&Y";
                }

                if (FlickerFrame == 0)
                {
                    FlickerFrame = 3;
                }

                FlickerFrame--;
            }
            text = ((num < 4) ? "&C" : ((num < 8) ? "&b" : ((num >= 12) ? "&B" : "&c")));
            if (!Options.DisableTextAnimationEffects)
            {
                FrameOffset += Stat.Random(0, 20);
            }
            if (FlickerFrame == 0 && Stat.RandomCosmetic(1, 400) == 1)
            {
                text = "&Y";
            }
            else
            {
                text = "&K";
            }
            if (!text.IsNullOrEmpty())
            {
                E.ApplyColors(text, ICON_COLOR_PRIORITY);
            }

            return base.Render(E);
        }
    }
}