namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Moves a unit from A to B.
    /// </summary>
    public class PatrolRoutine : Routine
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PatrolRoutine"/> class.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        /// <param name="TargetA">A.</param>
        /// <param name="TargetB">B.</param>
        public PatrolRoutine(Unit unit, Point TargetA, Point TargetB)
            : base(unit, TargetA, TargetB)
        {
        }

        /// <summary>
        /// Updates the unit's state and target.
        /// </summary>
        /// <returns>Returns true if the routine is active.</returns>
        public override bool Update()
        {
            if (this.PointToPointDistance(this.unit.Position, this.TargetA) <= 3)
            {
                return this.ReachedTargetA();
            }
            else if (this.PointToPointDistance(this.unit.Position, this.TargetB) <= 3)
            {
                return this.ReachedTargetB();
            }
            else
            {
                this.unit.path.Enqueue(this.TargetB);
            }

            return false;
        }

        /// <summary>
        /// Commands the unit to the other point.
        /// </summary>
        /// <returns>Always false.</returns>
        protected override bool ReachedTargetA()
        {
            this.unit.path.Enqueue(this.TargetB);
            return false;
        }

        /// <summary>
        /// Commands the unit to the other point.
        /// </summary>
        /// <returns>Always false.</returns>
        protected override bool ReachedTargetB()
        {
            this.unit.path.Enqueue(this.TargetA);
            return false;
        }
    }
}
