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
    public class MovementLogic : IMovementLogic
    {
        /// <summary>
        /// Dictionary associating the units wtih their potetntial routines.
        /// </summary>
        public Dictionary<Unit, Routine> Routines = new Dictionary<Unit, Routine>();

        /// <summary>
        /// Game model to operate on.
        /// </summary>
        private GameModel model;

        /// <summary>
        /// Ref to pathfinder. If two objects collide movementLogic calls this to find out an alternative path.
        /// </summary>
        private PathfindingLogic pathfinder;

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
            foreach (Unit unit in this.model.Units)
            {
                if (unit.UnitState != UnitStateEnum.Fighting && !unit.InIdle && !unit.Hiding)
                {
                    this.SetNewTargetIfReached(unit, this.Routines);
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
            if (this.PointToPointDistance(unit.Position, unit.Target) < 2)
            {
                Point newTarget = new Point(0, 0);
                if (unit.Path.TryDequeue(out newTarget))
                {
                    unit.Target = newTarget;
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
                    unit.InIdle = true;
                }
            }
        }

        private void MoveTowardTarget(Unit unit, int speed, double threshold)
        {
            Point trajectory = this.ComputeTrajectory(unit.Position, unit.Target, threshold);
            Point delta = new Point(trajectory.X, trajectory.Y);

            this.UpdateFacing(unit, delta);

            delta.X *= speed;
            delta.Y *= speed;

            unit.PrevPosition = unit.Position;
            unit.ChangePosition(delta);
        }

        private void CallPathfinderOnCollision(Unit unit)
        {
            GameObject collison = null;
            if ((collison = this.FindCollision(unit)) != null && this.ValidateCollision(collison, unit))
            {
                this.pathfinder.FindPath(unit, collison, this.ComputeTrajectory(unit.Position, unit.Target, Config.DefaultThreshold));

                // Reset target
                unit.ResetTarget();

                // Movetorawrds with 0 threshhold
                this.MoveTowardTarget(unit, Config.Speed, 0);
            }
        }

        private bool ValidateCollision(GameObject collision, Unit unit)
        {
            unit.EntryPoint = unit.PrevPosition;

            if (unit.Enemy != null && unit.Enemy.Equals(collision))
            {
                return false;
            }

            if (this.Routines.ContainsKey(unit))
            {
                Routine r = this.Routines[unit];

                if (r as HarvestLumberRoutine != null)
                {
                    if ((r as HarvestLumberRoutine).TargetObject.Equals(collision))
                    {
                        if (r.Update())
                        {
                            this.activeRoutines.Add(r);
                        }

                        return false;
                    }
                }
                else if (r as GoldMiningRoutine != null)
                {
                    if ((r as GoldMiningRoutine).TargetObject.Equals(collision))
                    {
                        if (r.Update())
                        {
                            this.activeRoutines.Add(r);
                        }

                        return false;
                    }
                }
            }

            return true;
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
            if (delta == new Point(0, 0)) unit.Facing = DirectionEnum.South;
            else if (delta == new Point(0, -1)) unit.Facing = DirectionEnum.North;
            else if (delta == new Point(0, 1)) unit.Facing = DirectionEnum.South;
            else if (delta == new Point(1, 0)) unit.Facing = DirectionEnum.East;
            else if (delta == new Point(-1, 0)) unit.Facing = DirectionEnum.West;
            else if (delta == new Point(1, -1)) unit.Facing = DirectionEnum.NorthEast;
            else if (delta == new Point(1, 1)) unit.Facing = DirectionEnum.SouthEast;
            else if (delta == new Point(-1, 1)) unit.Facing = DirectionEnum.SouthWest;
            else if (delta == new Point(-1, -1)) unit.Facing = DirectionEnum.NorthWest;
        }

        private GameObject FindCollision(Unit unit)
        {
            foreach (Building building in this.model.Buildings)
            {
                if (unit.IsPositionInHitbox(building))
                {
                    return building;
                }
            }

            foreach (GoldMine mine in this.model.GoldMines)
            {
                if (unit.IsPositionInHitbox(mine))
                {
                    return mine;
                }
            }

            foreach (CombatObject tree in this.model.LumberMines)
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
