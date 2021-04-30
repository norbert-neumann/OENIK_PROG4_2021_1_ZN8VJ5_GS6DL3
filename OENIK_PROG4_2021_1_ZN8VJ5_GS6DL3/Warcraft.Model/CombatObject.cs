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
                this.health -= amount;
            }
            else
            {
                if (this.shield >= amount)
                {
                    this.shield -= amount;
                }
                else
                {
                    this.health -= amount - this.shield;
                    this.shield = 0;
                }
            }

            return this.health <= 0;
        }
    }
}
