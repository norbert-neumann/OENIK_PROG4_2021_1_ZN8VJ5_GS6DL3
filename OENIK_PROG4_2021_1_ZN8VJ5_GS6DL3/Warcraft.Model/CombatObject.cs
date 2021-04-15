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
    public abstract class CombatObject : GameObject
    {
        /// <summary>
        /// Unit's shield.
        /// </summary>
        protected int shield;

        /// <summary>
        /// Unit's current health.
        /// </summary>
        protected int health;

        /// <summary>
        /// Unit's maximum health.
        /// </summary>
        protected int maxHealth;

        /// <summary>
        /// Race of the unit.
        /// </summary>
        public RaceEnum Race;

        public OwnerEnum Owner { get; set; }

        /// <summary>
        /// Takes damage and signals if the damage taken was fatal.
        /// </summary>
        /// <param name="amount">Amount of damage taken.</param>
        /// <returns>True if the damage taken was fatal.</returns>
        public bool AcceptDamage(int amount)
        {
            this.health -= amount - this.shield;
            return this.health <= 0;
        }
    }
}
