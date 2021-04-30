namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using Warcraft.Model;

    /// <summary>
    /// Deals with everything realted to combat.
    /// </summary>
    public class CombatLogic
    {
        /// <summary>
        /// Game model to operate on.
        /// </summary>
        public GameModel model;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatLogic"/> class.
        /// </summary>
        /// <param name="model">Game model to operate on.</param>
        public CombatLogic(GameModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Check if agressive object reached it's enemy. If not set target. If yes flip UnitStateEnum.
        /// </summary>
        public void SetTarget()
        {
            foreach (Unit unit in this.model.units)
            {
                if (unit.enemy != null)
                {
                    if ((unit.enemy as Building) != null)
                    {
                        if (unit.IsPositionInHitbox(unit.enemy))
                        {
                            this.AttackEnemyBuilding(unit);
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
                            this.AttackEnemyUnit(unit);
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
                    CombatObject enemy = this.FindEnemyInRange(unit);
                    if (enemy != null)
                    {
                        unit.enemy = enemy;
                        unit.target = enemy.Position;
                        unit.inIdle = false;
                    }
                }
            }
        }

        private void AttackEnemyUnit(Unit unit)
        {
            unit.UnitState = UnitStateEnum.Fighting;
            if (unit.enemy.AcceptDamage(unit.attack))
            {
                this.model.unitsToRemove.Add(unit.enemy as Unit);
                this.ResetUnit(unit);
            }
        }

        private void AttackEnemyBuilding(Unit unit)
        {
            unit.UnitState = UnitStateEnum.Fighting;
            if (unit.enemy.AcceptDamage(unit.attack))
            {
                this.model.buildingsToRemove.Add(unit.enemy as Building);
                this.ResetUnit(unit);
            }
        }

        private void ResetUnit(Unit unit)
        {
            unit.enemy = null;
            unit.UnitState = UnitStateEnum.Walking;
        }

        private void HarvestLumber(Unit unit)
        {
            unit.UnitState = UnitStateEnum.Fighting;
            if (unit.enemy.AcceptDamage(unit.attack))
            {
                this.model.treesToRemove.Add(unit.enemy as CombatObject);
            }
        }

        private CombatObject FindEnemyInRange(Unit self)
        {
            foreach (Unit other in this.model.units)
            {
                if (self.Owner != other.Owner && self.Distance(other) <= Config.AggroRange)
                {
                    return other;
                }
            }

            foreach (Building buildng in this.model.buildings)
            {
                if (self.Owner != buildng.Owner && self.Distance(buildng) <= Config.AggroRange)
                {
                    return buildng;
                }
            }

            return null;
        }
    }
}
