using XRL;
using XRL.Rules;
using XRL.UI.ObjectFinderClassifiers;
using XRL.Wish;
using XRL.World.Effects;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;
using XRL.World.Parts.Skill;
using XRL.World.Skills;

namespace Wingytone.Wishes
{
    [HasWishCommand]
    public static class WingyWishes
    {
        [WishCommand("wingygotols")]
        public static void GoToLongSigh()
        {
            The.Player.DirectMoveTo(The.ZoneManager.GetZone("JoppaWorld.33.3.1.1.29845651").GetPassableCells(The.Player)
                .GetRandomElement());
            The.Core.IDKFA = true;
            The.Core.Calm = true;
            
            var muts = The.Player.GetPart<Mutations>();
            XRL.UI.Popup.Suppress = true;
            muts.AddMutation("LightManipulation", 10);
            XRL.UI.Popup.Suppress = false;
        }
        
        [WishCommand("wingygotoremfd")]
        public static void GoToRhemFrontDoor()
        {
            The.Player.DirectMoveTo(The.ZoneManager.GetZone("JoppaWorld.33.3.1.1.10").GetPassableCells(The.Player)
                .GetRandomElement());
            The.Core.IDKFA = true;
            The.Core.Calm = true;
        }
        
        [WishCommand("wingygotomeh")]
        public static void GoToMeh()
        {
            The.Player.DirectMoveTo(The.ZoneManager.GetZone("JoppaWorld.33.3.1.1.29845658").GetPassableCells(The.Player)
                .GetRandomElement());
            The.Core.IDKFA = true;
            The.Core.Calm = true;
        }
        
        [WishCommand("mehphase")]
        public static void MehPhase(string phase)
        {
            var meh = The.ActiveZone.FindObject("EncodedEater");
            var mehHP = meh.GetStat("Hitpoints");
            
            if (phase == "2")
            {
                mehHP.Penalty = (int)(mehHP.BaseValue * (1 - 0.79));
            }
            if (phase == "3")
            {
                mehHP.Penalty = (int)(mehHP.BaseValue * (1 - 0.62));
            }
            if (phase == "4")
            {
                mehHP.Penalty = (int)(mehHP.BaseValue * (1 - 0.49));
            }
            if (phase == "5")
            {
                mehHP.Penalty = (int)(mehHP.BaseValue * (1 - 0.34));
            }
            if (phase == "6")
            {
                mehHP.Penalty = (int)(mehHP.BaseValue * (1 - 0.14));
            }
            if (phase == "7")
            {
                mehHP.Penalty = (int)(mehHP.BaseValue * (1 - 0.6));
            }
            
        }

        [WishCommand("mehdmg")]
        public static void MehPhaseChanger()
        {
            var meh = The.ActiveZone.FindObject("EncodedEater");
            var mehHP = meh.GetStat("Hitpoints");

            mehHP.Penalty += 777;
        }
        
        [WishCommand("mehdie")]
        public static void MehDies()
        {
            var meh = The.ActiveZone.FindObject("EncodedEater");
            meh.Die();
        }
        
        [WishCommand("meheffect")]
        public static void MehCame()
        {
            var meh = The.ActiveZone.FindObject("EncodedEater");

            meh.ApplyEffect(new ChargingSuperAttack(){Duration = 10, radius = Stat.Random(1,7)});
        }
        
        [WishCommand("wingyarmup")]
        public static void GimmeStuff()
        {
            XRL.UI.Popup.Suppress = true;
            The.Player.ReceiveObject("Zetachrome Apex");
            The.Player.ReceiveObject("Zetachrome Lune");
            The.Player.ReceiveObject("Zetachrome Pumps");
            The.Player.ReceiveObject("Zetachrome Gloves");
            
            The.Player.ReceiveObject("Kaleidocera Cape");
            The.Player.ReceiveObject("Kaleidocera Krakows");
            The.Player.ReceiveObject("Kaleidocera Muffs");
            
            The.Player.ReceiveObject("Long Sword8", 2);
            The.Player.ReceiveObject("Dagger8");

            The.Player.ReceiveObject("Force Bracelet");
            The.Player.ReceiveObject("VISAGE");
            
            The.Player.ReceiveObject("High-Energy Thermo Cask");
            The.Player.ReceiveObject("Point-Defense Drone");
            The.Player.ReceiveObject("Light-Obfuscating Lens");

            The.Player.ReceiveObject("Spaser Rifle");
            The.Player.ReceiveObject("V77 Channel Rifle");
            The.Player.ReceiveObject("Antimatter Cell", 20);

            The.Player.ReceiveObject("AcidGasGrenade3", 10);
            The.Player.ReceiveObject("FlashbangGrenade3", 10);
            The.Player.ReceiveObject("HeatGrenade3", 10);
            The.Player.ReceiveObject("ColdGrenade3", 10);
            The.Player.ReceiveObject("HEGrenade3",10);
            The.Player.ReceiveObject("NormalityGasGrenade3", 10);

            The.Player.ReceiveObject("SalveTonic", 20);
            The.Player.ReceiveObject("Urberry", 20);
            The.Player.ReceiveObject("Arsplice Seed", 10);

            The.Player.ReceiveObject("BlazeTonic", 10);
            The.Player.ReceiveObject("UbernostrumTonic", 5);

            The.Player.Body.GetBody().AddPart("Floating Nearby", Dynamic: true);

            foreach (var S in SkillFactory.Factory.SkillList)
            {
                The.Player.AddSkill(S.Value.Class);
                foreach (var P in S.Value.PowerList)
                {
                    The.Player.AddSkill(P.Class);
                }

            }

            The.Player.AwardXP(250000);
            
            Examiner.IDAll();
            
            XRL.UI.Popup.Suppress = false;
        }
    }
}