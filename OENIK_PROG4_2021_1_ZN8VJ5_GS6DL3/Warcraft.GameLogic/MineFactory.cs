namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Creates a mine based on a description.
    /// </summary>
    public class MineFactory
    {
        private GameModel model;

        /// <summary>
        /// Initializes a new instance of the <see cref="MineFactory"/> class.
        /// </summary>
        /// <param name="model">Game model to operaete on.</param>
        public MineFactory(GameModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Creates a mine based on a description.
        /// </summary>
        /// <param name="description">What mine to create.</param>
        /// <param name="x">X pos.</param>
        /// <param name="y">Y pos.</param>
        /// <returns>The newly created game object.</returns>
        public GameObject Create(string description, int x, int y)
        {
            string[] components = description.ToUpper().Split(' ');

            int width = 0;
            int height = 0;

            if (components[0] == "GOLD")
            {
                width = 100;
                height = 100;

                width = (int)(width * Config.Zoom);
                height = (int)(height * Config.Zoom);

                GoldMine goldMine = new GoldMine(x, y, width, height, int.Parse(components[1]));
                this.model.GoldMines.Add(goldMine);

                return goldMine;
            }
            else
            {
                width = 80;
                height = 70;

                width = (int)(width * Config.Zoom);
                height = (int)(height * Config.Zoom);

                CombatObject tree = new CombatObject(x, y, width, height, 100);
                this.model.LumberMines.Add(tree);

                return tree;
            }
        }
    }
}
