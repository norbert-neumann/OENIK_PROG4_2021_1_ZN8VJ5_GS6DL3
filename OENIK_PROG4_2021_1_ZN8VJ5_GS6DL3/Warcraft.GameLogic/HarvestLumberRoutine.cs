namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Controls a unit to move to a tree, harvest it, then return to Hall.
    /// </summary>
    public class HarvestLumberRoutine : Routine
    {
        // Hall -> TargetA
        // Mine -> TargetB
        private DateTime startTime;
        private TimeSpan waitTime;
        private bool mining = false;

        /// <summary>
        /// Return gold to this object.
        /// </summary>
        public Building hall;

        /// <summary>
        /// Tree to harvest.
        /// </summary>
        public CombatObject tree;

        /// <summary>
        /// The current target object (hall or gold mine).
        /// </summary>
        public GameObject targetObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="HarvestLumberRoutine"/> class.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        /// <param name="waitTime">Number of seconds to mine.</param>
        /// <param name="hall">Hall.</param>
        /// <param name="tree">Treee.</param>
        public HarvestLumberRoutine(Unit unit, TimeSpan waitTime, Building hall, CombatObject tree)
            : base(unit, hall.Position, tree.Position)
        {
            this.waitTime = waitTime;
            this.hall = hall;
            this.tree = tree;
            this.targetObject = tree;
        }

        /// <summary>
        /// Updates the unit's state and target.
        /// </summary>
        /// <returns>Returns true if the routine is active.</returns>
        public override bool Update()
        {
            if (this.unit.IsPositionInHitbox(this.hall))
            {
                return this.ReachedTargetA();
            }
            else if (this.unit.IsPositionInHitbox(this.tree))
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
            this.unit.target = TargetB;
            this.targetObject = this.tree;
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
                    this.unit.UnitState = UnitStateEnum.WalkingWithLumber;
                    this.unit.target = this.TargetA;
                    this.mining = false;
                    this.unit.Position = new Point(this.unit.entryPoint.X - (this.unit.hitbox.Width / 1), this.unit.entryPoint.Y - (this.unit.hitbox.Height / 1));
                    this.targetObject = this.hall;
                    return false;
                }

                return true;
            }
            else
            {
                this.startTime = DateTime.Now;
                this.mining = true;
                this.unit.UnitState = UnitStateEnum.Fighting;
                this.unit.target = this.TargetB;
                return true;
            }
        }
    }
}
