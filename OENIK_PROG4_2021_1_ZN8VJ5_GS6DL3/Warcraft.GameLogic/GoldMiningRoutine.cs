namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Moves to gold mine, mines, then returns home.
    /// </summary>
    public class GoldMiningRoutine : Routine
    {
        private DateTime startTime;
        private TimeSpan waitTime;
        private bool mining = false;

        /// <summary>
        /// Return gold to this object.
        /// </summary>
        public Building hall;

        /// <summary>
        /// Gold mine to mine in.
        /// </summary>
        public GoldMine mine;

        /// <summary>
        /// The current target object (hall or gold mine).
        /// </summary>
        public GameObject targetObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoldMiningRoutine"/> class.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        /// <param name="waitTime">Number of seconds to mine.</param>
        /// <param name="hall">Hall.</param>
        /// <param name="mine">Gold mine.</param>
        public GoldMiningRoutine(Unit unit, TimeSpan waitTime, Building hall, GoldMine mine)
            : base(unit, hall.Position, mine.Position)
        {
            this.unit = unit;
            this.startTime = startTime;
            this.waitTime = waitTime;
            this.hall = hall;
            this.mine = mine;
            this.targetObject = mine;
        }

        /// <summary>
        /// Updates the unit's state and target.
        /// </summary>
        /// <returns>Returns true if the routine is active.</returns>
        public override bool Update()
        {
            if (this.unit.Collides(this.hall))
            {
                return this.ReachedTargetA();
            }
            else if (this.unit.Collides(this.mine))
            {
                return this.ReachedTargetB();
            }
            else
            {
                this.unit.target = this.TargetB;
            }

            return false;
        }

        /// <summary>
        /// Commands the unit to mine.
        /// </summary>
        /// <returns>False always.</returns>
        protected override bool ReachedTargetA()
        {
            this.unit.UnitState = UnitStateEnum.Walking;
            this.targetObject = this.mine;
            this.unit.target = this.TargetB;
            return false;
        }

        /// <summary>
        /// Checks if the mining is over or not. If it's over commands the unit to hall.
        /// Else kepps minig.
        /// </summary>
        /// <returns>True if the unit is minig.</returns>
        protected override bool ReachedTargetB()
        {
            if (this.mining)
            {
                if (DateTime.Now - this.startTime >= this.waitTime)
                {
                    this.unit.UnitState = UnitStateEnum.WalkingWithGold;
                    this.unit.target = this.TargetA;
                    this.mining = false;
                    this.unit.Position = new Point(this.unit.entryPoint.X - (this.unit.hitbox.Width / 1), this.unit.entryPoint.Y - (this.unit.hitbox.Height / 1));
                    this.unit.hiding = false;
                    this.targetObject = this.hall;
                    this.mine.NumberOfUsers--;
                    this.unit.animationIndex = 0;
                    return false;
                }

                return true;
            }
            else
            {
                this.mine.NumberOfUsers++;
                this.startTime = DateTime.Now;
                this.mining = true;
                this.unit.hiding = true;
                return true;
            }
        }
    }
}
