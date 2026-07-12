// Decompiled with JetBrains decompiler
// Type: XRL.World.Parts.Mutation.Interdiction
// Assembly: Assembly-CSharp, Version=2.0.211.50, Culture=neutral, PublicKeyToken=null
// MVID: CB67EBEE-0C5F-432E-A24D-5C369451F89C
// Assembly location: H:\SteamLibrary\steamapps\common\Caves of Qud\CoQ_Data\Managed\Assembly-CSharp.dll
// XML documentation location: H:\SteamLibrary\steamapps\common\Caves of Qud\CoQ_Data\Managed\Assembly-CSharp.xml

using ConsoleLib.Console;
using System;
using System.Collections.Generic;
using XRL.World.Effects;

#nullable disable
namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class EatersInterdiction : BaseMutation
    {
        public GameObject interdictTarget;
        public int SpeedPenalty = 5;
        [NonSerialized] private long lastms;
        [NonSerialized] private long glowstep;

        public EatersInterdiction() => this.Type = "Mental";

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("BeforeTakeAction");
            base.Register(Object, Registrar);
        }

        public void StopEatersInterdiction()
        {
            if (this.interdictTarget == null)
                return;
            this.interdictTarget.RemoveEffect(
                (Effect)this.interdictTarget.GetEffect<Interdicted>(
                    (Predicate<Interdicted>)(fx => this.ParentObject.IDMatch(fx.interdictorId))));
            this.interdictTarget = (GameObject)null;
        }

        public void BeginEatersInterdiction(GameObject target)
        {
            this.StopEatersInterdiction();
            if (this.ParentObject.IsEMPed() || !target.PhaseMatches(3) || !this.ParentObject.HasLOSTo(target))
                return;
            target.ApplyEffect((Effect)new Interdicted(this.ParentObject.ID, this.SpeedPenalty));
            var targetinter = target.GetEffect<Interdicted>();

            targetinter.DisplayName = "cryoburned";
            
            this.interdictTarget = target;
            this.DidXToY("lock", "onto", target, ColorAsBadFor: target);
        }

        public void CheckEatersInterdiction()
        {
            if (this.interdictTarget == null)
                return;
            if (this.ParentObject.IsEMPed())
                this.StopEatersInterdiction();
            if (!this.interdictTarget.PhaseMatches(3))
                this.StopEatersInterdiction();
            if (this.ParentObject.HasLOSTo(this.interdictTarget))
                return;
            this.StopEatersInterdiction();
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeforeTakeAction")
            {
                this.CheckEatersInterdiction();
                if (this.ParentObject.Target != null && (this.interdictTarget != this.ParentObject.Target ||
                                                         !this.interdictTarget.HasEffect<Interdicted>(
                                                             (Predicate<Interdicted>)(FX =>
                                                                 FX.interdictorId == this.ParentObject.ID))))
                    this.BeginEatersInterdiction(this.ParentObject.Target);
                if (ParentObject.Target != null && !ParentObject.Target.MakeSave("Toughness",12, ParentObject, null, null))
                    ParentObject.Target.TakeDamage("1d5".Roll(), "takes %t from cryogenic dissipation.", "Cold");
            }
            return true;
        }

        public override bool Render(RenderEvent E)
        {
            if (this.interdictTarget != null)
                E.WantsToPaint = true;
            return true;
        }

        public override void OnPaint(ScreenBuffer buffer)
        {
            if (this.interdictTarget == null || !GameObject.Validate(this.ParentObject))
                return;
            List<Tuple<Cell, char>> lineTo = this.ParentObject.GetLineTo(this.interdictTarget);
            if (lineTo == null || lineTo.Count <= 0)
                return;
            if (this.lastms == 0L)
                this.lastms = IComponent<GameObject>.frameTimerMS;
            else if (IComponent<GameObject>.frameTimerMS - this.lastms > 200L)
                ++this.glowstep;
            if (this.glowstep >= (long)lineTo.Count)
                this.glowstep = 0L;
            for (int index = 0; index < lineTo.Count; ++index)
            {
                char ch = 'c';
                if ((long)index == this.glowstep)
                    ch = 'C';
                Tuple<Cell, char> tuple = lineTo[index];
                buffer.Goto(tuple.Item1.X, tuple.Item1.Y);
                buffer.Buffer[tuple.Item1.X, tuple.Item1.Y].SetBackground(ch);
                buffer.Buffer[tuple.Item1.X, tuple.Item1.Y].Detail = ColorUtility.ColorMap[ch];
            }
        }

        public override bool ChangeLevel(int NewLevel) => base.ChangeLevel(NewLevel);

        public override bool Mutate(GameObject GO, int Level) => base.Mutate(GO, Level);

        public override bool Unmutate(GameObject GO) => base.Unmutate(GO);
    }
}