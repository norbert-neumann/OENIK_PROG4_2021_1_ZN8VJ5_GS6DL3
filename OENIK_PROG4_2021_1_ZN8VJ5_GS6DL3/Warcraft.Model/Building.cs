namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a building. Some unit can repair buildings nad heal them. This class extends CombatObject with this functionality.
    /// </summary>
    public class Building : CombatObject
    {
        /// <summary>
        /// This string tells the renderer what to display.
        /// </summary>
        public string AnimationString;

        /// <summary>
        /// Building type.
        /// </summary>
        public BuildingEnum Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        /// <param name="owner">Owner of the building.</param>
        /// <param name="type">Building type.</param>
        /// <param name="race">Race of which this building belongs to.</param>
        /// <param name="x">X position of building.</param>
        /// <param name="y">Y position of building.</param>
        /// <param name="width">Width of the building's hitbox.</param>
        /// <param name="height">Height of the building's hitbox.</param>
        public Building(OwnerEnum owner, BuildingEnum type, RaceEnum race, int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            this.Owner = owner;
            this.Type = type;
            this.Race = race;

            switch (race)
            {
                case RaceEnum.Human: this.AnimationString += "H";
                    break;
                case RaceEnum.Orc:
                    this.AnimationString += "O";
                    break;
                default:
                    break;
            }

            switch (type)
            {
                case BuildingEnum.Hall:
                    this.AnimationString += "H";
                    break;
                case BuildingEnum.Farm:
                    this.AnimationString += "F";
                    break;
                case BuildingEnum.Barracks:
                    this.AnimationString += "B";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Restores some amount of HP to the building.
        /// </summary>
        /// <param name="amount">Heal quantity.</param>
        public void AcceptHeal(int amount)
        {
            this.Health += amount;

            if (this.Health >= this.maxHealth)
            {
                this.Health = this.maxHealth;
            }
        }

        // NOT USED.

        /// <summary>
        /// Returns the state string that is used for getting the proper sprite.
        /// </summary>
        /// <returns>The building's state string.</returns>
        public string StateString()
        {
            StringBuilder sb = new StringBuilder();

            switch (this.Race)
            {
                case RaceEnum.Human:
                    sb.Append("H");
                    break;
                case RaceEnum.Orc:
                    sb.Append("O");
                    break;
                default:
                    break;
            }

            switch (this.Type)
            {
                case BuildingEnum.Hall:
                    sb.Append("H");
                    break;
                case BuildingEnum.Farm:
                    sb.Append("F");
                    break;
                case BuildingEnum.Barracks:
                    sb.Append("B");
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }
    }
}
