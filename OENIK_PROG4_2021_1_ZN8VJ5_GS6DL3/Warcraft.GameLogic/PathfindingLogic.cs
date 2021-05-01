namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using Warcraft.Model;

    /// <summary>
    /// Finds a path around a specific collision and a unit.
    /// </summary>
    public class PathfindingLogic : IPathfindingLogic
    {
        /// <summary>
        /// Finds a path.
        /// </summary>
        /// <param name="unit">Unit to find a path for.</param>
        /// <param name="collison">Unit collides with this gameObject.</param>
        /// <param name="trajectory">The units trajectory.</param>
        public void FindPath(Unit unit, GameObject collison, Point trajectory)
        {
            Point upperLeftBorder = new Point(collison.Hitbox.X - Config.BorderWidth, collison.Hitbox.Y - Config.BorderWidth);
            Point upperRightBorder = new Point(collison.Hitbox.Right + Config.BorderWidth, collison.Hitbox.Y - Config.BorderWidth);
            Point bottomLeftBorder = new Point(collison.Hitbox.X - Config.BorderWidth, collison.Hitbox.Bottom + Config.BorderWidth);
            Point bottomRightBorder = new Point(collison.Hitbox.Right + Config.BorderWidth, collison.Hitbox.Bottom + Config.BorderWidth);
            Point startPoint = unit.PrevPosition;
            Point endPoint = new Point(trajectory.X * collison.Hitbox.Width, trajectory.Y * collison.Hitbox.Height);
            endPoint.X += startPoint.X + 20;
            endPoint.Y += startPoint.Y + 20;
            endPoint = this.CalculateEndPoint(unit.Target, collison);

            Queue<Point> pathA = new Queue<Point>();
            Queue<Point> pathB = new Queue<Point>();

            double pathALength = 0;
            double pathBLength = 0;

            // Now calculate the two possible paths

            // LEFT
            if (startPoint.X <= collison.Hitbox.X - 1)
            {
                // Path A: Up + Right + (Down)
                // Up
                pathALength += this.L1Distance(startPoint, upperLeftBorder);
                pathA.Enqueue(upperLeftBorder);
                if (upperLeftBorder.Y != endPoint.Y)
                {
                    pathALength += this.L1Distance(upperLeftBorder, upperRightBorder);
                    pathA.Enqueue(upperRightBorder);
                    pathALength += this.L1Distance(upperRightBorder, endPoint);
                }
                else
                {
                    pathALength += this.L1Distance(upperLeftBorder, endPoint);
                }

                pathA.Enqueue(endPoint);

                // Path B: Down + Right + (Up)
                pathB.Enqueue(bottomLeftBorder);
                if (bottomLeftBorder.X != endPoint.Y)
                {
                    pathBLength += this.L1Distance(bottomLeftBorder, bottomRightBorder);
                    pathB.Enqueue(bottomRightBorder);
                    pathBLength += this.L1Distance(bottomRightBorder, endPoint);
                }
                else
                {
                    pathBLength += this.L1Distance(bottomLeftBorder, endPoint);
                }

                pathB.Enqueue(endPoint);
            }

            // RIGHT
            else if (startPoint.X >= collison.Hitbox.Right + 1)
            {
                // PATH A: Up + Left + Down
                pathA.Enqueue(upperRightBorder);
                pathALength += this.L1Distance(startPoint, upperRightBorder);
                if (upperRightBorder.X != endPoint.X)
                {
                    pathALength += this.L1Distance(upperRightBorder, upperLeftBorder);
                    pathA.Enqueue(upperLeftBorder);
                    pathALength += this.L1Distance(upperLeftBorder, endPoint);
                }
                else
                {
                    pathALength += this.L1Distance(upperRightBorder, endPoint);
                }

                pathA.Enqueue(endPoint);

                // PATH B: Down + Left + Up
                pathB.Enqueue(bottomRightBorder);
                pathBLength += this.L1Distance(startPoint, bottomRightBorder);
                if (bottomRightBorder.X != endPoint.X)
                {
                    pathBLength += this.L1Distance(bottomRightBorder, bottomLeftBorder);
                    pathB.Enqueue(bottomLeftBorder);
                    pathBLength += this.L1Distance(bottomLeftBorder, endPoint);
                }
                else
                {
                    pathBLength += this.L1Distance(bottomRightBorder, endPoint);
                }

                pathB.Enqueue(endPoint);
            }

            // UP
            else if (startPoint.Y <= collison.Hitbox.Y - 1)
            {
                // PATH A: Right + Down + Left
                pathA.Enqueue(upperRightBorder);
                pathALength += this.L1Distance(startPoint, upperRightBorder);
                if (upperRightBorder.X != endPoint.X)
                {
                    pathALength += this.L1Distance(upperRightBorder, bottomRightBorder);
                    pathA.Enqueue(bottomRightBorder);
                    pathALength += this.L1Distance(bottomRightBorder, endPoint);
                }
                else
                {
                    pathALength += this.L1Distance(upperRightBorder, endPoint);
                }

                pathA.Enqueue(endPoint);

                // PATH B: Left + Down + Right
                pathB.Enqueue(upperLeftBorder);
                pathBLength += this.L1Distance(startPoint, upperLeftBorder);
                if (upperLeftBorder.X != endPoint.X)
                {
                    pathBLength += this.L1Distance(bottomLeftBorder, upperLeftBorder);
                    pathB.Enqueue(bottomLeftBorder);
                    pathBLength += this.L1Distance(bottomLeftBorder, endPoint);
                }
                else
                {
                    pathBLength += this.L1Distance(upperLeftBorder, endPoint);
                }

                pathB.Enqueue(endPoint);
            }

            // DOWN
            else if (startPoint.Y >= collison.Hitbox.Bottom + 1)
            {
                // PATH A: Right + Up + Left
                pathA.Enqueue(bottomRightBorder);
                pathALength += this.L1Distance(startPoint, bottomRightBorder);
                if (bottomRightBorder.X != endPoint.X)
                {
                    pathALength += this.L1Distance(upperRightBorder, bottomRightBorder);
                    pathA.Enqueue(upperRightBorder);
                    pathALength += this.L1Distance(upperRightBorder, endPoint);
                }
                else
                {
                    pathALength += this.L1Distance(bottomRightBorder, endPoint);
                }

                pathA.Enqueue(endPoint);

                // PATH B: Left + Up + Right
                pathB.Enqueue(bottomLeftBorder);
                pathBLength += this.L1Distance(startPoint, bottomLeftBorder);
                if (bottomLeftBorder.X != endPoint.X)
                {
                    pathBLength += this.L1Distance(bottomLeftBorder, upperLeftBorder);
                    pathB.Enqueue(upperLeftBorder);
                    pathBLength += this.L1Distance(upperLeftBorder, endPoint);
                }
                else
                {
                    pathBLength += this.L1Distance(bottomLeftBorder, endPoint);
                }

                pathB.Enqueue(endPoint);
            }

            if (pathALength < pathBLength)
            {
                unit.Path = pathA;
            }
            else
            {
                unit.Path = pathB;
            }
        }

        private Point CalculateEndPoint(Point target, GameObject collison)
        {
            Point endPoint = new Point(target.X, target.Y);
            Point upperLeftBorder = new Point(collison.Hitbox.X - Config.BorderWidth, collison.Hitbox.Y - Config.BorderWidth);
            Point upperRightBorder = new Point(collison.Hitbox.Right + Config.BorderWidth, collison.Hitbox.Y - Config.BorderWidth);
            Point bottomLeftBorder = new Point(collison.Hitbox.X - Config.BorderWidth, collison.Hitbox.Bottom + Config.BorderWidth);
            Point bottomRightBorder = new Point(collison.Hitbox.Right + Config.BorderWidth, collison.Hitbox.Bottom + Config.BorderWidth);

            double upperLeftDistance = this.L1Distance(upperLeftBorder, target);
            double upperRightDistance = this.L1Distance(upperRightBorder, target);
            double bottomLeftDistance = this.L1Distance(bottomLeftBorder, target);
            double bottomRightDistance = this.L1Distance(bottomRightBorder, target);

            double min = Math.Min(Math.Min(upperLeftDistance, upperRightDistance), Math.Min(bottomLeftDistance, bottomRightDistance));

            if (min == upperLeftDistance)
            {
                if (Math.Abs(endPoint.X - upperLeftBorder.X) >= Math.Abs(endPoint.Y - upperLeftBorder.Y))
                {
                    endPoint.X = collison.Hitbox.Left - Config.BorderWidth;
                }

                if (Math.Abs(endPoint.Y - upperLeftBorder.Y) >= Math.Abs(endPoint.X - upperLeftBorder.X))
                {
                    endPoint.Y = collison.Hitbox.Top - Config.BorderWidth;
                }
            }
            else if (min == upperRightDistance)
            {
                if (Math.Abs(endPoint.X - upperLeftBorder.X) >= Math.Abs(endPoint.Y - upperLeftBorder.Y))
                {
                    endPoint.X = collison.Hitbox.Right + Config.BorderWidth;
                }

                if (Math.Abs(endPoint.Y - upperLeftBorder.Y) >= Math.Abs(endPoint.X - upperLeftBorder.X))
                {
                    endPoint.Y = collison.Hitbox.Top - Config.BorderWidth;
                }
            }
            else if (min == bottomLeftDistance)
            {
                if (Math.Abs(endPoint.X - upperLeftBorder.X) >= Math.Abs(endPoint.Y - upperLeftBorder.Y))
                {
                    endPoint.X = collison.Hitbox.Left - Config.BorderWidth;
                }

                if (Math.Abs(endPoint.Y - upperLeftBorder.Y) >= Math.Abs(endPoint.X - upperLeftBorder.X))
                {
                    endPoint.Y = collison.Hitbox.Bottom + Config.BorderWidth;
                }
            }
            else if (min == bottomRightDistance)
            {
                if (Math.Abs(endPoint.X - upperLeftBorder.X) >= Math.Abs(endPoint.Y - upperLeftBorder.Y))
                {
                    endPoint.X = collison.Hitbox.Right + Config.BorderWidth;
                }

                if (Math.Abs(endPoint.Y - upperLeftBorder.Y) >= Math.Abs(endPoint.X - upperLeftBorder.X))
                {
                    endPoint.Y = collison.Hitbox.Bottom + Config.BorderWidth;
                }
            }

            return endPoint;
        }

        private double L1Distance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
