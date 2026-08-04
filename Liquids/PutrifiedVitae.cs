using Qud.API;
using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Effects;
using static Wingytone.Colors.CustomColors;

namespace XRL.Liquids
{
    [Serializable]
    [IsLiquid]
    public class PutrifiedVitae : BaseLiquid
    {
        
        // todo; becoming infected by the amalgam can only be cured by having the player switch bodies with a shell or something. Yeah, this is what doll body shells are gonna come from.
        
        public new const string ID = "putrifiedvitae";

        public PutrifiedVitae() : base("putrifiedvitae")
        {
            FlameTemperature = 500;
            VaporTemperature = 1000;
            Temperature = 0;
            Fluidity = 10;
            Cleansing = 0;
            Weight = 0.1;
            InterruptAutowalk = true;
            ConsiderDangerousToContact = true;
            ConsiderDangerousToDrink = true;
        }

        [NonSerialized] public static List<string> Colors = new List<string>(2)
        {
            "a",
            "A"
        };

        public override List<string> GetColors()
        {
            return PutrifiedVitae.Colors;
        }

        public override string GetColor()
        {
            return "crimson";
        }

        public override string GetName(LiquidVolume Liquid)
        {
            return "{{fleshy|putrified vita}}";
        }

        public override string GetAdjective(LiquidVolume Liquid)
        {
            return "{{fleshy|fetid}}";
        }

        public override string GetWaterRitualName()
        {
            return "blood of the eaters";
        }

        public override string GetSmearedAdjective(LiquidVolume Liquid)
        {
            return "{{fleshy|fetid}}";
        }

        public override string GetSmearedName(LiquidVolume Liquid)
        {
            return "{{fleshy|fetid}}";
        }

        public override string GetStainedName(LiquidVolume Liquid)
        {
            return "{{fleshy|gore}}-stained";
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
            Message.Compound(
                "{{red|You feel the repugnant liquid cloy to your throat, your stomach wrenches horribly, rancid vitae ravenously binds with your flesh.}}");
            Damage value = new Damage((Liquid.Proportion("PutrifiedVitae") / 25 + 1 + "d100").Roll());
            Event @event = Event.New("TakeDamage");
            @event.SetParameter("Damage", value);
            @event.SetParameter("Owner", Liquid.ParentObject);
            @event.SetParameter("Attacker", Liquid.ParentObject);
            @event.SetParameter("Message", "from {{red|drinking putrified vitae}}!");
            @event.SetParameter("Phase", Target.GetPhase());
            Target.FireEvent(@event);
            Target.Bloodsplatter(true);
            Target.ApplyEffect(new AmalgamConversion());
            ExitInterface = true;
            return true;
        }

        public override void RenderBackgroundPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            eRender.ColorString = "^k" + eRender.ColorString;
        }

        public override void BaseRenderPrimary(LiquidVolume Liquid)
        {
            if (Liquid.IsWadingDepth())
            {
                Liquid.ParentObject.Render.RenderString = "~";
            }

            Liquid.ParentObject.Render.ColorString = "&a";
            Liquid.ParentObject.Render.DetailColor = "A";
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
                eRender.ColorString = "&a^k";
                return;
            }

            Render Render = Liquid.ParentObject.Render;
            int num = (XRLCore.CurrentFrame + Liquid.FrameOffset) % 60;
            if (Stat.RandomCosmetic(1, 600) == 1)
            {
                eRender.RenderString = "\u000f";
                eRender.ColorString = "&K";
            }
            if (Stat.RandomCosmetic(1, 60) == 1)
            {
                if (num < 15)
                {
                    Render.RenderString = "÷";
                    Render.ColorString = "&k^K";
                    Render.TileColor = "&a";
                    Render.DetailColor = "A";
                }
                else if (num < 30)
                {
                    Render.RenderString = "~";
                    Render.ColorString = "&k^K";
                    Render.TileColor = "&A";
                    Render.DetailColor = "a";
                }
                else if (num < 45)
                {
                    Render.RenderString = "\t";
                    Render.ColorString = "&k^K";
                    Render.TileColor = "&a";
                    Render.DetailColor = "k";
                }
                else if (num < 59)
                {
                    Render.RenderString = "\t";
                    Render.ColorString = "&k^K";
                    Render.TileColor = "&A";
                    Render.DetailColor = "K";

                }
                else
                {
                    Render.RenderString = "~";
                    Render.ColorString = "&k^K";
                    Render.TileColor = "&A";
                    Render.DetailColor = "a";
                }
            }
        }

        public override string GetPaintAtlas(LiquidVolume Liquid)
        {
            if (Liquid.IsWadingDepth())
            {
                return "Liquids/Splotchy/";
            }

            return base.GetPaintAtlas(Liquid);
        }
        
        public override void ObjectGoingProne(LiquidVolume Liquid, GameObject GO, bool UsePopups)
        {
            UsePopups = false;
            if (Liquid.IsWadingDepth())
            {
                if (GO.IsPlayer())
                {
                    BaseLiquid.AddPlayerMessage("{{R|Putrid vita spills into your mouth as you fall over.}}");
                }

                GO.Splatter("&w.");
                if (!GO.MakeSave("Toughness", 30, null, null, "Putrified Vitae"))
                {
                    GO.ApplyEffect(new AmalgamConversion());
                }
            }
        }
    }
}