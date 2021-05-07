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
        /// <param name="movementLogic">Movement logic that is shared with EnemyLogic.</param>
        public CoreLogic(GameModel model, MovementLogic movementLogic)
        {
            this.model = model;
            this.pathfindingLogic = new PathfindingLogic();
            this.combatLogic = new CombatLogic(this.model);
            this.movementLogic = movementLogic;
            this.animationLogic = new AnimationLogic(this.model);

            this.buildingFactory = new BuildingFactory(this.model);
            this.unitFactory = new UnitFactory(this.model, 100, 0, 1);
        }

        // Run before rendering

        /// <summary>
        /// One step of the game.
        /// </summary>
        /// <returns>The winner of the game. EMPTY if the game is still on.</returns>
        public OwnerEnum Step()
        {
            this.combatLogic.SetTarget();
            this.movementLogic.UpdatePositions();
            this.Remove();

            if (this.model.PlayerHall.Health <= 0)
            {
                return OwnerEnum.ENEMY;
            }

            if (this.model.EnemyHall.Health <= 0)
            {
                return OwnerEnum.PLAYER;
            }

            return OwnerEnum.EMPTY;
        }

        /// <summary>
        /// Updates each unit's sprite.
        /// </summary>
        public void UpdateAnimation()
        {
            this.animationLogic.UpdateSprites();
        }

        /// <summary>
        /// Inits a new building that will be added to the game scene.
        /// For now the building has extended hitbox.
        /// </summary>
        /// <param name="description">Building type.</param>
        /// <param name="cursorPos">Cursor position that will mark the building's pos.</param>
        public void InitNewBuilding(string description, Point cursorPos)
        {
            if (description.Contains("farm"))
            {
                if (this.model.PlayerGold >= 50 && this.model.PlayerLumber >= 100)
                {
                    this.model.NewBuilding = this.buildingFactory.Create(description, cursorPos.X, cursorPos.Y, false);
                    this.model.NewBuilding.Hitbox.Width += 2 * Config.HitboxExtension;
                    this.model.NewBuilding.Hitbox.Height += 2 * Config.HitboxExtension;
                }
                else
                {
                    this.model.CommandResult = "Cost: 50g 100l";
                }

            }
            else if (description.Contains("barrack"))
            {
                if (this.model.PlayerGold >= 100 && this.model.PlayerLumber >= 200)
                {
                    this.model.NewBuilding = this.buildingFactory.Create(description, cursorPos.X, cursorPos.Y, false);
                    this.model.NewBuilding.Hitbox.Width += 2 * Config.HitboxExtension;
                    this.model.NewBuilding.Hitbox.Height += 2 * Config.HitboxExtension;
                }
                else
                {
                    this.model.CommandResult = "Cost: 100g 200l";
                }

            }
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
                if (this.model.NewBuilding != null)
                {
                    this.TryPlaceBuilding();
                }
                else
                {
                    this.model.SelectedPoint = cursorPos;
                }
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
                this.model.SelectedSubject.InIdle = false;
                this.ClearSelections();
            }
        }

        /// <summary>
        /// Creates a new unit.
        /// </summary>
        /// <param name="description">Unit type description.</param>
        public void CreateUnit(string description)
        {
            System.Drawing.Point positon = new Point(this.model.PlayerHall.Hitbox.X, this.model.PlayerHall.Hitbox.Y);
            positon.Y += this.model.PlayerHall.Hitbox.Height / 2;
            this.unitFactory.Create(description, positon.X - 25, positon.Y);
        }

        /// <inheritdoc/>
        public void PatrollUnit()
        {
            if (this.ValidateDestintaion())
            {
                this.ResetCurrentCommand();
                this.movementLogic.Routines.Add(
                    this.model.SelectedSubject,
                    new PatrolRoutine(
                        this.model.SelectedSubject,
                        this.model.SelectedSubject.Position,
                        this.model.SelectedPoint));
                this.model.SelectedSubject.InIdle = false;
                this.ClearSelections();
            }
        }

        /// <inheritdoc/>
        public void SetUnitsEnemy()
        {
            if (this.model.SelectedSubject != null && this.model.SelectedObject as CombatObject != null)
            {
                this.ResetCurrentCommand();
                this.model.SelectedSubject.Enemy = this.model.SelectedObject as CombatObject;
                this.model.SelectedSubject.Target = this.model.SelectedObject.Position;
                this.model.SelectedSubject.InIdle = false;
                this.ClearSelections();
            }
        }

        /// <inheritdoc/>
        public void GoTo()
        {
            if (this.ValidateDestintaion())
            {
                this.ResetCurrentCommand();
                this.model.SelectedSubject.Target = this.model.SelectedPoint;
                this.model.SelectedSubject.InIdle = false;
                this.ClearSelections();
            }
        }

        /// <summary>
        /// Checks if the new building colledies with somehing. The building can be added to the sceene only if
        /// it doesn't collide with anything.
        /// </summary>
        /// <returns>Bool indicating collision.</returns>
        public bool NewBuildingCollides()
        {
            foreach (Building building in this.model.Buildings)
            {
                if (this.model.NewBuilding.Collides(building))
                {
                    return true;
                }
            }

            foreach (Unit unit in this.model.Units)
            {
                if (this.model.NewBuilding.Collides(unit))
                {
                    return true;
                }
            }

            foreach (GoldMine mine in this.model.GoldMines)
            {
                if (this.model.NewBuilding.Collides(mine))
                {
                    return true;
                }
            }

            foreach (CombatObject tree in this.model.LumberMines)
            {
                if (this.model.NewBuilding.Collides(tree))
                {
                    return true;
                }
            }

            return false;
        }

        private void TryPlaceBuilding()
        {
            if (!this.NewBuildingCollides())
            {
                this.model.NewBuilding.Hitbox.X += Config.HitboxExtension;
                this.model.NewBuilding.Hitbox.Y += Config.HitboxExtension;
                this.model.NewBuilding.Hitbox.Width -= 2 * Config.HitboxExtension;
                this.model.NewBuilding.Hitbox.Height -= 2 * Config.HitboxExtension;
                this.model.Buildings.Add(this.model.NewBuilding);
                this.model.NewBuilding = null;
            }
        }

        private void ResetCurrentCommand()
        {
            if (this.movementLogic.Routines.ContainsKey(this.model.SelectedSubject))
            {
                this.movementLogic.Routines.Remove(this.model.SelectedSubject);
                this.model.SelectedSubject.Path.Clear();
                this.model.SelectedSubject.Target = this.model.SelectedSubject.Position;
                this.model.SelectedSubject.Enemy = null;
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
