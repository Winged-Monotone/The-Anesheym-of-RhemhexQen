using Qud.API;
using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Effects;

namespace XRL.Liquids
{
    [Serializable]
    [IsLiquid]
    public class ViscousPuss : BaseLiquid
    {
        public new const string ID = "viscouspuss";

        public ViscousPuss() : base("viscouspuss")
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

        [NonSerialized] public static List<string> Colors = new List<string>(3)
        {
            "Y",
            "w",
            "W"
        };

        public override List<string> GetColors()
        {
            return PutrifiedVitae.Colors;
        }

        public override string GetColor()
        {
            return "W";
        }

        public override string GetName(LiquidVolume Liquid)
        {
            return "viscous {{yellow|pus}}";
        }

        public override string GetAdjective(LiquidVolume Liquid)
        {
            return "{{white|pustulating}}";
        }

        public override string GetWaterRitualName()
        {
            return "pus";
        }

        public override string GetSmearedAdjective(LiquidVolume Liquid)
        {
            return "{{white|pus-encrusted}}";
        }

        public override string GetSmearedName(LiquidVolume Liquid)
        {
            return "{{white|pus-encrusted}}";
        }

        public override string GetStainedName(LiquidVolume Liquid)
        {
            return "{{white|gore}}-stained";
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
                "{{red|Bitty, foaming and coarse with a bit of chunk, your throat swells immediately and you feel the bacterium of the ancients assault your molecules.}}");
            Target.Bloodsplatter(true);
            Target.ApplyEffect(new Witkrunk());
            
            ExitInterface = true;
            return true;
        }

        public override void RenderBackgroundPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            eRender.ColorString = "^Y" + eRender.ColorString;
        }

        public override void BaseRenderPrimary(LiquidVolume Liquid)
        {
            if (Liquid.IsWadingDepth())
            {
                Liquid.ParentObject.Render.RenderString = "~";
            }

            Liquid.ParentObject.Render.ColorString = "&Y^k";
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
                eRender.ColorString = "&Y^w";
                return;
            }

            Render Render = Liquid.ParentObject.Render;
            int num = (XRLCore.CurrentFrame + Liquid.FrameOffset) % 60;
            if (Stat.RandomCosmetic(1, 600) == 1)
            {
                eRender.RenderString = "\u000f";
                eRender.ColorString = "&y";
                eRender.DetailColor = "Y";
            }

            if (Stat.RandomCosmetic(1, 60) == 1)
            {
                if (num < 15)
                {
                    Render.RenderString = "÷";
                    Render.ColorString = "&y";
                    Render.DetailColor = "Y";
                }
                else if (num < 30)
                {
                    Render.RenderString = "~";
                    Render.ColorString = "&y";
                    Render.DetailColor = "Y";
                }
                else if (num < 45)
                {
                    Render.RenderString = "\t";
                    Render.ColorString = "&y";
                    Render.DetailColor = "Y";
                }
                else if (num < 59)
                {
                    Render.RenderString = "\t";
                    Render.ColorString = "&y";
                    Render.DetailColor = "Y";
                }
                else
                {
                    Render.RenderString = "~";
                    Render.ColorString = "&y";
                    Render.DetailColor = "Y";
                }
            }
        }

        public override string GetPaintAtlas(LiquidVolume Liquid)
        {
            if (Liquid.IsWadingDepth())
            {
                return "Liquids/Gunk/";
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
                    BaseLiquid.AddPlayerMessage("{{R|Pus spills into your mouth as you fall over.}}");
                }

                GO.Splatter("&w.");
                if (!GO.MakeSave("Toughness", 30, null, null, "ViscousPuss"))
                {
                    GO.ApplyEffect(new Witkrunk());
                }
            }
        }
    }
}