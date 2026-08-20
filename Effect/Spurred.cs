using System.Text;
using XRL.Core;

namespace XRL.World.Effects
{
    public class Spurred : Effect
    {
        public int Bonus = 4;
        public int SpeedBonusPercent = 10;
        public int SpeedBonus;
        
        public Spurred() => DisplayName = "{{g|spurred}}";
        
        public Spurred(int Duration)
            : this()
        {
            this.Duration = Duration;
        }
        
        public override int GetEffectType()
        {
            return TYPE_MENTAL | TYPE_MINOR | TYPE_REMOVABLE;
        }
        
        public override bool UseStandardDurationCountdown() => true;
        
        public override string GetDetails()
        {
            StringBuilder stringBuilder = Event.NewStringBuilder();
            stringBuilder.Append(Bonus).Append(" DV\n").Append(Bonus).Append(" to-hit\n").Append(Bonus).Append(" Willpower\n").Append(Bonus).Append(" Ego\n");
            if (Object != null)
            {
                if (SpeedBonus != 0)
                    stringBuilder.Append(SpeedBonus).Append(" Quickness");
            }
            else
                stringBuilder.Append(SpeedBonusPercent).Append("% Quickness");
            return stringBuilder.ToString();
        }
        
        public override bool Apply(GameObject Object)
        {
            ApplyStats();
            return true;
        }
        
        public override void Remove(GameObject Object)
        {
            UnapplyStats();
            base.Remove(Object);
        }
        
        private void ApplyStats()
        {
            SpeedBonus = Object.Stat("Speed") * SpeedBonusPercent / 100;
            StatShifter.SetStatShift("DV", Bonus);
            StatShifter.SetStatShift("Ego", Bonus);
            StatShifter.SetStatShift("Willpower", Bonus);
            StatShifter.SetStatShift("Speed", SpeedBonus);
        }
        
        private void UnapplyStats() => this.StatShifter.RemoveStatShifts();
        
        public override bool Render(RenderEvent E)
        {
            if (this.Duration > 0)
            {
                int num = XRLCore.CurrentFrame % 60;
                if (num > 25 && num < 40)
                {
                    E.Tile = (string) null;
                    E.RenderString = "*";
                    E.ColorString = "&g";
                }
            }
            return true;
        }
    }
}