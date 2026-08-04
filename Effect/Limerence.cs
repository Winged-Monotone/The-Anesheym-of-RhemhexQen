using System;
using System.Collections.Generic;
using Qud.API;
using XRL.Core;
using XRL.Rules;
using XRL.UI;

namespace XRL.World.Effects
{
	[Serializable]
	public class Limerence : Effect, ITierInitialized
	{
		private int Penalty = 3;

		private int SpeedPenalty = 5;

		private int SecondDuration;

		public GameObject Beauty;

		public GameObject PreviousLeader;

		public Limerence()
		{
			DisplayName = "{{lovesickness|limerence}}";
		}

		public Limerence(int Duration)
			: this()
		{
			base.Duration = Duration;
		}

		public Limerence(int Duration, GameObject Beauty)
			: this(Duration)
		{
			this.Beauty = Beauty;
		}

		public void Initialize(int Tier)
		{
			Duration = Stat.Random(3000, 3600);
		}

		public override int GetEffectType()
		{
			return 100663298;
		}

		public override bool SameAs(Effect e)
		{
			return false;
		}

		public override bool UseStandardDurationCountdown()
		{
			return true;
		}

		public override bool UseThawEventToUpdateDuration()
		{
			return true;
		}

		public override string GetDetails()
		{
			string text = "-3 Intelligence\n-3 Willpower\n-5 Move Speed\n You will occasionally move towards your object of obsession.";
			
			return text;
		}

		public override bool Apply(GameObject Object)
		{
			if (Object.HasEffect<Limerence>())
			{
				return false;
			}

			if (!Object.IsOrganic)
			{
				return false;
			}

			if (!Object.FireEvent(Event.New("ApplyLimerence", "Duration", Duration)))
			{
				return false;
			}

			if (Beauty != Object)
			{
				Object.StopFight(Beauty, Involuntary: false, Beauty.IsPlayer());
			}

			Object?.PlayWorldSound("Sounds/StatusEffects/sfx_statusEffect_charm");

			DidXToY("are", "obsessed with", Beauty, null, "!");
			ApplyStats();
			
			return true;
		}

		public override void Remove(GameObject Object)
		{
			GameObject.Validate(ref PreviousLeader);
			if (Object.Brain != null && PreviousLeader != Object.Brain.PartyLeader &&
			    Object.FireEvent(Event.New("CanRestorePartyLeader", "PreviousLeader", PreviousLeader)))
			{
				GameObject partyLeader = Object.PartyLeader;
				if (partyLeader != null && partyLeader.FireEvent(Event.New("CanCompanionRestorePartyLeader",
					    "Companion", Object, "PreviousLeader", PreviousLeader)))
				{
					Object.PartyLeader = PreviousLeader;
				}
			}

			UnapplyStats();
		}

		private void ApplyStats()
		{
			base.StatShifter.SetStatShift(base.Object, "Intelligence", -Penalty);
			base.StatShifter.SetStatShift(base.Object, "Willpower", -Penalty);
			base.StatShifter.SetStatShift(base.Object, "MoveSpeed", SpeedPenalty);
		}

		private void UnapplyStats()
		{
			base.StatShifter.RemoveStatShifts(base.Object);
		}

		public override bool WantEvent(int ID, int cascade)
		{
			if (!base.WantEvent(ID, cascade))
			{
				return ID == SingletonEvent<EndTurnEvent>.ID;
			}

			return true;
		}

		public override bool HandleEvent(EndTurnEvent E)
		{
			GameObject.Validate(ref PreviousLeader);

			var target = Object;

			var Distance = Beauty.DistanceTo(target.GetCurrentCell());

			if (Distance > 1)
			{
				if (target.MakeSave("Willpower", 16, Vs: "forced movement"))
					target.Push(target.GetDirectionToward(Beauty), 9999, 1);
			}
			return base.HandleEvent(E);
		}

		public override bool Render(RenderEvent E)
		{
			if (Duration > 0)
			{
				int num = XRLCore.CurrentFrame % 60;
				if (num > 5 && num < 10)
				{
					E.Tile = null;
					E.RenderString = "\u0003";
					if (SecondDuration > 0)
					{
						E.ColorString = "&A";
					}
					else
					{
						E.ColorString = "&A";
					}
				}
			}

			return true;
		}
	}
}
