using XRL.Rules;
using XRL.World;
using System;
using System.Collections.Generic;
using XRL.UI;

namespace XRL.World.Parts
{
    public class SingularityBigSuck : IPart
    {
        // Big butthole has a little warning flash and then sucks in the player with a radial burst before exploding. If th eplayer touches the singularity before it explodes, the player is SPEGHETTIFACIATIOEBRNEUIORG WYUEDBWYDOVIWDUWJ;

        public int NonPrimedDuration = Stat.Random(7, 14);
        public int PrimedDuration = 3;

        public bool Primed = false;

        public override bool WantEvent(int ID, int cascade)
        {
            return ID == EndTurnEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            --NonPrimedDuration;

            if (NonPrimedDuration <= 0 && !Primed)
            {
                var eCurrentZone = ParentObject.CurrentZone;
                var eCellsList = eCurrentZone.GetCells();

                var Affected = new HashSet<GameObject>();
                
                Primed = true;

                foreach (var C in eCellsList)
                {
                    var obj = C.GetCombatTarget();
                    
                    // HashSet checks if the property has been added to its list that is specific for these parameters
                    if (obj.IsValid() && obj.IsCreature && Affected.Add(obj))
                    {
                        var text2 = obj.Render.Tile;
                        // var colorString = (obj.Render.TileColor.IsNullOrEmpty() ? obj.Render.ColorString : obj.Render.TileColor);
                        // var detailColor = obj.Render.DetailColor;
                        
                        var colorString = "&y";
                        var detailColor = "Y";
                        
                        obj.TileParticleBlip(text2, colorString, detailColor, 20, IgnoreVisibility: false, obj.Render.getHFlip(), obj.Render.getVFlip(), 0L);;
                        obj.Push(obj.GetDirectionToward(ParentObject, false), 15000);
                        obj.TileParticleBlip(text2, colorString, detailColor, 25, IgnoreVisibility: false, obj.Render.getHFlip(), obj.Render.getVFlip(), 0L);;
                        obj.Push(obj.GetDirectionToward(ParentObject, false), 15000);
                        obj.TileParticleBlip(text2, colorString, detailColor, 30, IgnoreVisibility: false, obj.Render.getHFlip(), obj.Render.getVFlip(), 0L);;
                        obj.Push(obj.GetDirectionToward(ParentObject, false), 15000);
                    }
                }
            }
            else if (Primed)
            {
                --PrimedDuration;

                if (PrimedDuration <= 0)
                {
                    ParentObject.Explode(1500, Neutron: true);
                }
            }

            return base.HandleEvent(E);
        }
    }
}