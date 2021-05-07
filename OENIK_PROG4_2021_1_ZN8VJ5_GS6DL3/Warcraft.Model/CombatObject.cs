namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// "Fighting" entity. Has attack damage, shield and health.
    /// </summary>
    public class CombatObject : GameObject
    {
        /// <summary>
        /// Race of the unit.
        /// </summary>
        public RaceEnum Race;

        /// <summary>
        /// Unit's shield.
        /// </summary>
        protected int shield = 0;

        /// <summary>
        /// Unit's current health.
        /// </summary>
        protected int health = 100;

        /// <summary>
        /// Unit's maximum health.
        /// </summary>
        protected int maxHealth;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatObject"/> class.
        /// </summary>
        /// <param name="x">X position of the hitbox.</param>
        /// <param name="y">Y positon of the hitbox.</param>
        /// <param name="w">Width of the hitbox.</param>
        /// <param name="h">Height of the hitbox.</param>
        public CombatObject(int x, int y, int w, int h)
        {
            this.Hitbox = new System.Drawing.Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatObject"/> class.
        /// </summary>
        /// <param name="x">X position of the hitbox.</param>
        /// <param name="y">Y positon of the hitbox.</param>
        /// <param name="w">Width of the hitbox.</param>
        /// <param name="h">Height of the hitbox.</param>
        /// <param name="healt">Initial health.</param>
        public CombatObject(int x, int y, int w, int h, int healt)
            : this(x, y, w, h)
        {
            this.maxHealth = healt;
            this.Health = healt;
        }

        /// <summary>
        /// Enum indicating this game object's owner.
        /// </summary>
        public OwnerEnum Owner { get; set; }

        /// <summary>
        /// Takes damage and signals if the damage taken was fatal.
        /// </summary>
        /// <param name="amount">Amount of damage taken.</param>
        /// <returns>True if the damage taken was fatal.</returns>
        public bool AcceptDamage(int amount)
        {
            if (this.shield == 0)
            {
                this.Health -= amount;
            }
            else
            {
                if (this.shield >= amount)
                {
                    this.shield -= amount;
                }
                else
                {
                    this.Health -= amount - this.shield;
                    this.shield = 0;
                }
            }

            return this.Health <= 0;
        }

        /// <summary>
        /// This will be only used when the player clicks on a tree.
        /// In that case we need to show it's current and max capacity.
        /// </summary>
        /// <returns>String showing the tree's current and max capacity.</returns>
        public override string ToString()
        {
            return string.Format($"Capacity : {this.Health}/{this.maxHealth}");
        }
    }
}
