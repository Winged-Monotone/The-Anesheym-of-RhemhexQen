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

namespace XRL.Liquids
{
    [Serializable]
    [IsLiquid]
    public class LiquidSulfer : BaseLiquid
    {
        public new const string ID = "liquidsulfur";

        [NonSerialized] public static List<string> Colors = new List<string>(3) { "O", "o", "W" };

        public LiquidSulfer()
            : base("liquidsulfur")
        {
            FlameTemperature = 800;
            VaporTemperature = 1600;
            Temperature = 360;
            Fluidity = 15;
            Weight = 0.3;
            Combustibility = 25;
            ThermalConductivity = 1000;
            InterruptAutowalk = true;
            ConsiderDangerousToContact = true;
            ConsiderDangerousToDrink = true;
            Glows = true;
            FreezeObject1 = "SmallBoulder";
            FreezeObjectThreshold1 = 1;
            FreezeObjectVerb1 = "solidify";
            FreezeObject2 = "MediumBoulder";
            FreezeObjectThreshold2 = 100;
            FreezeObjectVerb2 = "solidify";
            FreezeObject3 = "Shale";
            FreezeObjectThreshold3 = 300;
            FreezeObjectVerb3 = "solidify";
        }

        public override List<string> GetColors()
        {
            return Colors;
        }

        public override string GetColor()
        {
            return "O";
        }

        public override string GetName(LiquidVolume Liquid)
        {
            return "{{sulfuric|liquid sulfur}}";
        }

        public override string GetAdjective(LiquidVolume Liquid)
        {
            return "{{sulfuric|sulfuric}}";
        }

        public override string GetWaterRitualName()
        {
            return "brimstone milk";
        }

        public override string GetSmearedAdjective(LiquidVolume Liquid)
        {
            return "{{sulfuric|sulfuric}}";
        }

        public override string GetSmearedName(LiquidVolume Liquid)
        {
            return "{{sulfuric|sulfur-smudged}}";
        }

        public override string GetStainedName(LiquidVolume Liquid)
        {
            return "{{sulfuric|sulfurous}}";
        }

        public override string GetPreparedCookingIngredient()
        {
            return "heatMinor";
        }

        public override bool SafeContainer(GameObject GO)
        {
            if (GO.Physics != null)
            {
                return GO.Physics.FlameTemperature > Temperature;
            }

            return true;
        }

        public override float GetValuePerDram()
        {
            return 50f;
        }

        public override bool Vaporized(LiquidVolume Liquid, GameObject gameObject)
        {
            return false;
        }

        public override bool Drank(LiquidVolume Liquid, int Volume, GameObject Target, StringBuilder Message,
            ref bool ExitInterface)
        {
            JournalAPI.AddAccomplishment("Inexplicably, you drank liquid sulfur.",
                "For reasons kept secret by the sphinx of subjective experience, =name= drank the forbidden tea.",
                "While traveling through " + The.Player.CurrentZone.GetTerrainDisplayName() +
                ", =name= stopped at a tavern in " + JournalAPI.GetLandmarkNearestPlayer().Text + ". There " +
                The.Player.GetPronounProvider().Subjective +
                " lost a bet and bashfully drank a dram of lava. Miraculously, " +
                The.Player.GetPronounProvider().Subjective + " left the tavern unharmed.", null, "general",
                MuralCategory.BodyExperienceNeutral, MuralWeight.Medium, null, -1L);
            Message.Compound("{{sulfuric|IT BURNS!}}");
            Target.TemperatureChange(Temperature, Target);
            string dice = Liquid.Proportion("sulfuric") / 100 + 1 + "d100";
            Target.TakeDamage(dice.Roll(), "from {{sulfuric|drinking sulfur}}!", "Heat", null, null, Target,
                Liquid.ParentObject);
            ExitInterface = true;
            return true;
        }

        public override void BeforeRender(LiquidVolume Liquid)
        {
            if ((!Liquid.Sealed || Liquid.LiquidVisibleWhenSealed) && (Liquid.Primary == "sulfuric" ||
                                                                       Liquid.Secondary == "sulfuric" ||
                                                                       Liquid.IsPureLiquid("sulfuric")))
            {
                Liquid.AddLight(50);
            }
        }

        public override void RenderBackgroundPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            if (eRender.ColorsVisible)
            {
                eRender.ColorString = "^o" + eRender.ColorString;
            }
        }

        public override void BaseRenderPrimary(LiquidVolume Liquid)
        {
            Liquid.ParentObject.Render.ColorString = "&W^o";
            Liquid.ParentObject.Render.TileColor = "&W";
            Liquid.ParentObject.Render.DetailColor = "o";
        }

        public override void BaseRenderSecondary(LiquidVolume Liquid)
        {
            Liquid.ParentObject.Render.ColorString += "&o";
        }

        public override void RenderPrimary(LiquidVolume Liquid, RenderEvent eRender)
        {
            var LiquidTemp = Liquid.ParentObject.Temperature;

            if (!Liquid.IsWadingDepth())
            {
                return;
            }

            if (Liquid.ParentObject.IsFrozen())
            {
                eRender.RenderString = "~";
                eRender.TileVariantColors("&W^w", "&W", "w");
                return;
            }

            Render render = Liquid.ParentObject.Render;
            int num = (XRLCore.CurrentFrame + Liquid.FrameOffset) % 60;
            if (Stat.RandomCosmetic(1, 600) == 1)
            {
                eRender.RenderString = "\u000f";
                eRender.TileVariantColors("&W^o", "&W", "o");
            }

            if (Stat.RandomCosmetic(1, 60) == 1)
            {
                if (num < 15)
                {
                    render.RenderString = "÷";
                    render.ColorString = "&W^o";
                    render.TileColor = "&W";
                    render.DetailColor = "O";
                }
                else if (num < 30)
                {
                    render.RenderString = "~";
                    render.ColorString = "&W^O";
                    render.TileColor = "&W";
                    render.DetailColor = "o";
                }
                else if (num < 45)
                {
                    render.RenderString = "\t";
                    render.ColorString = "&W^o";
                    render.TileColor = "&W";
                    render.DetailColor = "O";
                }
                else
                {
                    render.RenderString = "~";
                    render.ColorString = "&W^o";
                    render.TileColor = "&W";
                    render.DetailColor = "o";
                }
            }
            else if (LiquidTemp >= 370)
            {
                if (num < 15)
                {
                    eRender.RenderString = "÷";
                    eRender.ColorString = "&C^b";
                    eRender.DetailColor = "b";
                }
                else if (num < 30)
                {
                    eRender.RenderString = "~";
                    eRender.ColorString = "&c^b";
                    eRender.DetailColor = "b";
                }
                else if (num < 45)
                {
                    eRender.RenderString = "\t";
                    eRender.ColorString = "&w^b";
                    eRender.DetailColor = "B";
                }
                else
                {
                    eRender.RenderString = "~";
                    eRender.ColorString = "&C^b";
                    eRender.DetailColor = "b";
                }
            }
        }

        public override void RenderSecondary(LiquidVolume Liquid, RenderEvent eRender)
        {
            if (eRender.ColorsVisible)
            {
                eRender.ColorString += "&O";
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

        public override int GetNavigationWeight(LiquidVolume Liquid, GameObject GO, bool Smart, bool Slimewalking,
            bool FilthAffinity, ref bool Uncacheable)
        {
            if (Smart && GO != null)
            {
                Uncacheable = true;
                int num = GO.Stat("HeatResistance");
                if (num > 0 && ((!Liquid.IsSwimmingDepth())
                        ? (!HasFlammableEquipmentEvent.Check(GO, Temperature))
                        : (!HasFlammableEquipmentOrInventoryEvent.Check(GO, Temperature))))
                {
                    if (num >= 100)
                    {
                        return 0;
                    }

                    return Math.Min(Math.Max(40 + 59 * (100 - num) / 100, 0), 99);
                }
            }

            return 99;
        }

        public override void StainElements(LiquidVolume Liquid, GetItemElementsEvent E)
        {
            E.Add("might", 1);
        }
    }
}