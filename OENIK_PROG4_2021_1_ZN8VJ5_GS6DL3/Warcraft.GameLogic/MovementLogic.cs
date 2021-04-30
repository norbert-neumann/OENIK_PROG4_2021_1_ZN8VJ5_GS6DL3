namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using Warcraft.Model;

    /// <summary>
    /// Deals with everything realted to movement.
    /// </summary>
    public class MovementLogic
    {
        /// <summary>
        /// Game model to operate on.
        /// </summary>
        public GameModel model;

        /// <summary>
        /// Ref to pathfinder. If two objects collide movementLogic calls this to find out an alternative path.
        /// </summary>
        public PathfindingLogic pathfinder;

        /// <summary>
        /// Dictionary associating the units wtih their potetntial routines.
        /// </summary>
        public Dictionary<Unit, Routine> routines = new Dictionary<Unit, Routine>();

        private List<Routine> activeRoutines = new List<Routine>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementLogic"/> class.
        /// </summary>
        /// <param name="model">Game model to operate on.</param>
        /// <param name="pathfinder">Pathfinder to operate on.</param>
        public MovementLogic(GameModel model, PathfindingLogic pathfinder)
        {
            this.model = model;
            this.pathfinder = pathfinder;
        }

        /// <summary>
        /// Update each unit's positions.
        /// </summary>
        public void UpdatePositions()
        {
            // IDLE and FIGHTING check, REACHED TARGET CHECK
            foreach (Unit unit in this.model.units)
            {
                if (unit.UnitState != UnitStateEnum.Fighting && !unit.inIdle && !unit.hiding)
                {
                    this.SetNewTargetIfReached(unit, this.routines);
                    this.MoveTowardTarget(unit, Config.Speed, Config.DefaultThreshold);
                    this.CallPathfinderOnCollision(unit);
                }
            }

            List<Routine> routinesToRemove = new List<Routine>();

            foreach (Routine routine in this.activeRoutines)
            {
                if (!routine.Update())
                {
                    routinesToRemove.Add(routine);
                }
            }

            foreach (Routine routine in routinesToRemove)
            {
                this.activeRoutines.Remove(routine);
            }
        }

        private void SetNewTargetIfReached(Unit unit, Dictionary<Unit, Routine> routines)
        {
            if (this.PointToPointDistance(unit.Position, unit.target) < 2)
            {
                Point newTarget = new Point();
                if (unit.path.TryDequeue(out newTarget))
                {
                    unit.target = newTarget;
                }
                else if (routines.ContainsKey(unit))
                {
                    bool putInActiveRoutines = routines[unit].Update();
                    if (putInActiveRoutines)
                    {
                        this.activeRoutines.Add(routines[unit]);
                    }
                }
                else
                {
                    unit.inIdle = true;
                }
            }
        }

        private void MoveTowardTarget(Unit unit, int speed, double threshold)
        {
            Point trajectory = this.ComputeTrajectory(unit.Position, unit.target, threshold);
            Point delta = new Point(trajectory.X, trajectory.Y);

            this.UpdateFacing(unit, delta);

            delta.X *= speed;
            delta.Y *= speed;

            unit.prevPosition = unit.Position;
            unit.ChangePosition(delta);
        }

        private void CallPathfinderOnCollision(Unit unit)
        {
            GameObject collison = null;
            if ((collison = this.FindCollision(unit)) != null && this.ValidateCollision(collison, unit))
            {
                this.pathfinder.FindPath(unit, collison, this.ComputeTrajectory(unit.Position, unit.target, Config.DefaultThreshold));

                // Reset target
                unit.ResetTarget();

                // Movetorawrds with 0 threshhold
                this.MoveTowardTarget(unit, Config.Speed, 0);
            }
        }
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

            double distance = this.PointToPointDistance(postion, target);

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
            foreach (Building building in this.model.buildings)
            {
                if (unit.IsPositionInHitbox(building))
                {
                    return building;
                }
            }

            foreach (GoldMine mine in this.model.goldMines)
            {
                if (unit.IsPositionInHitbox(mine))
                {
                    return mine;
                }
            }

            foreach (CombatObject tree in this.model.lumberMines)
            {
                if (unit.IsPositionInHitbox(tree))
                {
                    return tree;
                }
            }

            return null;
        }

        private double PointToPointDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
