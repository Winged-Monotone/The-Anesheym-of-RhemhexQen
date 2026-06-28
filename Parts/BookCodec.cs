// XRL.World.Parts.Book
using System;
using Qud.API;
using XRL;
using XRL.UI;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
[Serializable]
public class BookCodec : IPart
{
	public const string DEFAULT_OPEN_SOUND = "Sounds/codecring";

	public const string DEFAULT_PAGE_SOUND = "Sounds/codecringturn";

	public string ID = "";

	public override bool SameAs(IPart p)
	{
		if ((p as Book).ID != ID)
		{
			return false;
		}
		return base.SameAs(p);
	}

	public override bool WantEvent(int ID, int cascade)
	{
		if (!base.WantEvent(ID, cascade) && ID != GetInventoryActionsEvent.ID && ID != PooledEvent<HasBeenReadEvent>.ID)
		{
			return ID == InventoryActionEvent.ID;
		}
		return true;
	}

	public override bool HandleEvent(HasBeenReadEvent E)
	{
		if (E.Actor == The.Player && GetHasBeenRead())
		{
			return false;
		}
		return base.HandleEvent(E);
	}

	public override bool HandleEvent(GetInventoryActionsEvent E)
	{
		E.AddAction("Read", "read", "Read", null, 'r', FireOnActor: false, GetHasBeenRead() ? 15 : 100);
		return base.HandleEvent(E);
	}

	public override bool HandleEvent(InventoryActionEvent E)
	{
		if (E.Command == "Read" && E.Actor.IsPlayer())
		{
			BookUI.ShowBookByID(ID, DEFAULT_OPEN_SOUND);
			AfterReadBookEvent.Send(E.Actor, ParentObject, this, E);
			if (!GetHasBeenRead())
			{
				SetHasBeenRead(flag: true);
				string referenceDisplayName = ParentObject.GetReferenceDisplayName();
				JournalAPI.AddAccomplishment("You read " + referenceDisplayName + ".", "In the month of " + Calendar.GetMonth() + " of " + Calendar.GetYear() + ", =name= penned the influential book, " + referenceDisplayName + ".", "At a remote library near " + JournalAPI.GetLandmarkNearestPlayer().Text + ", =name= met with a group of blind scribes and together they penned the beloved codex " + referenceDisplayName + ".", null, "general", MuralCategory.CreatesSomething, MuralWeight.VeryLow, null, -1L);
			}
		}
		return base.HandleEvent(E);
	}

	public string GetBookKey()
	{
		return "AlreadyRead_" + ID;
	}

	public bool GetHasBeenRead()
	{
		return The.Game.GetStringGameState(GetBookKey()) == "Yes";
	}

	public void SetHasBeenRead(bool flag)
	{
		if (flag)
		{
			The.Game.SetStringGameState(GetBookKey(), "Yes");
		}
		else
		{
			The.Game.SetStringGameState(GetBookKey(), "");
		}
	}
}

}