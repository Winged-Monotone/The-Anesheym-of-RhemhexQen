using System;
using XRL.Rules;

namespace XRL.World.Parts
{
    public class wmLiquidRelatedMethods : IPart
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
                var BasePool = GameObject.Create("Water");
                var LiquidRandomness = Stat.Random(50, 150);

                BasePool.LiquidVolume.Volume = LiquidRandomness;
                BasePool.LiquidVolume.InitialLiquid = LiquidType;
                
                C.AddObject(BasePool);
            }
        }
    }
}