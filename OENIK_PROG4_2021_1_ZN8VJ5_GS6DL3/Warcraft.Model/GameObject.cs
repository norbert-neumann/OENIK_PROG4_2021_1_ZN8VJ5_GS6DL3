namespace Warcraft.Model
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
        public Rectangle hitbox;

        public Point Position
        { 
            get
            {
                return new Point(hitbox.X + (hitbox.Width / 2), hitbox.Y + (hitbox.Height / 2));
            }
            set
            {
                Point newPos = (Point)value;
                this.hitbox = new Rectangle(newPos.X + (hitbox.Width / 2), newPos.Y + (hitbox.Height / 2), hitbox.Width, hitbox.Height);
            }
        }

        /// <summary>
        /// Sets the object's position to a new one.
        /// </summary>
        /// <param name="newX">New X coordinate.</param>
        /// <param name="newY">New Y coordinate.</param>
        public void SetPosition(Point newPosition)
        {
            Position = newPosition;
        }

        /// <summary>
        /// Changes the object's position by a given amount.
        /// </summary>
        /// <param name="dx">Change in x coordinate.</param>
        /// <param name="dy">Change in y coordinate.</param>
        public void ChangePosition(Point delta)
        {
            this.hitbox.Offset(delta);
        }

        public double Distance(GameObject other)
        {
            return Math.Sqrt(Math.Pow(this.Position.X - other.Position.X, 2) + Math.Pow(this.Position.Y - other.Position.Y, 2));
        }

        public bool Collides(GameObject other)
        {
            return this.hitbox.IntersectsWith(other.hitbox);
        }

        public bool IsPositionInHitbox(GameObject other)
        {
            return this.Position.X >= other.hitbox.X && this.Position.X <= other.hitbox.Right &&
                this.Position.Y >= other.hitbox.Y && this.Position.Y <= other.hitbox.Bottom;
        }
    }
}
