namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Abstract routine class.
    /// </summary>
    public abstract class Routine
    {
        /// <summary>
        /// Unit to operate on.
        /// </summary>
        protected Unit unit;

        /// <summary>
        /// Point A to move to.
        /// </summary>
        protected Point targetA;

        /// <summary>
        /// Point B to move to.
        /// </summary>
        protected Point targetB;

        /// <summary>
        /// Initializes a new instance of the <see cref="Routine"/> class.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        /// <param name="targetA">Point A to move to.</param>
        /// <param name="targetB">Point B to move to.</param>
        protected Routine(Unit unit, Point targetA, Point targetB)
        {
            this.unit = unit;
            this.targetA = targetA;
            this.targetB = targetB;
        }

        /// <summary>
        /// Updates the unit's state and target.
        /// </summary>
        /// <returns>Returns true if the routine is active.</returns>
        public abstract bool Update();

        /// <summary>
        /// Logic that will run if the unit has reached A.
        /// </summary>
        /// <returns>Bool indicating whether the routine is active.</returns>
        protected abstract bool ReachedTargetA();

        /// <summary>
        /// Logic that will run if the unit has reached B.
        /// </summary>
        /// <returns>Bool indicating whether the routine is active.</returns>
        protected abstract bool ReachedTargetB();

        /// <summary>
        /// Computes to euclidean distance between to points.
        /// </summary>
        /// <param name="a">Point A.</param>
        /// <param name="b">Point B.</param>
        /// <returns>A-B distance.</returns>
        protected double PointToPointDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
