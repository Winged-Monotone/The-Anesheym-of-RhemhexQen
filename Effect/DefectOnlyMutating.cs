using Qud.API;
using System;
using XRL;
using XRL.Language;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using System.Collections.Generic;
using ConsoleLib.Console;
using XRL.Core;

namespace XRL.World.Effects
{
    [Serializable]
    public class DefectOnlyMutating : Effect
    {
        private string Population;

        public bool triggered;

        public int PermuteMutationsAt;

        public bool MutationsPermuted;
        public int DurationSplatter = 30;

        public DefectOnlyMutating()
        {
            base.DisplayName = "{{M|violently mutating}}";
        }

        public DefectOnlyMutating(int Duration, string Population = "MutatingResults")
            : this()
        {
            base.Duration = Duration;
            this.Population = Population;
            PermuteMutationsAt = Duration / 2;
        }

        public DefectOnlyMutating(int Duration, int PermuteMutationsAt, string Population = "MutatingResults")
            : this(Duration, Population)
        {
            this.PermuteMutationsAt = PermuteMutationsAt;
        }

        public override int GetEffectType()
        {
            return 4;
        }

        public override bool SameAs(Effect e)
        {
            DefectOnlyMutating mutating = e as DefectOnlyMutating;
            if (mutating.Population != Population)
            {
                return false;
            }

            if (mutating.triggered != triggered)
            {
                return false;
            }

            if (mutating.PermuteMutationsAt != PermuteMutationsAt)
            {
                return false;
            }

            if (mutating.MutationsPermuted != MutationsPermuted)
            {
                return false;
            }

            return base.SameAs(e);
        }

        public override string GetDetails()
        {
            return "Your flesh is violently mutating.";
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.HasEffect("DefectOnlyMutating"))
            {
                return false;
            }

            if (Object.FireEvent("ApplyDefectOnlyMutating"))
            {
                if (Object.IsPlayer())
                {
                    IComponent<GameObject>.AddPlayerMessage("Your flesh begins to ripple.", 'R');
                }

                return true;
            }

            return false;
        }

        public bool IsDivisible(int Divisor, int Remainder)
        {
            return Remainder >= Divisor && Divisor % Remainder == 0;
        }

        public override void Remove(GameObject Object)
        {
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("EndTurn");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn" && Duration > 0)
            {
                Duration--;
                if (Duration < PermuteMutationsAt && !MutationsPermuted)
                {
                    base.Object.PermuteRandomMutationBuys();
                    if (base.Object.IsPlayer())
                    {
                        IComponent<GameObject>.AddPlayerMessage("You writhe in agony as your flesh festers.", 'R');
                    }
                }

                if (Duration <= 0 && !triggered)
                {
                    triggered = true;
                    string blueprint = PopulationManager.RollOneFrom(Population).Blueprint;
                    if (blueprint == "Mutation")
                    {
                        MutationEntry mutationEntry = MutationsAPI.FindRandomMutationFor(base.Object,
                            (MutationEntry e) => e.Category != null && !e.Category.Name.Contains("Defect"));
                        if (mutationEntry != null)
                        {
                            if (base.Object.IsPlayer())
                            {
                                Popup.Show("Your flesh calms itself.");
                            }
                        }
                    }
                    else if (blueprint == "Defect")
                    {
                        MutationEntry mutationEntry2 = MutationsAPI.FindRandomMutationFor(base.Object,
                            (MutationEntry e) => e.Category != null && e.Category.Name.Contains("Defect"));
                        if (mutationEntry2 != null)
                        {
                            if (base.Object.IsPlayer())
                            {
                                Popup.Show("You skin violently mutates and you gain a new defect:\n\n&W" +
                                           mutationEntry2.GetDisplayName());
                            }
                        }
                    }
                    else if (blueprint.StartsWith("Points:"))
                    {
                        int num = Stat.Roll(blueprint.Split(':')[1]);
                        if (base.Object.IsPlayer())
                            if (base.Object.IsPlayer())
                            {
                                Popup.Show("Your flesh calms itself.");
                            }
                    }

                    if (DurationSplatter >= 0)
                    {
                        --DurationSplatter;
                        if (IsDivisible(DurationSplatter, 3))
                        {
                            base.Object.BloodsplatterBurst(true, 1.0f, 6);
                            List<Cell> adjacentCells1 = Object.CurrentCell.GetAdjacentCells(true);
                            adjacentCells1.Add(Object.CurrentCell);
                            foreach (Cell cell in adjacentCells1)
                            {
                                if (!cell.IsOccluding() && Stat.Random(1, 100) <= 75)
                                {
                                    GameObject AmalgamContainer = GameObject.Create("Amalgam1");
                                    cell.AddObject(AmalgamContainer);
                                }
                            }
                        }
                    }
                }
            }

            return base.FireEvent(E);
        }
    }
}