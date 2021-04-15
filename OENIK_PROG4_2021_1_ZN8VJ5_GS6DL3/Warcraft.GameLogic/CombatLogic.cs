using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Warcraft.Model;

namespace Warcraft.GameLogic
{
    public class CombatLogic
    {
        public GameModel model;

        public CombatLogic(GameModel model)
        {
            this.model = model;
        }

        // Check if agressive object reached it's enemy. If not set target. If yes flip UnitStateEnum
        public void SetTarget()
        {
            foreach (Unit unit in model.playerUnits)
            {
                if (unit.enemy != null)
                {
                    if ((unit.enemy as Building) != null)
                    {
                        if (unit.IsPositionInHitbox(unit.enemy))
                        {
                            unit.UnitState = UnitStateEnum.Fighting;
                        }
                        else
                        {
                            unit.UnitState = UnitStateEnum.Walking;
                            unit.target = unit.enemy.Position;
                        }
                    }
                    else
                    {
                        if (unit.Distance(unit.enemy) <= unit.range)
                        {
                            unit.UnitState = UnitStateEnum.Fighting;
                        }
                        else
                        {
                            unit.UnitState = UnitStateEnum.Walking;
                            unit.target = unit.enemy.Position;
                        }
                    }
                }
                else
                {
                    Unit enemy = FindEnemyInRange(unit, model.enemyUnits);
                    if (enemy != null)
                    {
                        // We set the target both ways so no need to search for enemies in the next loop
                        unit.enemy = enemy;
                        enemy.enemy = unit;
                    }
                }
            }

            foreach (Unit unit in model.enemyUnits)
            {
                if (unit.enemy != null)
                {
                    if ((unit.enemy as Building) != null)
                    {
                        if (unit.IsPositionInHitbox(unit.enemy))
                        {
                            unit.UnitState = UnitStateEnum.Fighting;
                        }
                        else
                        {
                            unit.UnitState = UnitStateEnum.Walking;
                            unit.target = unit.enemy.Position;
                        }
                    }
                    else
                    {
                        if (unit.Distance(unit.enemy) <= unit.range)
                        {
                            unit.UnitState = UnitStateEnum.Fighting;
                        }
                        else
                        {
                            unit.UnitState = UnitStateEnum.Walking;
                            unit.target = unit.enemy.Position;
                        }
                    }
                }
            }
        }

        private Unit FindEnemyInRange(Unit self, List<Unit> enemies)
        {
            foreach (Unit enemy in enemies)
            {
                if (self.Distance(enemy) <= Config.AggroRange)
                {
                    return enemy;
                }
            }
            return null;
        }
    }
}
