using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ConsoleLib.Console;
using Genkit;
using Newtonsoft.Json;
using UnityEngine;
using XRL.Core;
using XRL.Messages;
using XRL.Rules;
using XRL.Serialization;
using XRL.UI;
using XRL.World.Capabilities;
using XRL.World.Parts;

namespace XRL.World
{
    public static class tryPickStar
    {
        public struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public static List<Cell> PickStar(this GameObject Self, World.Point center, int Radius = 1, int Range = 9999,
            bool Locked = false, AllowVis VisLevel = AllowVis.OnlyVisible, string Label = null)
        {
            try
            {
                if (Self == null)
                {
                    return null;
                }

                // Pre-calculate squared radius to avoid expensive Math.Sqrt() calls


                Cell BaseCell = Self.CurrentCell;
                if (BaseCell == null)
                {
                    return null;
                }

                Zone parentZone = BaseCell.ParentZone;
                if (parentZone == null)
                {
                    return null;
                }

                Cell TargetedCell = Self.CurrentCell;
                if (TargetedCell == null || TargetedCell.ParentZone != parentZone)
                {
                    return null;
                }

                int x1 = BaseCell.X - Radius;
                int x2 = BaseCell.X + Radius;
                int y1 = BaseCell.Y - Radius;
                int y2 = BaseCell.Y + Radius;

                parentZone.Constrain(ref x1, ref y1, ref x2, ref y2);

                // Check if the cell's center falls inside the circle

                List<Cell> list1 = Event.NewCellList();

                for (int y = y1; y <= y2; y++)
                {
                    // IComponent<GameObject>.AddPlayerMessage("Testing Y Iterations");

                    for (int x = x1; x <= x2; x++)
                    {
                        // IComponent<GameObject>.AddPlayerMessage("Testing X Iterations");

                        for (int m = 0; m < Radius; m++)
                        {
                            double dx = x - center.X;
                            double dy = y - center.Y;
                            double distance = Math.Sqrt(dx * dx + dy * dy);
                            
                            Zone.Line(x1,y1,x2,y2);

                            if (distance >= Radius - 0.5 && distance <= Radius + 0.5)
                            {
                                if (!list1.Contains(parentZone.GetCell(x,y)))
                                {
                                    list1.Add(parentZone.GetCell(x,y));
                                }     
                                
                                // IComponent<GameObject>.AddPlayerMessage("Foreach: Cell Grabbing");
                            }
                        }
                    }
                }
                return list1;
            }
            catch
            {
                return null;
            }
        }
    }
}