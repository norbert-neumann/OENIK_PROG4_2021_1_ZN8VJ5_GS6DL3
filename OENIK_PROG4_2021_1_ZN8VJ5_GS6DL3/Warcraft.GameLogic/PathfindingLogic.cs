using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Warcraft.Model;

namespace Warcraft.GameLogic
{
    public class PathfindingLogic
    {
        public void FindPath(Unit unit, GameObject collison, Point trajectory)
        {
            Point UpperLeftBorder = new Point(collison.hitbox.X - Config.BorderWidth, collison.hitbox.Y - Config.BorderWidth);
            Point UpperRightBorder = new Point(collison.hitbox.Right + Config.BorderWidth, collison.hitbox.Y - Config.BorderWidth);
            Point BottomLeftBorder = new Point(collison.hitbox.X - Config.BorderWidth, collison.hitbox.Bottom + Config.BorderWidth);
            Point BottomRightBorder = new Point(collison.hitbox.Right + Config.BorderWidth, collison.hitbox.Bottom + Config.BorderWidth);
            Point startPoint = unit.prevPosition;
            // THIS MIGHT CAUSE BUGS
            //Point startPoint = unit.Position;
            Point endPoint = new Point(trajectory.X * collison.hitbox.Width, trajectory.Y * collison.hitbox.Height);
            endPoint.X += startPoint.X + 20;
            endPoint.Y += startPoint.Y + 20;
            endPoint = CalculateEndPoint(unit.target, collison);

            Queue<Point> pathA = new Queue<Point>();
            Queue<Point> pathB = new Queue<Point>();

            double pathALength = 0;
            double pathBLength = 0;

            // Now calculate the two possible paths

            // LEFT
            if (startPoint.X <= collison.hitbox.X - 1)
            {
                // Path A: Up + Right + (Down)
                // Up
                pathALength += L1Distance(startPoint, UpperLeftBorder);
                pathA.Enqueue(UpperLeftBorder);
                if (UpperLeftBorder.Y != endPoint.Y)
                {
                    pathALength += L1Distance(UpperLeftBorder, UpperRightBorder);
                    pathA.Enqueue(UpperRightBorder);
                    pathALength += L1Distance(UpperRightBorder, endPoint);
                }
                else
                {
                    pathALength += L1Distance(UpperLeftBorder, endPoint);
                }
                pathA.Enqueue(endPoint);

                // Path B: Down + Right + (Up)
                pathB.Enqueue(BottomLeftBorder);
                if (BottomLeftBorder.X != endPoint.Y)
                {
                    pathBLength += L1Distance(BottomLeftBorder, BottomRightBorder);
                    pathB.Enqueue(BottomRightBorder);
                    pathBLength += L1Distance(BottomRightBorder, endPoint);
                }
                else
                {
                    pathBLength += L1Distance(BottomLeftBorder, endPoint);
                }
                pathB.Enqueue(endPoint);
            }

            // RIGHT
            else if (startPoint.X >= collison.hitbox.Right + 1)
            {
                // PATH A: Up + Left + Down
                pathA.Enqueue(UpperRightBorder);
                pathALength += L1Distance(startPoint, UpperRightBorder);
                if (UpperRightBorder.X != endPoint.X)
                {
                    pathALength += L1Distance(UpperRightBorder, UpperLeftBorder);
                    pathA.Enqueue(UpperLeftBorder);
                    pathALength += L1Distance(UpperLeftBorder, endPoint);
                }
                else
                {
                    pathALength += L1Distance(UpperRightBorder, endPoint);
                }
                pathA.Enqueue(endPoint);

                // PATH B: Down + Left + Up
                pathB.Enqueue(BottomRightBorder);
                pathBLength += L1Distance(startPoint, BottomRightBorder);
                if (BottomRightBorder.X != endPoint.X)
                {
                    pathBLength += L1Distance(BottomRightBorder, BottomLeftBorder);
                    pathB.Enqueue(BottomLeftBorder);
                    pathBLength += L1Distance(BottomLeftBorder, endPoint);
                }
                else
                {
                    pathBLength += L1Distance(BottomRightBorder, endPoint);
                }
                pathB.Enqueue(endPoint);
            }

            // UP
            else if (startPoint.Y <= collison.hitbox.Y - 1)
            {
                // PATH A: Right + Down + Left
                pathA.Enqueue(UpperRightBorder);
                pathALength += L1Distance(startPoint, UpperRightBorder);
                if (UpperRightBorder.X != endPoint.X)
                {
                    pathALength += L1Distance(UpperRightBorder, BottomRightBorder);
                    pathA.Enqueue(BottomRightBorder);
                    pathALength += L1Distance(BottomRightBorder, endPoint);
                }
                else
                {
                    pathALength += L1Distance(UpperRightBorder, endPoint);
                }
                pathA.Enqueue(endPoint);

                // PATH B: Left + Down + Right
                pathB.Enqueue(UpperLeftBorder);
                pathBLength += L1Distance(startPoint, UpperLeftBorder);
                if (UpperLeftBorder.X != endPoint.X)
                {
                    pathBLength += L1Distance(BottomLeftBorder, UpperLeftBorder);
                    pathB.Enqueue(BottomLeftBorder);
                    pathBLength += L1Distance(BottomLeftBorder, endPoint);
                }
                else
                {
                    pathBLength += L1Distance(UpperLeftBorder, endPoint);
                }
                pathB.Enqueue(endPoint);
            }

            // DOWN
            else if (startPoint.Y >= collison.hitbox.Bottom + 1)
            {
                // PATH A: Right + Up + Left
                pathA.Enqueue(BottomRightBorder);
                pathALength += L1Distance(startPoint, BottomRightBorder);
                if (BottomRightBorder.X != endPoint.X)
                {
                    pathALength += L1Distance(UpperRightBorder, BottomRightBorder);
                    pathA.Enqueue(UpperRightBorder);
                    pathALength += L1Distance(UpperRightBorder, endPoint);
                }
                else
                {
                    pathALength += L1Distance(BottomRightBorder, endPoint);
                }
                pathA.Enqueue(endPoint);

                // PATH B: Left + Up + Right
                pathB.Enqueue(BottomLeftBorder);
                pathBLength += L1Distance(startPoint, BottomLeftBorder);
                if (BottomLeftBorder.X != endPoint.X)
                {
                    pathBLength += L1Distance(BottomLeftBorder, UpperLeftBorder);
                    pathB.Enqueue(UpperLeftBorder);
                    pathBLength += L1Distance(UpperLeftBorder, endPoint);
                }
                else
                {
                    pathBLength += L1Distance(BottomLeftBorder, endPoint);
                }
                pathB.Enqueue(endPoint);
            }

            if (pathALength < pathBLength)
            {
                unit.path = pathA;
            }
            else
            {
                unit.path = pathB;
            }
        }

        private Point CalculateEndPoint(Point target, GameObject collison)
        {
            Point endPoint = new Point(target.X, target.Y);
            Point UpperLeftBorder = new Point(collison.hitbox.X - Config.BorderWidth, collison.hitbox.Y - Config.BorderWidth);
            Point UpperRightBorder = new Point(collison.hitbox.Right + Config.BorderWidth, collison.hitbox.Y - Config.BorderWidth);
            Point BottomLeftBorder = new Point(collison.hitbox.X - Config.BorderWidth, collison.hitbox.Bottom + Config.BorderWidth);
            Point BottomRightBorder = new Point(collison.hitbox.Right + Config.BorderWidth, collison.hitbox.Bottom + Config.BorderWidth);

            double UpperLeftDistance = L1Distance(UpperLeftBorder, target);
            double UpperRightDistance = L1Distance(UpperRightBorder, target);
            double BottomLeftDistance = L1Distance(BottomLeftBorder, target);
            double BottomRightDistance = L1Distance(BottomRightBorder, target);

            double min = Math.Min(Math.Min(UpperLeftDistance, UpperRightDistance), Math.Min(BottomLeftDistance, BottomRightDistance));

            if (min == UpperLeftDistance)
            {
                if (Math.Abs(endPoint.X - UpperLeftBorder.X) >= Math.Abs(endPoint.Y - UpperLeftBorder.Y))
                {
                    endPoint.X = collison.hitbox.Left - Config.BorderWidth;
                }
                if (Math.Abs(endPoint.Y - UpperLeftBorder.Y) >= Math.Abs(endPoint.X - UpperLeftBorder.X))
                {
                    endPoint.Y = collison.hitbox.Top - Config.BorderWidth;
                }
            }
            else if (min == UpperRightDistance)
            {
                if (Math.Abs(endPoint.X - UpperLeftBorder.X) >= Math.Abs(endPoint.Y - UpperLeftBorder.Y))
                {
                    endPoint.X = collison.hitbox.Right + Config.BorderWidth;
                }
                if (Math.Abs(endPoint.Y - UpperLeftBorder.Y) >= Math.Abs(endPoint.X - UpperLeftBorder.X))
                {
                    endPoint.Y = collison.hitbox.Top - Config.BorderWidth;
                }
            }
            else if (min == BottomLeftDistance)
            {
                if (Math.Abs(endPoint.X - UpperLeftBorder.X) >= Math.Abs(endPoint.Y - UpperLeftBorder.Y))
                {
                    endPoint.X = collison.hitbox.Left - Config.BorderWidth;
                }
                if (Math.Abs(endPoint.Y - UpperLeftBorder.Y) >= Math.Abs(endPoint.X - UpperLeftBorder.X))
                {
                    endPoint.Y = collison.hitbox.Bottom + Config.BorderWidth;
                }
            }
            else if (min == BottomRightDistance)
            {
                if (Math.Abs(endPoint.X - UpperLeftBorder.X) >= Math.Abs(endPoint.Y - UpperLeftBorder.Y))
                {
                    endPoint.X = collison.hitbox.Right + Config.BorderWidth;
                }
                if (Math.Abs(endPoint.Y - UpperLeftBorder.Y) >= Math.Abs(endPoint.X - UpperLeftBorder.X))
                {
                    endPoint.Y = collison.hitbox.Bottom + Config.BorderWidth;
                }
            }

            return endPoint;
        }

        private double L1Distance(Point A, Point B)
        {
            return Math.Abs(A.X - B.X) + Math.Abs(A.Y - B.Y);
        }
    }
}
