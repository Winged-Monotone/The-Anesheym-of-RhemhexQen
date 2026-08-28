using XRL.Rules;

namespace XRL.World.Effects
{
    public class AmalgamConversion : Effect
    {
        public int CycleDuration = 1500;
        public int InfectionPhases = 4;

        public bool SkinSlink = false;

        public AmalgamConversion()
        {
            base.DisplayName = "{{Red|vitamosis}}";
            Duration = 1;
        }
        
        public AmalgamConversion(bool skinSlink) : this()
        {
            this.SkinSlink = skinSlink;
        }

        public override string GetDetails()
        {
            if (SkinSlink)
            {
                return
                    "A skin slinker has slithered under your skin and is releasing enzymes, slowly transforming you into an amalgam.";
            }

            return
                "Putrified vitae has compromised your molecular structure, slowly transforming you into an amalgam.";
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.TryGetEffect(out AmalgamConversion fx))
            {
                if (!SkinSlink && fx.SkinSlink) fx.SkinSlink = false; // non-slink variant harder to remove, prefer that
                return false;
            }

            return base.Apply(Object);
        }

        public bool CheckRemove()
        {
            // end effect if no more slinks on body
            if (SkinSlink && !Object.HasBodyPart("Skin Slink"))
            {
                Object.RemoveEffect(this);
                return true;
            }

            return false;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == EndTurnEvent.ID
                   || ID == RegenerateDefaultEquipmentEvent.ID
                   ;
        }

        public override bool HandleEvent(RegenerateDefaultEquipmentEvent E)
        {
            CheckRemove();
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            if (CheckRemove())
            {
                return true;
            }

            if (Object == The.Player)
                --CycleDuration;
            else
            {
                CycleDuration -= 150;
            }

            if (CycleDuration == 0)
            {
                CycleDuration = 1500;
                --InfectionPhases;
                if (InfectionPhases <= 0)
                {
                    if (Object == The.Player)
                    {
                        The.Player.Die(Reason: "You succumb to the Crimson Tide.");
                    }
                    else
                    {
                        Object.ReplaceWith("Amalgam" + Stat.Random(1, 4));
                        XDidY(Object, "flesh unravels as it transforms", "into an amalgam", SubjectPossessedBy: Object);
                    }
                }
            }

            return base.HandleEvent(E);
        }
    }
}