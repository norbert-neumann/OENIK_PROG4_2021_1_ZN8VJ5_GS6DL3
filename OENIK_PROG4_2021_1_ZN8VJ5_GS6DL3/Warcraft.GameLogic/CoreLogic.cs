namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using Warcraft.Model;

    /// <summary>
    /// Encapsulates and calls each task specific logic.
    /// The Controller also calls this logic's methods.
    /// </summary>
    public class CoreLogic : ICoreLogic
    {
        private GameModel model;
        private CombatLogic combatLogic;
        private MovementLogic movementLogic;
        private AnimationLogic animationLogic;
        private PathfindingLogic pathfindingLogic;
        private BuildingFactory buildingFactory;
        private UnitFactory unitFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreLogic"/> class.
        /// </summary>
        /// <param name="model">Game model to operate on.</param>
        /// <param name="combatLogic">Combat logic instance.</param>
        /// <param name="movementLogic">Movement logic instance.</param>
        /// <param name="animationLogic">Animatin logic instance.</param>
        /// <param name="pathfindingLogic">Pathfinding logic instance.</param>
        public CoreLogic(GameModel model, CombatLogic combatLogic, MovementLogic movementLogic, AnimationLogic animationLogic, PathfindingLogic pathfindingLogic)
        {
            this.model = model;
            this.combatLogic = combatLogic;
            this.movementLogic = movementLogic;
            this.animationLogic = animationLogic;
            this.pathfindingLogic = pathfindingLogic;
            this.buildingFactory = new BuildingFactory(this.model);
            this.unitFactory = new UnitFactory(this.model);
        }

        // Run before rendering

        /// <summary>
        /// One step of the game.
        /// </summary>
        public void Step()
        {
            this.combatLogic.SetTarget();
            this.movementLogic.UpdatePositions();
            this.Remove();
        }

        /// <summary>
        /// Updates each unit's sprite.
        /// </summary>
        public void UpdateAnimation()
        {
            this.animationLogic.UpdateSprites();
        }

        /// <summary>
        /// Selects an Object, Subject, or a Point depoending on the cursorPos.
        /// </summary>
        /// <param name="cursorPos">User's cursor position.</param>
        public void Select(Point cursorPos)
        {
            GameObject clickedObject = this.FindGameObject(cursorPos);

            if (clickedObject != null)
            {
                if (clickedObject is Unit)
                {
                    Unit clickedUnit = clickedObject as Unit;
                    if (clickedUnit.Owner == OwnerEnum.PLAYER)
                    {
                        this.model.SelectedSubject = clickedUnit;
                    }
                    else
                    {
                        this.model.SelectedObject = clickedUnit;
                    }
                }
                else
                {
                    this.model.SelectedObject = clickedObject;
                }
            }
            else
            {
                this.model.SelectedPoint = cursorPos;
            }
        }

        /// <inheritdoc/>
        public void MineGold()
        {
            if (this.ValidateCommand() && this.model.SelectedObject is GoldMine)
            {
                this.ResetCurrentCommand();
                this.movementLogic.Routines.Add(
                    this.model.SelectedSubject,
                    new GoldMiningRoutine(
                        this.model.SelectedSubject,
                        TimeSpan.FromSeconds(3),
                        this.model.PlayerHall,
                        this.model.SelectedObject as GoldMine));

                this.model.SelectedSubject.InIdle = false;
                this.ClearSelections();
            }
        }

        /// <inheritdoc/>
        public void HarvestLumber()
        {
            // TODO
            if (this.ValidateCommand() && this.model.SelectedObject is CombatObject)
            {
                this.ResetCurrentCommand();
                this.movementLogic.Routines.Add(
                    this.model.SelectedSubject,
                    new HarvestLumberRoutine(
                        this.model.SelectedSubject,
                        TimeSpan.FromSeconds(3),
                        this.model.PlayerHall,
                        this.model.SelectedObject as CombatObject));
                this.ClearSelections();
            }
        }

        /// <inheritdoc/>
        public void PatrollUnit()
        {
            if (this.ValidateDestintaion())
            {
                this.movementLogic.Routines.Add(
                    this.model.SelectedSubject,
                    new PatrolRoutine(
                        this.model.SelectedSubject,
                        this.model.SelectedSubject.Position,
                        this.model.SelectedPoint));
                this.ClearSelections();
            }
        }

        /// <inheritdoc/>
        public void SetUnitsEnemy()
        {
            if (this.model.SelectedSubject != null && this.model.SelectedObject as CombatObject != null)
            {
                this.model.SelectedSubject.Enemy = this.model.SelectedObject as CombatObject;
                this.ClearSelections();
            }
        }

        /// <inheritdoc/>
        public void GoTo()
        {
            if (this.ValidateDestintaion())
            {
                this.model.SelectedSubject.Target = this.model.SelectedPoint;
                this.ClearSelections();
            }
        }

        private void ClearSelections()
        {
            this.model.SelectedSubject = null;
            this.model.SelectedObject = null;
            this.model.SelectedPoint = new Point(-1, -1);
        }

        private bool ValidateCommand()
        {
            return this.model.SelectedSubject != null && this.model.SelectedObject != null;
        }

        private bool ValidateDestintaion()
        {
            return this.model.SelectedSubject != null && this.model.SelectedPoint != new Point(-1, -1);
        }

        private GameObject FindGameObject(Point cursorPos)
        {
            foreach (Unit unit in this.model.Units)
            {
                if (this.PositionInHitbox(cursorPos, unit.Hitbox))
                {
                    return unit;
                }
            }

            foreach (Building building in this.model.Buildings)
            {
                if (this.PositionInHitbox(cursorPos, building.Hitbox))
                {
                    return building;
                }
            }

            foreach (GoldMine mine in this.model.GoldMines)
            {
                if (this.PositionInHitbox(cursorPos, mine.Hitbox))
                {
                    return mine;
                }
            }

            foreach (CombatObject tree in this.model.LumberMines)
            {
                if (this.PositionInHitbox(cursorPos, tree.Hitbox))
                {
                    return tree;
                }
            }

            return null;
        }

        private bool PositionInHitbox(Point point, Rectangle hitbox)
        {
            return point.X >= hitbox.X && point.X <= hitbox.Right &&
                point.Y >= hitbox.Y && point.Y <= hitbox.Bottom;
        }

        private void Remove()
        {
            foreach (Unit unit in this.model.UnitsToRemove)
            {
                this.model.Units.Remove(unit);
            }

            foreach (Building b in this.model.BuildingsToRemove)
            {
                this.model.Buildings.Remove(b);
            }

            foreach (CombatObject t in this.model.TreesToRemove)
            {
                this.model.LumberMines.Remove(t);
            }

            foreach (GoldMine m in this.model.GoldMinesToRemove)
            {
                this.model.GoldMines.Remove(m);
            }

            this.model.UnitsToRemove.Clear();
            this.model.BuildingsToRemove.Clear();
            this.model.TreesToRemove.Clear();
            this.model.GoldMinesToRemove.Clear();
        }
    }
}
