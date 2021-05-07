namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Inits GameModel based on the screen resolution.
    /// </summary>
    public class MapBuilder
    {
        /// <summary>
        /// Fills the given gamemodel with harcoded buildings, units, and mines.
        /// </summary>
        /// <param name="model">GameModel to fill.</param>
        /// <param name="screenWidth">Screen widht.</param>
        /// <param name="screenHeight">Screen height.</param>
        /// <returns>True if the screen's resolution is big enough.</returns>
        public static bool Build(GameModel model, int screenWidth, int screenHeight)
        {
            // min Widht = startPosX + tree size
            // min Height = goldMine.X + goldmine.Height
            model.SelectedPoint = new System.Drawing.Point(-1, -1);
            BuildingFactory buildingFactory = new BuildingFactory(model);
            MineFactory mineFactory = new MineFactory(model);
            UnitFactory unitFactory = new UnitFactory(model, 100, 0, 1);

            model.EnemyHall = buildingFactory.Create("enemy orc hall", 50, 50, true);
            buildingFactory.Create("enemy orc farm", 50, 450, true);
            buildingFactory.Create("enemy orc barrack", 50, 250, true);

            int centerMineLevel = (screenHeight / 2) - 0;

            int startPosY = 50;
            int delta = -250;
            int treeSize = 80;
            int space = 30;

            while (startPosY + treeSize + (2 * space) < screenHeight - 50 + delta)
            {
                int startPosX = 450;
                while (startPosX + treeSize + (2 * space) < screenWidth)
                {
                    mineFactory.Create("tree", startPosX, startPosY);
                    startPosX += treeSize + (2 * space);
                }

                startPosY += treeSize + (2 * space);
            }

            mineFactory.Create("gold 1600", 250, 450);
            mineFactory.Create("gold 1000", screenWidth - 50 + delta - 300, screenHeight - 50 + delta + 10);

            model.PlayerHall = buildingFactory.Create("player human hall", screenWidth - 50 + delta, screenHeight - 50 + delta, true);
            unitFactory.Create("player human peasant", 700, 700);
            unitFactory.Create("player human peasant", 700, 750);
            unitFactory.Create("player human peasant", 700, 800);

            return true;
        }
    }
}
