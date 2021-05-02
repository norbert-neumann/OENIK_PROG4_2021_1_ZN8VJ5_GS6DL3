namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a mine, which stores basic amount of resources.
    /// Units can take from the mine.
    /// </summary>
    public class Mine : GameObject
    {
        /// <summary>
        /// Starting capacity of the mine.
        /// </summary>
        public const int BaseCapacity = 100;    // TODO: Replace placeholder with Config...

        /// <summary>
        /// Current capacity of the mine.
        /// </summary>
        public int CurrentCapacity { get; set; }

        /// <summary>
        /// Determines if the mine is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.CurrentCapacity > 0; }
        }

        /// <summary>
        /// Takes a certain amount of resorces out of the mine.
        /// </summary>
        /// <param name="amount">Amount of resources taken.</param>
        public int Take(int amount)
        {
            if (this.CurrentCapacity >= amount)
            {
                this.CurrentCapacity -= amount;
            }
            else
            {
                amount = this.CurrentCapacity;
                this.CurrentCapacity = 0;
            }

            return amount;
        }
    }
}
