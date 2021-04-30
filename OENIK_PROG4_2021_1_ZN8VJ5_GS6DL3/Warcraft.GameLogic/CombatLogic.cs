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
        private GameModel model;

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
            foreach (Unit unit in this.model.Units)
            {
                if (unit.Enemy != null)
                {
                    if ((unit.Enemy as Building) != null)
                    {
                        if (unit.IsPositionInHitbox(unit.Enemy))
                        {
                            this.AttackEnemyBuilding(unit);
                        }
                        else
                        {
                            unit.UnitState = UnitStateEnum.Walking;
                            unit.Target = unit.Enemy.Position;
                        }
                    }
                    else
                    {
                        if (unit.Distance(unit.Enemy) <= unit.Range)
                        {
                            this.AttackEnemyUnit(unit);
                        }
                        else
                        {
                            unit.UnitState = UnitStateEnum.Walking;
                            unit.Target = unit.Enemy.Position;
                        }
                    }
                }
                else
                {
                    CombatObject enemy = this.FindEnemyInRange(unit);
                    if (enemy != null)
                    {
                        unit.Enemy = enemy;
                        unit.Target = enemy.Position;
                        unit.InIdle = false;
                    }
                }
            }
        }

        private void AttackEnemyUnit(Unit unit)
        {
            unit.UnitState = UnitStateEnum.Fighting;
            if (unit.Enemy.AcceptDamage(unit.Attack))
            {
                this.model.UnitsToRemove.Add(unit.Enemy as Unit);
                this.ResetUnit(unit);
            }
        }

        private void AttackEnemyBuilding(Unit unit)
        {
            unit.UnitState = UnitStateEnum.Fighting;
            if (unit.Enemy.AcceptDamage(unit.Attack))
            {
                this.model.BuildingsToRemove.Add(unit.Enemy as Building);
                this.ResetUnit(unit);
            }
        }

        private void ResetUnit(Unit unit)
        {
            unit.Enemy = null;
            unit.UnitState = UnitStateEnum.Walking;
        }

        private void HarvestLumber(Unit unit)
        {
            unit.UnitState = UnitStateEnum.Fighting;
            if (unit.Enemy.AcceptDamage(unit.Attack))
            {
                this.model.TreesToRemove.Add(unit.Enemy as CombatObject);
            }
        }

        private CombatObject FindEnemyInRange(Unit self)
        {
            foreach (Unit other in this.model.Units)
            {
                if (self.Owner != other.Owner && self.Distance(other) <= Config.AggroRange)
                {
                    return other;
                }
            }

            foreach (Building buildng in this.model.Buildings)
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
