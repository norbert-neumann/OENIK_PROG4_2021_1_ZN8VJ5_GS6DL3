namespace Warcraft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a building. Some unit can repair buildings nad heal them. This class extends CombatObject with this functionality.
    /// </summary>
    public class Building : CombatObject
    {
        /// <summary>
        /// Restores some amount of HP to the building.
        /// </summary>
        /// <param name="amount">Heal quantity.</param>
        public void AcceptHeal(int amount)
        {
            this.health += amount;

            if (this.health >= this.maxHealth)
            {
                this.health = this.maxHealth;
            }
        }
    }
}
