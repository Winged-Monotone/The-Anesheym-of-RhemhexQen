using System;
using System.Collections.Generic;
using System.Threading;
using ConsoleLib.Console;
using XRL.UI;

namespace XRL.World.Parts.Mutation
{
[Serializable]
public class Battoujutsu : BaseMutation
{
	public Battoujutsu()
	{
		base.Type = "Physical";
	}

	public override string GetDescription()
	{
		return "You deliver a blinding fast slash with your weapon, both moving and dealing damage as you dash at rapid speeds.";
	}

	public override void CollectStats(Templates.StatCollector stats, int Level)
	{
		stats.Set("Range", GetRange(Level));
		stats.Set("Damage", GetDamage(Level));
		stats.CollectCooldownTurns(MyActivatedAbility(ActivatedAbilityID), GetCooldownTurns(Level));
	}

	public override bool WantEvent(int ID, int cascade)
	{
		if (!base.WantEvent(ID, cascade))
		{
			return ID == AIGetOffensiveAbilityListEvent.ID;
		}
		
		return true;
	}

	public override bool HandleEvent(AIGetOffensiveAbilityListEvent E)
	{
		if (IsMyActivatedAbilityAIUsable(ActivatedAbilityID) && E.Distance <= GetRange(base.Level) - 1 && !E.Actor.OnWorldMap() && GameObject.Validate(E.Target))
		{
			Cell cell = E.Target.CurrentCell;
			Cell cell2 = E.Actor.CurrentCell;
			if (cell != null && cell2 != null && (cell.X == cell2.X || cell.Y == cell2.Y || Math.Abs(cell.X - cell2.X) == Math.Abs(cell.Y - cell2.Y)))
			{
				E.Add("CommandBattoujutsu");
			}
		}
		return base.HandleEvent(E);
	}

	public override void Register(GameObject Object, IEventRegistrar Registrar)
	{
		Registrar.Register("CommandBattoujutsu");
		base.Register(Object, Registrar);
	}

	public override string GetLevelText(int Level)
	{
		string text = "You dash in a direction, dealing damage to creatures you pass through.\n";
		text = text + "Range: " + GetRange(Level) + "\n";
		text = text + "Damage: " + GetDamage(Level) + "\n";
		int cooldownTurns = GetCooldownTurns(Level);
		return text + "Cooldown: " + cooldownTurns + " " + ((cooldownTurns == 1) ? "round" : "rounds") + "\n";
	}

	public int GetRange(int Level)
	{
		return 7 + Level / 2;
	}

	public virtual int GetCooldownTurns(int Level)
	{
		return 20;
	}

	public virtual string GetDamage(int Level)
	{
		return Level + "d8";
	}

	public override bool FireEvent(Event E)
	{
		if (E.ID == "CommandBattoujutsu")
		{
			if (ParentObject.OnWorldMap())
			{
				if (ParentObject.IsPlayer())
				{
					Popup.ShowFail("You cannot do that on the world map.");
				}
				return false;
			}
			Cell cell = PickDirection("Battoujutsu Dash");
			if (cell == null)
			{
				return false;
			}
			ParentObject?.PlayWorldSound("Sounds/Abilities/sfx_ability_mutation_physical_generic_activate");
			UseEnergy(1000, "Physical Mutation Battoujutsu");
			CooldownMyActivatedAbility(ActivatedAbilityID, GetCooldownTurns(base.Level));
			DidX("dash", "with lightning speed", null, null, null, ParentObject, null, UseFullNames: false, IndefiniteSubject: false, null, null, DescribeSubjectDirection: false, DescribeSubjectDirectionLate: true);
			if (ParentObject != null)
			{
				string directionFromCell = ParentObject.CurrentCell.GetDirectionFromCell(cell);
				List<Cell> list = new List<Cell>();
				List<Cell> list2 = new List<Cell>();
				Cell targetCell = null;
				Cell cellFromDirection = ParentObject.Physics.CurrentCell;
				int num = 0;
				int i = 0;
				for (int range = GetRange(base.Level); i < range; i++)
				{
					cellFromDirection = cellFromDirection.GetCellFromDirection(directionFromCell);
					list2.Add(cellFromDirection);
					if (cellFromDirection == null)
					{
						break;
					}
					if (cellFromDirection.IsEmpty())
					{
						targetCell = cellFromDirection;
						num = i;
						list.AddRange(list2);
						list2.Clear();
					}
				}
				if (num > 0)
				{
					if (ParentObject.InActiveZone())
					{
						ScreenBuffer scrapBuffer = ScreenBuffer.GetScrapBuffer1();
						for (int j = 0; j <= num + 5; j++)
						{
							scrapBuffer.RenderBase();
							for (int num2 = j; num2 > j - 5; num2--)
							{
								if (num2 > 0 && num2 < list.Count)
								{
									string text = null;
									if (num2 - j == 0)
									{
										text = "&K/";
									}
									if (num2 - j == -1)
									{
										text = "&r/";
									}
									if (num2 - j == -2)
									{
										text = "&R¢";
									}
									if (num2 - j == -3)
									{
										text = "&r/";
									}
									if (num2 - j == -4)
									{
										text = "&K/";
									}
									if (text != null)
									{
										scrapBuffer.Goto(list[num2].X, list[num2].Y);
										scrapBuffer.Write(text);
									}
								}
							}
							scrapBuffer.Draw();
							Thread.Sleep(10);
						}
					}
					string damage = GetDamage(base.Level);
					foreach (Cell item in list)
					{
						foreach (GameObject objectsViaEvent in item.GetObjectsViaEventList())
						{
							if (objectsViaEvent != ParentObject)
							{
								objectsViaEvent.TakeDamage(damage.RollCached(), "from %t passage!", "Crushing", null, null, null, ParentObject, null, null, null, Accidental: false, Environmental: false, Indirect: false, ShowUninvolved: true);
							}
						}
					}
					ParentObject.DirectMoveTo(targetCell);
				}
			}
		}
		return base.FireEvent(E);
	}

	public override bool Mutate(GameObject GO, int Level)
	{
		ActivatedAbilityID = AddMyActivatedAbility("Battou!", "CommandBattoujutsu", "Physical Mutations", GetDescription(), "~");
		return base.Mutate(GO, Level);
	}

	public override bool Unmutate(GameObject GO)
	{
		RemoveMyActivatedAbility(ref ActivatedAbilityID);
		return base.Unmutate(GO);
	}
}
}