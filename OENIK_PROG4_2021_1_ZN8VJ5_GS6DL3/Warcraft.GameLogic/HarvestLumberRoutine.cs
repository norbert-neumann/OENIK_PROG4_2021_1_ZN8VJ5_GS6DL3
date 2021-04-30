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
        /// <summary>
        /// The current target object (hall or gold mine).
        /// </summary>
        public GameObject TargetObject;

        // Hall -> TargetA
        // Mine -> TargetB
        private DateTime startTime;
        private TimeSpan waitTime;
        private bool mining = false;

        /// <summary>
        /// Return gold to this object.
        /// </summary>
        private Building hall;

        /// <summary>
        /// Tree to harvest.
        /// </summary>
        private CombatObject tree;

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
            this.TargetObject = tree;
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
                this.unit.Target = this.targetB;
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
            this.unit.Target = this.targetB;
            this.TargetObject = this.tree;
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
                    this.unit.Target = this.targetA;
                    this.mining = false;
                    this.unit.Position = new Point(this.unit.EntryPoint.X - (this.unit.Hitbox.Width / 1), this.unit.EntryPoint.Y - (this.unit.Hitbox.Height / 1));
                    this.TargetObject = this.hall;
                    return false;
                }

                return true;
            }
            else
            {
                this.startTime = DateTime.Now;
                this.mining = true;
                this.unit.UnitState = UnitStateEnum.Fighting;
                this.unit.Target = this.targetB;
                return true;
            }
        }
    }
}
