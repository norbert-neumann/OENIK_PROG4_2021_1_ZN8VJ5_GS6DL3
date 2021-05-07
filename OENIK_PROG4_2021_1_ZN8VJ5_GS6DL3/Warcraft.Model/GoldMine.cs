namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a mine, which stores basic amount of resources.
    /// Units can take from the mine.
    /// </summary>
    public class GoldMine : GameObject
    {
        /// <summary>
        /// Number of units using this mine.
        /// </summary>
        public int NumberOfUsers;

        /// <summary>
        /// Starting capacity of the mine.
        /// </summary>
        private int baseCapacity;   // TODO: Replace placeholder with Config...

        /// <summary>
        /// Initializes a new instance of the <see cref="GoldMine"/> class.
        /// </summary>
        /// <param name="x">Hitbox X pos.</param>
        /// <param name="y">Hitbox Y pos.</param>
        /// <param name="w">Hitbox width.</param>
        /// <param name="h">Hitbox's height.</param>
        /// <param name="capacity">Mine's capcaity.</param>
        public GoldMine(int x, int y, int w, int h, int capacity)
        {
            this.Hitbox = new Rectangle(x, y, w, h);
            this.baseCapacity = capacity;
            this.CurrentCapacity = capacity;
        }

        /// <summary>
        /// Determines if the mine is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.CurrentCapacity > 0; }
        }

        /// <summary>
        /// String telling the renderer which sprite to display.
        /// </summary>
        public string AnimationString
        {
            get { return this.NumberOfUsers == 0 ? "IM" : "AM"; }
        }

        /// <summary>
        /// Current capacity of the mine.
        /// </summary>
        private int CurrentCapacity { get; set; }

        /// <summary>
        /// Takes a certain amount of resorces out of the mine.
        /// </summary>
        /// <param name="amount">Amount of resources taken.</param>
        /// <returns>The amount of resources acutally taken.</returns>
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
            return this.CurrentCapacity > 0;
        }

        /// <summary>
        /// This will be shown on the HUD.
        /// </summary>
        /// <returns>The mine's current and max capacity.</returns>
        public override string ToString()
        {
            return string.Format($"Capacity : {this.CurrentCapacity}/{this.baseCapacity}");
        }
    }
}
