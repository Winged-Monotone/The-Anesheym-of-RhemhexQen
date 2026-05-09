using System;
using System.Collections.Generic;
using System.Text;
using XRL;
using XRL.Core;
using XRL.Liquids;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Effects;


namespace XRL.Liquids
{
    [Serializable]
    [IsLiquid]
    public class AnesheymPheromones : BaseLiquid
    {
        public AnesheymPheromones() : base("insectpheromones")
        {
            FlameTemperature = 0;
            VaporTemperature = 0;
            Temperature = 0;
            Fluidity = 50;
            Cleansing = 1;
            Weight = 0.1;
            InterruptAutowalk = false;
            ConsiderDangerousToContact = false;
            ConsiderDangerousToDrink = false;
        }

        [NonSerialized] public static List<string> Colors = new List<string>(3)
        {
            "m",
            "M",
            "y"
        };

        public override List<string> GetColors()
        {
            return Colors;
        }

        public override string GetColor()
        {
            return "M";
        }

        public override string GetName(LiquidVolume Liquid)
        {
            return "{{purple|insect pheromones}}";
        }

        public override string GetAdjective(LiquidVolume Liquid)
        {
            return "{{purple|smelly}}";
        }

        public override string GetWaterRitualName()
        {
            return "insect pheromones";
        }

        public override string GetSmearedAdjective(LiquidVolume Liquid)
        {
            return "{{purple|pheromonous}}";
        }

        public override string GetSmearedName(LiquidVolume Liquid)
        {
            return "{{purple|poisoned}}";
        }

        public override string GetStainedName(LiquidVolume Liquid)
        {
            return "{{purple|pheromone}}-stained";
        }

        public override float GetValuePerDram()
        {
            return 10f;
        }

        // public override bool Vaporized(LiquidVolume Liquid)
        // {
        //     return false;
        // }
        public override bool Drank(LiquidVolume Liquid, int Volume, GameObject Target, StringBuilder Message,
            ref bool ExitInterface)
        {
            Message.Compound("{{purple| It tastes like silkworm innards ...}}");
            Damage value = new Damage((Liquid.Proportion("PoisonIchor") / 100 + 1 + "d100").Roll());
            Event @event = Event.New("ReputationChange");
            @event.SetParameter("Owner", Liquid.ParentObject);
            @event.SetParameter("Attacker", Liquid.ParentObject);
            @event.SetParameter("Message", "from {{purple|drinking poison ichor}}!");
            @event.SetParameter("Phase", Target.GetPhase());
            Target.FireEvent(@event);
            ExitInterface = true;
            return true;
        }

        public override void RenderBackgroundPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            eRender.ColorString = "^M" + eRender.ColorString;
        }

        public override void BaseRenderPrimary(LiquidVolume Liquid)
        {
            if (Liquid.IsWadingDepth())
            {
                Liquid.ParentObject.Render.RenderString = "~";
            }

            Liquid.ParentObject.Render.ColorString = "&M^m";
        }

        public override void RenderPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            if (!Liquid.IsWadingDepth())
            {
                return;
            }

            if (Liquid.ParentObject.IsFrozen())
            {
                eRender.RenderString = "~";
                eRender.ColorString = "&M^m";
                return;
            }

            Render pRender = Liquid.ParentObject.Render;
            int num = (XRLCore.CurrentFrame + Liquid.FrameOffset) % 60;
            if (Stat.RandomCosmetic(1, 600) == 1)
            {
                eRender.RenderString = "\u000f";
                eRender.ColorString = "&M^m";
            }

            if (Stat.RandomCosmetic(1, 60) == 1)
            {
                if (num < 15)
                {
                    pRender.RenderString = "÷";
                    pRender.ColorString = "&M^m";
                }
                else if (num < 30)
                {
                    pRender.RenderString = "~";
                    pRender.ColorString = "&M^m";
                }
                else if (num < 45)
                {
                    pRender.RenderString = "\t";
                    pRender.ColorString = "&M^m";
                }
                else
                {
                    pRender.RenderString = "~";
                    pRender.ColorString = "&M^m";
                }
            }
        }

        public override void ObjectGoingProne(LiquidVolume Liquid, GameObject GO, bool UsePopups)
        {
            if (Liquid.IsWadingDepth())
            {
                if (GO.IsPlayer())
                {
                    BaseLiquid.AddPlayerMessage(
                        "{{R|Poisonous ichor splashes into your mouth. You feel your nervious system shutters..}}");
                }

                GO.Splatter("&w.");
                if (!GO.MakeSave("Toughness", 30, null, null, "Goo Poison"))
                {
                    GO.ApplyEffect(new Poisoned(Stat.Roll("1d4+4"), Stat.Roll("1d2+2") + "d2", 10));
                }
            }
        }
    }
}