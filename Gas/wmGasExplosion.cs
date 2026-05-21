using System;


namespace XRL.World.Parts
{
    [Serializable]
    public class wmGasExplosion : IPart
    {
        public int Force = 10000;
        public string Damage = "0";
        public string GasType = "SulfuricGas50";

        public wmGasExplosion()
        {
        }

        public override bool SameAs(IPart p)
        {
            ExplodeOnHit explodeOnHit = p as ExplodeOnHit;
            if (explodeOnHit.Force != Force)
            {
                return false;
            }

            if (explodeOnHit.Damage != Damage)
            {
                return false;
            }

            return base.SameAs(p);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade) && ID != AfterThrownEvent.ID)
            {
                return ID == PooledEvent<IsExplosiveEvent>.ID;
            }

            return true;
        }

        public override bool HandleEvent(AfterThrownEvent E)
        {
            wmGasDetonate(E.Actor);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(IsExplosiveEvent E)
        {
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar eventRegistrar)
        {
            eventRegistrar.Register("ExplodeOnHit");
            base.Register(Object, eventRegistrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "ProjectileHit")
            {
                wmGasDetonate(E.GetGameObjectParameter("Owner"));
            }

            return base.FireEvent(E);
        }

        public void wmGasDetonate(GameObject Owner = null)
        {
            DidX("explode", null, "!");

            var AdjCell = ParentObject.CurrentCell.GetAdjacentCells();

            foreach (var C in AdjCell)
            {
                if (C != null && C.IsEmptyExcludingCombat())
                {
                    C.AddObject(GasType);
                }
            }

            ParentObject.Explode(Force, Owner, Damage);
        }
    }
}