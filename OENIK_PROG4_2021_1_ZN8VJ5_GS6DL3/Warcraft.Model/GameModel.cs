using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Warcraft.Model
{
    public class GameModel
    {
        public int GameWidth { get; private set; }
        public int GameHeight { get; private set; }

        public int PlayerGold { get; set; }
        public int PlayerLumber { get; set; }
        public int EnemyGold { get; set; }
        public int EnemyLumber { get; set; }
        List<Unit> PlayerUnits { get => playerUnits; }
        List<Building> PlayerBuildings { get => playerBuildings; }
        List<Unit> EnemyUnits { get => enemyUnits; }
        List<Building> EnemyBuildings { get => enemyBuildings; }
        List<Mine> GoldMines { get => goldMines; }
        List<Mine> LumberMines { get => lumberMines; }

        public List<Unit> playerUnits = new List<Unit>();
        public List<Building> playerBuildings = new List<Building>();
        public List<Unit> enemyUnits = new List<Unit>();
        public List<Building> enemyBuildings = new List<Building>();

        public List<Mine> goldMines = new List<Mine>();
        public List<Mine> lumberMines = new List<Mine>();

        public GameModel(double width, double height)
        {
            this.GameWidth = (int)width;
            this.GameHeight = (int)height;
        }

        public void AddGold(OwnerEnum to, int amount)
        {
            switch (to)
            {
                case OwnerEnum.PLAYER: PlayerGold += amount; break;
                case OwnerEnum.ENEMY: EnemyGold += amount; break;
                default: break;
            }
        }

        public void AddLumber(OwnerEnum to, int amount)
        {
            switch (to)
            {
                case OwnerEnum.PLAYER: PlayerLumber += amount; break;
                case OwnerEnum.ENEMY: EnemyLumber += amount; break;
                default: break;
            }
        }

        public ActiveMine GetClosestGoldMine(Unit unit)
        {
            return this.FindMineWithMinDistance(unit, goldMines) as ActiveMine;
        }

        public Mine GetClosestLumberMine(Unit unit)
        {
            return this.FindMineWithMinDistance(unit, lumberMines);
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
