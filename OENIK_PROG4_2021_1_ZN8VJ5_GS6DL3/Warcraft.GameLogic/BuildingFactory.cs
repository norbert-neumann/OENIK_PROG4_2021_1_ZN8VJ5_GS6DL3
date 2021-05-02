namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Creates and adds a new building to the game model.
    /// </summary>
    public class BuildingFactory
    {
        private GameModel model;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingFactory"/> class.
        /// </summary>
        /// <param name="model">Model to operate on.</param>
        public BuildingFactory(GameModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Creates a specific building described by some parameters.
        /// </summary>
        /// <param name="description">Type and over of the building.</param>
        /// <param name="x">X pos.</param>
        /// <param name="y">Y pos.</param>
        /// <param name="add">Indicaties wheter the building shoud be added to the model right away.</param>
        /// <returns>The newly created buolding.</returns>
        public Building Create(string description, int x, int y, bool add)
        {
            string[] components = description.ToUpper().Split(' ');
            OwnerEnum owner;
            RaceEnum race;
            BuildingEnum buildingType;

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
                case "HALL": buildingType = BuildingEnum.Hall; break;
                case "BARRACK": buildingType = BuildingEnum.Barracks; break;
                case "FARM": buildingType = BuildingEnum.Farm; break;
                default: throw new ArgumentException();
            }

            int width = 0;
            int height = 0;

            // Widths and heights are harcoded here.
            switch (buildingType)
            {
                case BuildingEnum.Hall: width = 120; height = 120; break;
                case BuildingEnum.Farm: width = 90; height = 80; break;
                case BuildingEnum.Barracks: width = 110; height = 110; break;
            }

            width = (int)(width * Config.Zoom);
            height = (int)(height * Config.Zoom);

            Building building = new Building(owner, buildingType, race, x, y, width, height);

            if (add)
            {
                this.model.Buildings.Add(building);
            }

            return building;
        }
    }
}
