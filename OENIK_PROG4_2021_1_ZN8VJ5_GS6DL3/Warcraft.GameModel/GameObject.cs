namespace Warcraft
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Abstract game object class: each object that is rendered has to have a position nad
    /// the we have to change the positon somehow. This class implements this functionality.
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// X coordinate of the object.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate of the object.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Sets the object's position to a new one.
        /// </summary>
        /// <param name="newX">New X coordinate.</param>
        /// <param name="newY">New Y coordinate.</param>
        public void SetPosition(int newX, int newY)
        {
            this.X = newX;
            this.Y = newY;
        }

        /// <summary>
        /// Changes the object's position by a given amount.
        /// </summary>
        /// <param name="dx">Change in x coordinate.</param>
        /// <param name="dy">Change in y coordinate.</param>
        public void ChangePosition(int dx, int dy)
        {
            this.X += dx;
            this.Y += dy;
        }
    }
}
