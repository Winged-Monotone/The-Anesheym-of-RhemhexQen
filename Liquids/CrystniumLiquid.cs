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
using Qud.API;
using XRL.UI.ObjectFinderClassifiers;
using AiUnity.NLog.Core.Targets;


namespace XRL.Liquids
{
    [Serializable]
    [IsLiquid]
    public class LiquidCrystnium : BaseLiquid
    {
        public new const string ID = "crystnium";

        [NonSerialized] public static List<string> Colors = new List<string>(4) { "B", "y", "Y", "b" };

        public LiquidCrystnium()
            : base("crystnium")
        {
            Combustibility = -50;
            VaporObject = "SteamGas";
            Fluidity = 30;
            Evaporativity = 2;
            Cleansing = 5;
            PureElectricalConductivity = 0;
            MixedElectricalConductivity = 100;
            EnableCleaning = true;
            SlipperyWhenFrozen = true;
            SlipperySaveTargetBase = 5;
            SlipperySaveTargetScale = 0.3;
            SlipperySaveVs = "Ice Slip Move";
            SlipperyMessage = "{{C|=subject.T= =verb:slip= on the ice!}}";
            SlipperyParticle = "&C\u001a";
        }

        public override List<string> GetColors()
        {
            return Colors;
        }

        public override string GetColor()
        {
            return "B";
        }

        public override string GetName(LiquidVolume Liquid)
        {
            if (!The.Player.HasPart<DrankCrystnium>())
                return "{{B|fresh water}}";
            else
            {
                return "{{B|crystnium}}";
            }
        }

        public override string GetAdjective(LiquidVolume Liquid)
        {
            if (Liquid == null)
            {
                return "{{Y|crystening}}";
            }

            return null;
        }

        public override string GetWaterRitualName()
        {
            return "water";
        }

        public override string GetSmearedAdjective(LiquidVolume Liquid)
        {
            if (Liquid.IsMixed())
            {
                if (Liquid.Proportion("salt", "blood") > Liquid.Proportion("water"))
                {
                    return null;
                }

                if (Liquid.Primary != "oil" && Liquid.Primary != "lava" && Liquid.Primary != "wax")
                {
                    return "{{Y|dilute}}";
                }
            }

            return "{{B|wet}}";
        }

        public override string GetSmearedName(LiquidVolume Liquid)
        {
            return "{{B|wet}}";
        }

        public override string GetStainedName(LiquidVolume Liquid)
        {
            return "{{B|water}}";
        }

        public override bool Drank(LiquidVolume Liquid, int Volume, GameObject Target, StringBuilder Message,
            ref bool ExitInterface)
        {
            if (!Target.HasPart<DrankCrystnium>())
            {
                Target.AddPart<DrankCrystnium>();
            }

            try
            {
                Message?.Compound("Your throat begins to crystallize! You feel like your belly is full of stones!");

                if (!Target.HasEffect<CrystenedFleshEffect>())
                {
                    Target.ApplyEffect(new CrystenedFleshEffect(100, null));
                }
                else if (Target.HasEffect<CrystniumGasEffect>())
                {
                    var Effect = Target.GetEffect<CrystniumGasEffect>();
                    Effect.Severity += 50;
                }
            }
            finally
            {
            }

            return true;
        }

        public override void SmearOn(LiquidVolume Liquid, GameObject Target, GameObject By, bool FromCell)
        {
        }

        public override void RenderBackgroundPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            if (eRender.ColorsVisible)
            {
                eRender.ColorString = "^b" + eRender.ColorString;
            }
        }

        public override void BaseRenderPrimary(LiquidVolume Liquid)
        {
            Liquid.ParentObject.Render.ColorString = "&b^B";
            Liquid.ParentObject.Render.TileColor = "&b";
            Liquid.ParentObject.Render.DetailColor = "B";
        }

        public override void BaseRenderSecondary(LiquidVolume Liquid)
        {
            Liquid.ParentObject.Render.ColorString += "&b";
        }

        public override void RenderPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            Render render2 = Liquid.ParentObject.Render;
            int num3 = (XRLCore.CurrentFrame + Liquid.FrameOffset) % 60;
            if (Stat.RandomCosmetic(1, 600) == 1)
            {
                eRender.RenderString = "~";
                eRender.TileVariantColors("&Y^b", "&Y", "b");
            }

            if (Stat.RandomCosmetic(1, 60) == 1)
            {
                if (num3 < 15)
                {
                    render2.RenderString = "÷";
                    render2.ColorString = "&B^b";
                    render2.TileColor = "&B";
                    render2.DetailColor = "b";
                }
                else if (num3 < 30)
                {
                    render2.RenderString = "~";
                    render2.ColorString = "&B^b";
                    render2.TileColor = "&B";
                    render2.DetailColor = "b";
                }
                else if (num3 < 45)
                {
                    render2.RenderString = " ";
                    render2.ColorString = "&B^b";
                    render2.TileColor = "&B";
                    render2.DetailColor = "b";
                }
                else
                {
                    render2.RenderString = "~";
                    render2.ColorString = "&B^b";
                    render2.TileColor = "&B";
                    render2.DetailColor = "b";
                }

                Render render = Liquid.ParentObject.Render;
                int num = (XRLCore.CurrentFrame + Liquid.FrameOffset) % 60;
                if (Stat.RandomCosmetic(1, 600) == 1)
                {
                    eRender.RenderString = "÷";
                    eRender.TileVariantColors("&Y^B", "&Y", "B");
                }

                if (num < 15)
                {
                    render.RenderString = "÷";
                    render.ColorString = "&b^B";
                    render.TileColor = "&b";
                    render.DetailColor = "B";
                }
                else if (num < 30)
                {
                    render.RenderString = " ";
                    render.ColorString = "&Y^B";
                    render.TileColor = "&Y";
                    render.DetailColor = "B";
                }
                else if (num < 45)
                {
                    render.RenderString = "÷";
                    render.ColorString = "&b^B";
                    render.TileColor = "&b";
                    render.DetailColor = "B";
                }
                else
                {
                    render.RenderString = "~";
                    render.ColorString = "&y^B";
                    render.TileColor = "&y";
                    render.DetailColor = "B";
                }
            }
        }

        public override void RenderSecondary(LiquidVolume Liquid, RenderEvent eRender)
        {
            if (eRender.ColorsVisible)
            {
                eRender.ColorString += "&b";
            }
        }

        public override float GetValuePerDram()
        {
            return 0.000000001f;
        }

        public override float GetPureLiquidValueMultipler()
        {
            return 0.0000000001f;
        }
    }
}