﻿namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Storing a game state.
    /// </summary>
    public class GameModel
    {
        /// <summary>
        /// List containg ALL units.
        /// </summary>
        public List<Unit> Units = new List<Unit>();

        /// <summary>
        /// List containg ALL buildings.
        /// </summary>
        public List<Building> Buildings = new List<Building>();

        /// <summary>
        /// List containing all gold mines.
        /// </summary>
        public List<GoldMine> GoldMines = new List<GoldMine>();

        /// <summary>
        /// List containg ALL lumber mines..
        /// </summary>
        public List<CombatObject> LumberMines = new List<CombatObject>();

        /// <summary>
        /// After a unit's HP is below 0 the unit should be removed.
        /// We can't directly remove it form 'units' bcs we iterate over it with a foreach.
        /// </summary>
        public List<Unit> UnitsToRemove = new List<Unit>();

        /// <summary>
        /// Same as above but with buildings.
        /// </summary>
        public List<Building> BuildingsToRemove = new List<Building>();

        /// <summary>
        /// Same as above but with trees.
        /// </summary>
        public List<CombatObject> TreesToRemove = new List<CombatObject>();

        /// <summary>
        /// Same as above but with gold mines.
        /// </summary>
        public List<GoldMine> GoldMinesToRemove = new List<GoldMine>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameModel"/> class.
        /// </summary>
        /// <param name="width">Width of hte game.</param>
        /// <param name="height">Height of the game.</param>
        public GameModel(double width, double height)
        {
            this.GameWidth = (int)width;
            this.GameHeight = (int)height;
        }

        /// <summary>
        /// Game widht.
        /// </summary>
        public int GameWidth { get; private set; }

        /// <summary>
        /// Game height.
        /// </summary>
        public int GameHeight { get; private set; }

        /// <summary>
        /// Amount of gold the player has.
        /// </summary>
        public int PlayerGold { get; set; }

        /// <summary>
        /// Amount of lumber the player has.
        /// </summary>
        public int PlayerLumber { get; set; }

        /// <summary>
        /// Amount of gold the enemy has.
        /// </summary>
        public int EnemyGold { get; set; }

        /// <summary>
        /// Amount of lumber the enemy has.
        /// </summary>
        public int EnemyLumber { get; set; }

        /// <summary>
        /// Adds some amount of gold to some1.
        /// </summary>
        /// <param name="to">Add gold to this entity.</param>
        /// <param name="amount">Amount of gold added.</param>
        public void AddGold(OwnerEnum to, int amount)
        {
            switch (to)
            {
                case OwnerEnum.PLAYER: this.PlayerGold += amount; break;
                case OwnerEnum.ENEMY: this.EnemyGold += amount; break;
                default: break;
            }
        }

        /// <summary>
        /// Adds some amount of lumber to some1.
        /// </summary>
        /// <param name="to">Add lumber to this entity.</param>
        /// <param name="amount">Amount of lumbner added.</param>
        public void AddLumber(OwnerEnum to, int amount)
        {
            switch (to)
            {
                case OwnerEnum.PLAYER: this.PlayerLumber += amount; break;
                case OwnerEnum.ENEMY: this.EnemyLumber += amount; break;
                default: break;
            }
        }

        /// <summary>
        /// Returns the closest gold mine. Used by routines.
        /// </summary>
        /// <param name="unit">Closest gold mine to this unit.</param>
        /// <returns>Closeset gold mine.</returns>
        public GoldMine GetClosestGoldMine(Unit unit)
        {
            return null;
        }

        /// <summary>
        /// Returns the closest lumber mine. Used by routines.
        /// </summary>
        /// <param name="unit">Closest lumber mine to this unit.</param>
        /// <returns>Closeset lumber mine.</returns>
        public Mine GetClosestLumberMine(Unit unit)
        {
            return null;
        }

        private Mine FindMineWithMinDistance(Unit unit, List<Mine> mines)
        {
            double minDistance = double.MaxValue;
            int minIdx = -1;

            for (int i = 0; i < mines.Count; i++)
            {
                double dist = mines[i].Distance(unit);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    minIdx = i;
                }
            }

            return mines[minIdx];
        }
    }
}