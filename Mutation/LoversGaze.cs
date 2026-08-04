using System;
using System.Collections.Generic;
using System.Threading;
using ConsoleLib.Console;
using XRL.Language;
using XRL.Rules;
using XRL.World.Effects;

namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class LoversGaze : IDelayedLineMutation
    {
        [NonSerialized] private static GameObject _Projectile;

        public override string Command => "CommandLoversGaze";

        public override bool CanRefract => false;

        public override GameObject Projectile
        {
            get
            {
                if (!GameObject.Validate(ref _Projectile))
                {
                    _Projectile = GameObject.CreateUnmodified("ProjectileLoversGaze");
                }

                return _Projectile;
            }
        }

        public override void CollectStats(Templates.StatCollector stats, int Level)
        {
            stats.Set("Range", GetRange(Level), !stats.mode.Contains("ability"));
            stats.CollectCooldownTurns(MyActivatedAbility(ActivatedAbilityID), GetCooldown(Level));
        }
        

        public override void FireLine(List<Cell> Path)
        {
            PlayWorldSound("burn_blast", 1f, 0f, Combat: true);
            ScreenBuffer scrapBuffer = ScreenBuffer.GetScrapBuffer1();
            List<GameObject> list = Event.NewGameObjectList();
            bool flag = false;
            int i = 0;
            for (int count = Path.Count; i < count; i++)
            {
                Cell cell = Path[i];
                if (cell.IsBlackedOut())
                {
                    break;
                }

                foreach (GameObject @object in cell.Objects)
                {
                    if (IsValidTarget(@object) && !@object.MakeSave("Willpower", 20, null, null,
                            "Mehrishir Lewd Gaze Beam"))
                    {
                        list.Add(@object);
                    }
                }

                int num = 0;
                while (num < list.Count)
                {
                    GameObject target = list[num];
                    
                    if (target.IsPlayer())
                    {

                    }

                    if (!target.HasEffect<Limerence>())
                    {
                        target.ApplyEffect(new Limerence(){Beauty = ParentObject, Duration = 14});
                    }

                    list.RemoveAt(0);
                }

                if (cell.IsVisible())
                {
                    scrapBuffer.RenderBase();
                    if (i > 0)
                    {
                        scrapBuffer.WriteAt(Path[i - 1], "&b*");
                    }

                    if (i > 1)
                    {
                        scrapBuffer.WriteAt(Path[i - 2], "&K*");
                    }

                    scrapBuffer.WriteAt(cell, "&B*");
                    scrapBuffer.Draw();
                    Thread.Sleep(10);
                }
            }
        }

        public override string GetDescription()
        {
            return "You seduce things with your gaze.";
        }

        public override int GetRange(int Level)
        {
            return 7 + Level;
        }

        public override int GetCooldown(int Level)
        {
            return 50;
        }

        public override int GetDelay(int Level)
        {
            return 1;
        }

        public override string GetLevelText(int Level)
        {
            return "You can gaze {{rules|" + GetRange(Level) + "}} squares after a " +
                   Grammar.Cardinal(GetDelay(Level)) + "-turn warmup and seduce a target.\nCooldown: {{rules|" +
                   GetCooldown(Level) + "}} rounds";
        }
    }
}