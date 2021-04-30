﻿namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Warcraft.Model;

    /// <summary>
    /// Encapsulates and calls each task specific logic.
    /// The Controller also calls this logic's methods.
    /// </summary>
    public class CoreLogic
    {
        private GameModel model;
        private CombatLogic combatLogic;
        private MovementLogic movementLogic;
        private AnimationLogic animationLogic;
        private PathfindingLogic pathfindingLogic;

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