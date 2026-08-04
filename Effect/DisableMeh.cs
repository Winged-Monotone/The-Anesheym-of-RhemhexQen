using XRL.Rules;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    public class DisableMeh : Effect
    {
        public int Roll = Stat.Random(1, 3);

        public DisableMeh()
        {
            Duration = Stat.Random(7, 14);

            if (Roll == 1)
            {
                DisplayName = "{{yellow|dancing}}";
            }

            if (Roll == 2)
            {
                DisplayName = "{{red|laughing}}";
            }

            if (Roll == 3)
            {
                DisplayName = "weeping";
            }
        }

        public override void Applied(GameObject Object)
        {
            if (Roll == 1)
            {
                AddPlayerMessage("{{yellow|" + Object.DisplayName + " is doing a strange dance while playing " + Object.Its + " flute ...}}");;
            }

            if (Roll == 2)
            {
                if (Stat.Random(1, 100) <= 5)
                {
                    Object.PlayWorldSound("mehlaughter" + Stat.Random(1, 2) + ".wav", 50.0f, 0.4f, true);
                }
                AddPlayerMessage("{{red|" + Object.DisplayName + " is laughing at you ...}}");
            }

            if (Roll == 3)
            {
                AddPlayerMessage("{{yellow|" + Object.DisplayName + " is quietly weeping ...}}");
            }

            base.Applied(Object);
        }

        public override void Remove(GameObject Object)
        {
            var EncoPart = Object.GetPart<TheEncodedOne>();

            EncoPart.CastAbilityCooldown = Stat.Random(14, 28);

            base.Remove(Object);
        }

        public override string GetDetails()
        {
            if (Roll == 1)
            {
                return ("{{yellow|" + Object.DisplayName + " is doing a strange dance while playing " + Object.Its + " flute ...}}");
            }

            if (Roll == 2)
            {
                return ("{{red|" + Object.DisplayName + " is laughing at you ...}}");
            }

            if (Roll == 3)
            {
                return ("{{yellow|" + Object.DisplayName + " is quietly weeping ...}}");
            }

            return base.GetDescription();
        }

        public override bool UseStandardDurationCountdown()
        {
            return true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return ID == BeginTakeActionEvent.ID
                   || base.WantEvent(ID, cascade);
        }

        public override bool HandleEvent(BeginTakeActionEvent E)
        {
            if (E.Object == Object)
            {
                return false;
            }

            return base.HandleEvent(E);
        }
    }
}