namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Enemy controller.
    /// </summary>
    public class EnemyLogic
    {
        private MovementLogic movementLogic;
        private GameModel model;
        private UnitFactory factory;
        private Random rnd = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyLogic"/> class.
        /// </summary>
        /// <param name="model">Model to operate on.</param>
        /// <param name="movementLogic">Shared movementlogic with the CoreLogic.</param>
        public EnemyLogic(GameModel model, MovementLogic movementLogic)
        {
            this.model = model;
            this.model.EnemyGold = 150;
            this.model.EnemyLumber = 50;
            this.factory = new UnitFactory(this.model, 100, 5, 1);
            this.movementLogic = movementLogic;
        }

        /// <summary>
        /// Updates all enemy units.
        /// </summary>
        public void Step()
        {
            int unitGoldCost = 50;
            int unitLumberCost = 20;

            if (this.model.EnemyGold >= unitGoldCost && this.model.EnemyLumber >= unitLumberCost)
            {
                this.factory.Create("enemy orc peasant", 0, 0);
                this.model.EnemyGold -= unitGoldCost;
                this.model.EnemyLumber -= unitLumberCost;
            }

            if (this.model.GameTime.Elapsed.Minutes >= 10)
            {
                foreach (Unit unit in this.model.Units)
                {
                    if (unit.Owner == OwnerEnum.ENEMY && unit.InIdle)
                    {
                        // Assign random task
                        switch (this.rnd.Next(0, 4))
                        {
                            case 0: this.MineClosestGoldMine(unit); break;
                            case 1: this.MineClosestLumberMine(unit); break;
                            case 2: this.AttackClosestPlayer(unit); break;
                            case 3: this.AttackEnemyHall(unit); break;
                        }
                    }
                }
            }
            else if (this.model.GameTime.Elapsed.Minutes >= 5)
            {
                foreach (Unit unit in this.model.Units)
                {
                    if (unit.Owner == OwnerEnum.ENEMY && unit.InIdle)
                    {
                        // Assign random task
                        switch (this.rnd.Next(0, 3))
                        {
                            case 0: this.MineClosestGoldMine(unit); break;
                            case 1: this.MineClosestLumberMine(unit); break;
                            case 2: this.AttackClosestPlayer(unit); break;
                        }
                    }
                }
            }
            else
            {
                foreach (Unit unit in this.model.Units)
                {
                    if (unit.Owner == OwnerEnum.ENEMY && unit.InIdle)
                    {
                        // Assign random task
                        switch (this.rnd.Next(0, 2))
                        {
                            case 0: this.MineClosestGoldMine(unit); break;
                            case 1: this.MineClosestLumberMine(unit); break;
                        }
                    }
                }
            }
            else
            {
                foreach (Unit unit in this.model.Units)
                {
                    if (unit.Owner == OwnerEnum.ENEMY && unit.InIdle)
                    {
                        // Assign random task
                        switch (this.rnd.Next(0, 3))
                        {
                            case 0: this.MineClosestGoldMine(unit); break;
                            case 1: this.MineClosestLumberMine(unit); break;
                            case 2: this.PatrollAroundHall(unit); break;
                        }
                    }
                }
            }
        }

        private void PatrollAroundHall(Unit unit)
        {
            if (this.movementLogic.Routines.ContainsKey(unit))
            {
                this.movementLogic.Routines.Remove(unit);
            }

            Point bottomLeft = new Point(this.model.EnemyHall.Hitbox.X, this.model.EnemyHall.Hitbox.Y);
            bottomLeft.Offset(0, this.model.EnemyHall.Hitbox.Height + Config.BorderWidth);

            Point topRight = new Point(this.model.EnemyHall.Hitbox.X, this.model.EnemyHall.Hitbox.Y);
            topRight.Offset(this.model.EnemyHall.Hitbox.Width + Config.BorderWidth, 0);

            unit.InIdle = false;
            this.movementLogic.Routines.Add(unit, new PatrolRoutine(unit, bottomLeft, topRight));
        }

        private void MineClosestGoldMine(Unit unit)
        {
            if (this.movementLogic.Routines.ContainsKey(unit))
            {
                this.movementLogic.Routines.Remove(unit);
            }

            GoldMine mine = this.model.GetClosestGoldMine(unit);

            if (mine != null)
            {
                unit.InIdle = false;
                this.movementLogic.Routines.Add(unit, new GoldMiningRoutine(unit, TimeSpan.FromSeconds(3), this.model.EnemyHall, mine, this.model, OwnerEnum.ENEMY));
            }
        }

        private void MineClosestLumberMine(Unit unit)
        {
            if (this.movementLogic.Routines.ContainsKey(unit))
            {
                this.movementLogic.Routines.Remove(unit);
            }

            CombatObject mine = this.model.GetClosestLumberMine(unit);

            if (mine != null)
            {
                unit.InIdle = false;
                this.movementLogic.Routines.Add(unit, new HarvestLumberRoutine(unit, TimeSpan.FromSeconds(3), this.model.EnemyHall, mine, this.model, OwnerEnum.ENEMY));
            }
        }

        private void AttackClosestPlayer(Unit unit)
        {
            if (this.movementLogic.Routines.ContainsKey(unit))
            {
                this.movementLogic.Routines.Remove(unit);
            }

            CombatObject toAttack = this.model.GetClosestPlayerUnit(this.model.EnemyHall);

            if (toAttack != null)
            {
                unit.InIdle = false;
                unit.Enemy = toAttack;
            }
        }

        private void AttackEnemyHall(Unit unit)
        {
            if (this.movementLogic.Routines.ContainsKey(unit))
            {
                this.movementLogic.Routines.Remove(unit);
            }

            unit.InIdle = false;
            unit.Enemy = this.model.PlayerHall;
        }
    }
}
