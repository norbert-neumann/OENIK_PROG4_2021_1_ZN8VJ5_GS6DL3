namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Drawing;

    /// <summary>
    /// Represents a building. Some unit can repair buildings nad heal them. This class extends CombatObject with this functionality.
    /// </summary>
    public class Building : CombatObject
    {
        public BuildingEnum type;
        public string animationString;

        public Building(BuildingEnum type, RaceEnum race, int x, int y, int width, int height)
        {
            this.type = type;
            this.Race = race;
            this.hitbox = new Rectangle(x, y, width, height);

            switch (race)
            {
                case RaceEnum.Human: animationString += "H";
                    break;
                case RaceEnum.Orc: animationString += "O";
                    break;
                default:
                    break;
            }

            switch (type)
            {
                case BuildingEnum.Hall: animationString += "H";
                    break;
                case BuildingEnum.Farm: animationString += "F";
                    break;
                case BuildingEnum.Barracks: animationString += "B";
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
            this.health += amount;

            if (this.health >= this.maxHealth)
            {
                this.health = this.maxHealth;
            }
        }

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

            switch (type)
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
