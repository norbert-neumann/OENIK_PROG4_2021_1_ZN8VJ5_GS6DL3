namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Creates a specific unit.
    /// </summary>
    public class UnitFactory
    {
        private GameModel model;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitFactory"/> class.
        /// </summary>
        /// <param name="model">Game model to operate on.</param>
        public UnitFactory(GameModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Creates a specific unit based on the following params.
        /// </summary>
        /// <param name="description">Tpye and owner of the unit.</param>
        /// <param name="x">X pos.</param>
        /// <param name="y">Y pos.</param>
        /// <returns>The newly created unit.</returns>
        public Unit Create(string description, int x, int y)
        {
            string[] components = description.ToUpper().Split(' ');
            OwnerEnum owner;
            RaceEnum race;
            UnitTypeEnum unitType;

            switch (components[0])
            {
                case "PLAYER": owner = OwnerEnum.PLAYER; break;
                case "ENEMY": owner = OwnerEnum.ENEMY; break;
                default: throw new ArgumentException();
            }

            switch (components[1])
            {
                case "HUMAN": race = RaceEnum.Human; break;
                case "ORC": race = RaceEnum.Orc; break;
                default: throw new ArgumentException();
            }

            switch (components[2])
            {
                case "PEASANT": unitType = UnitTypeEnum.Peasant; break;
                case "FOOTMAN": unitType = UnitTypeEnum.Footman; break;
                case "KNIGHT": unitType = UnitTypeEnum.Knight; break;
                default: throw new ArgumentException();
            }

            int width = 0;
            int height = 0;

            switch (unitType)
            {
                case UnitTypeEnum.Peasant: width = 30; height = 30; break;
                case UnitTypeEnum.Footman: width = 30; height = 30; break; // TODO
                case UnitTypeEnum.Knight: width = 30; height = 30; break;  // TODO
            }

            width = (int)(width * Config.Zoom);
            height = (int)(height * Config.Zoom);

            Unit unit = new Unit(owner, race, unitType, x, y, width, height);

            this.model.Units.Add(unit);

            return unit;
        }
    }
}
