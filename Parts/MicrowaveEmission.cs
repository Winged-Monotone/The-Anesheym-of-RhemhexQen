using System;
using XRL.World.Effects;

namespace XRL.World.Parts
{
    public class MicrowaveEmission : IPart
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                || ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            var ActiveCell = ParentObject.GetCurrentCell();
            var Target = ActiveCell.GetCombatObject();
            
            if (Target.IsValid())
            {
                if (Target.TryGetEffect(out Microwaving Micro))
                {

                    if (Micro.Duration <= 10)
                        Micro.Duration += 5;
                    
                    Micro.Stacks += 1;
                }
                else
                {
                    Target.ApplyEffect(new Microwaving());
                }
            }

           var ACell = ActiveCell.GetOpenLiquidVolume();
           
           if (ACell.IsValid() && ACell != Target)
           {
               ACell.Destroy();
               PlayWorldSound("sfx_warmStaticSizzle", 0.20F, 0.1F);
           }
            
            return base.HandleEvent(E);
        }
    }
}