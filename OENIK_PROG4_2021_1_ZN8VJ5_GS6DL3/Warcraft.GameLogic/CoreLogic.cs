using System;
using System.Collections.Generic;
using System.Text;
using Warcraft.Model;

namespace Warcraft.GameLogic
{
    public class CoreLogic
    {
        private GameModel model;
        private CombatLogic combatLogic;
        private MovementLogic movementLogic;
        private AnimationLogic animationLogic;
        private PathfindingLogic pathfindingLogic;

        public CoreLogic(GameModel model, CombatLogic combatLogic, MovementLogic movementLogic, AnimationLogic animationLogic, PathfindingLogic pathfindingLogic)
        {
            this.model = model;
            this.combatLogic = combatLogic;
            this.movementLogic = movementLogic;
            this.animationLogic = animationLogic;
            this.pathfindingLogic = pathfindingLogic;
        }

        // Run before rendering
        public void Step()
        {
            combatLogic.SetTarget();
            movementLogic.UpdatePositions();
            animationLogic.UpdateSprites();
        }
    }
}
