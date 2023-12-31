﻿namespace Warcraft.Model
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
    /// Abstract game object class: each object that is rendered has to have a position and a way to detect collisons and
    /// we have to change the positon somehow. This class implements this functionality.
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// Object used for collison detection.
        /// </summary>
        public Rectangle Hitbox;

        /// <summary>
        /// Position of the game object. This marks the center of the hitbox so we need to write the getter and setter accordingly.
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(this.Hitbox.X + (this.Hitbox.Width / 2), this.Hitbox.Y + (this.Hitbox.Height / 2));
            }

            set
            {
                Point newPos = (Point)value;
                this.Hitbox.X = newPos.X;
                this.Hitbox.Y = newPos.Y;
                this.Hitbox = new Rectangle(newPos.X - (this.Hitbox.Width / 2), newPos.Y - (this.Hitbox.Height / 2), this.Hitbox.Width, this.Hitbox.Height);
            }
        }

        /// <summary>
        /// Sets the object's position to a new one.
        /// </summary>
        /// <param name="newPosition">The game object's new position.</param>
        public void SetPosition(Point newPosition)
        {
            this.Position = newPosition;
        }

        /// <summary>
        /// Sets the object's position.
        /// </summary>
        /// <param name="point">New position.</param>
        public void SetCenterPositon(Point point)
        {
            this.Position = new Point(point.X - (this.Hitbox.Width / 1), point.Y - (this.Hitbox.Height / 1));
        }

        /// <summary>
        /// Changes the object's position by a given amount.
        /// </summary>
        /// <param name="delta">Point stroring the XY displacements.</param>
        public void ChangePosition(Point delta)
        {
            this.Hitbox.Offset(delta);
        }

        /// <summary>
        /// Measures the distance between this and an other gameobject.
        /// </summary>
        /// <param name="other">Other GameObject.</param>
        /// <returns>Euclidean distance.</returns>
        public double Distance(GameObject other)
        {
            return Math.Sqrt(Math.Pow(this.Position.X - other.Position.X, 2) + Math.Pow(this.Position.Y - other.Position.Y, 2));
        }

        /// <summary>
        /// Collicion detection between this and an other object.
        /// </summary>
        /// <param name="other">Other GameObject.</param>
        /// <returns>True if the objects collide.</returns>
        public bool Collides(GameObject other)
        {
            return this.Hitbox.IntersectsWith(other.Hitbox);
        }

        /// <summary>
        /// Checks if this object's position (which is the hitbox's center) is inside of an other hitbox.
        /// </summary>
        /// <param name="other">Other GameObject.</param>
        /// <returns>True if this object's position is inside of an other hitbox.</returns>
        public bool IsPositionInHitbox(GameObject other)
        {
            return this.Position.X >= other.Hitbox.X && this.Position.X <= other.Hitbox.Right &&
                this.Position.Y >= other.Hitbox.Y && this.Position.Y <= other.Hitbox.Bottom;
        }
    }
}
