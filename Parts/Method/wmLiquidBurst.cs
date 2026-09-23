using System;

namespace XRL.World.Parts
{
    public class LiquidRelatedMethods : IPart
    {
        public static void wmLiquidBurst(GameObject Actor, String LiquidType, bool Explosive, bool ChangesTemperature, int ExplosiveForce, int TemepratureShift, string SplashParticle)
        {
            if (Explosive == true)
                Physics.ApplyExplosion(Actor.CurrentCell, ExplosiveForce);
            
            if (ChangesTemperature == true)
                Actor.CurrentCell.TemperatureChange(TemepratureShift);
                
            Actor.CurrentCell.Splash(SplashParticle);
                
            foreach (Cell C in Actor.CurrentCell.GetAdjacentCells())
            {
                C.AddObject(LiquidType);
                C.AddObject(LiquidType);
            }
        }
    }
}