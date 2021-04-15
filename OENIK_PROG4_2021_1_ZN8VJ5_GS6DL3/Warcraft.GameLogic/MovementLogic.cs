using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Warcraft.Model;

namespace Warcraft.GameLogic
{
    public class MovementLogic
    {
        public GameModel model;
        public PathfindingLogic pathfinder;

        public MovementLogic(GameModel model, PathfindingLogic pathfinder)
        {
            this.model = model;
            this.pathfinder = pathfinder;
        }

        public void UpdatePositions()
        {
            // IDLE and FIGHTING check, REACHED TARGET CHECK
            foreach (Unit unit in model.playerUnits)
            {
                if (unit.UnitState == UnitStateEnum.Fighting)
                {
                    this.UpdateFacing(unit, ComputeTrajectory(unit.Position, unit.enemy.Position, Config.DefaultThreshold));
                }
                else
                {
                    this.SetNewTargetIfReached(unit);
                    this.MoveTowardTarget(unit, Config.Speed, Config.DefaultThreshold);
                    this.CallPathfinderOnCollision(unit);
                }
            }
            foreach (Unit unit in model.enemyUnits)
            {
                this.SetNewTargetIfReached(unit);
                this.MoveTowardTarget(unit, Config.Speed, Config.DefaultThreshold);
                this.CallPathfinderOnCollision(unit);
            }
        }

        private void SetNewTargetIfReached(Unit unit)
        {
            if (PointToPointDistance(unit.Position, unit.target) < 2)
            {
                Point newTarget = new Point();
                if (unit.path.TryDequeue(out newTarget))
                {
                    unit.target = newTarget;
                }

            }
        }

        private void MoveTowardTarget(Unit unit, int speed, double threshold)
        {
            Point trajectory = ComputeTrajectory(unit.Position, unit.target, threshold);
            Point delta = new Point(trajectory.X, trajectory.Y);

            this.UpdateFacing(unit, delta);

            delta.X *= speed;
            delta.Y *= speed;

            unit.prevPosition = unit.Position;
            unit.ChangePosition(delta);
            // Where should the next line be??
            //AnimationLogic.IncrementAnimationIndex(unit);
        }

        private void CallPathfinderOnCollision(Unit unit)
        {
            GameObject collison = null;
            if ((collison = FindCollision(unit)) != null)
            {
                this.pathfinder.FindPath(unit, collison as Building, ComputeTrajectory(unit.Position, unit.target, Config.DefaultThreshold));
                // Reset target
                unit.ResetTarget();
                // Movetorawrds with 0 threshhold
                MoveTowardTarget(unit, Config.Speed, 0);
            }
        }

        private Point ComputeTrajectory(Point postion, Point target, double threshold)
        {
            double dx = target.X - postion.X;
            double dy = target.Y - postion.Y;
            double normalizationTerm = Math.Max(Math.Abs(dx), Math.Abs(dy));

            dx /= normalizationTerm;
            dy /= normalizationTerm;

            Point trajectory = new Point(0, 0);

            if (Math.Abs(dx) > threshold)
            {
                trajectory.X = Math.Sign(dx);
            }

            if (Math.Abs(dy) > threshold)
            {
                trajectory.Y = Math.Sign(dy);
            }

            return trajectory;
        }

        private void UpdateFacing(Unit unit, Point delta)
        {
            if (delta == new Point(0, 0)) unit.facing = DirectionEnum.South;
            else if (delta == new Point(0, -1)) unit.facing = DirectionEnum.North;
            else if (delta == new Point(0, 1)) unit.facing = DirectionEnum.South;
            else if (delta == new Point(1, 0)) unit.facing = DirectionEnum.East;
            else if (delta == new Point(-1, 0)) unit.facing = DirectionEnum.West;
            else if (delta == new Point(1, -1)) unit.facing = DirectionEnum.NorthEast;
            else if (delta == new Point(1, 1)) unit.facing = DirectionEnum.SouthEast;
            else if (delta == new Point(-1, 1)) unit.facing = DirectionEnum.SouthWest;
            else if (delta == new Point(-1, -1)) unit.facing = DirectionEnum.NorthWest;
        }

        private GameObject FindCollision(Unit unit)
        {
            foreach (Building building in model.playerBuildings)
            {
                if (unit.IsPositionInHitbox(building))
                {
                    return building;
                }
            }
            foreach (Building building in model.enemyBuildings)
            {
                if (unit.IsPositionInHitbox(building))
                {
                    return building;
                }
            }
            return null;
        }

        private double PointToPointDistance(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
        }
    }
}
