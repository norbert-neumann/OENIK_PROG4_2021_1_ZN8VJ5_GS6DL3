namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Storing a game state.
    /// </summary>
    public class GameModel : IGameModel
    {
        /// <summary>
        /// List of icons that are present in the hud.
        /// </summary>
        public List<Icon> Icons;

        /// <summary>
        /// Game time.
        /// </summary>
        public Stopwatch GameTime;

        /// <summary>
        /// Best game time. Possibly wont be used.
        /// </summary>
        public TimeSpan BestGameTime;

        /// <summary>
        /// Result of the last executed command.
        /// </summary>
        public string CommandResult = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameModel"/> class.
        /// </summary>
        /// <param name="width">Width of hte game.</param>
        /// <param name="height">Height of the game.</param>
        public GameModel(double width, double height)
        {
            this.GameWidth = (int)width;
            this.GameHeight = (int)height;
            this.Units = new List<Unit>();
            this.Buildings = new List<Building>();
            this.GoldMines = new List<GoldMine>();
            this.LumberMines = new List<CombatObject>();
            this.BuildingsToRemove = new List<Building>();
            this.UnitsToRemove = new List<Unit>();
            this.TreesToRemove = new List<CombatObject>();
            this.GoldMinesToRemove = new List<GoldMine>();
            this.GameTime = new Stopwatch();

            this.GameTime.Start();
            this.BestGameTime = TimeSpan.FromMinutes(10);

            this.PlayerGold = 100;
            this.PlayerLumber = 50;
        }

        /// <inheritdoc/>
        public RaceEnum PlayerRace { get; set; }

        /// <inheritdoc/>
        public RaceEnum EnemyRace { get; set; }

        /// <inheritdoc/>
        public Building NewBuilding { get; set; }

        /// <summary>
        /// A user action will be executed in this object.
        /// This can be an enemy, a tree, a building, anything.
        /// </summary>
        public GameObject SelectedObject { get; set; }

        /// <summary>
        /// A user action will be executed by this unit.
        /// </summary>
        public Unit SelectedSubject { get; set; }

        /// <summary>
        /// A user action will be executed on this point.
        /// </summary>
        public Point SelectedPoint { get; set; }

        /// <summary>
        /// Player's Hall building. If this is destroyed (=null) then the game is over.
        /// </summary>
        public Building PlayerHall { get; set; }

        /// <summary>
        /// Enemy's Hall building. If this is destroyed (=null) then the game is over.
        /// </summary>
        public Building EnemyHall { get; set; }

        /// <summary>
        /// List containg ALL units.
        /// </summary>
        public List<Unit> Units { get; set; }

        /// <summary>
        /// List containg ALL buildings.
        /// </summary>
        public List<Building> Buildings { get; set; }

        /// <summary>
        /// List containing all gold mines.
        /// </summary>
        public List<GoldMine> GoldMines { get; set; }

        /// <summary>
        /// List containg ALL lumber mines..
        /// </summary>
        public List<CombatObject> LumberMines { get; set; }

        /// <summary>
        /// After a unit's HP is below 0 the unit should be removed.
        /// We can't directly remove it form 'units' bcs we iterate over it with a foreach.
        /// </summary>
        public List<Unit> UnitsToRemove { get; set; }

        /// <summary>
        /// Same as above but with buildings.
        /// </summary>
        public List<Building> BuildingsToRemove { get; set; }

        /// <summary>
        /// Same as above but with trees.
        /// </summary>
        public List<CombatObject> TreesToRemove { get; set; }

        /// <summary>
        /// Same as above but with gold mines.
        /// </summary>
        public List<GoldMine> GoldMinesToRemove { get; set; }

        /// <summary>
        /// Game widht.
        /// </summary>
        public int GameWidth { get;  set; }

        /// <summary>
        /// Game height.
        /// </summary>
        public int GameHeight { get; set; }

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
            double minDistance = double.MaxValue;
            int minIdx = -1;

            for (int i = 0; i < this.GoldMines.Count; i++)
            {
                double dist = this.GoldMines[i].Distance(unit);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    minIdx = i;
                }
            }

            if (minIdx != -1)
            {
                return this.GoldMines[minIdx];
            }

            return null;
        }

        /// <summary>
        /// Returns the closest lumber mine. Used by routines.
        /// </summary>
        /// <param name="unit">Closest lumber mine to this unit.</param>
        /// <returns>Closeset lumber mine.</returns>
        public CombatObject GetClosestLumberMine(Unit unit)
        {
            double minDistance = double.MaxValue;
            int minIdx = -1;

            for (int i = 0; i < this.LumberMines.Count; i++)
            {
                double dist = this.LumberMines[i].Distance(unit);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    minIdx = i;
                }
            }

            if (minIdx != -1)
            {
                return this.LumberMines[minIdx];
            }

            return null;
        }

        /// <summary>
        /// Returns the closeset player owned unit to the given parameter.
        /// </summary>
        /// <param name="gameObject">Closest to this object.</param>
        /// <returns>Closeset player owned unit.</returns>
        public CombatObject GetClosestPlayerUnit(GameObject gameObject)
        {
            double minDistance = double.MaxValue;
            int minIdx = -1;

            for (int i = 0; i < this.Units.Count; i++)
            {
                if (this.Units[i].Owner == OwnerEnum.PLAYER)
                {
                    double dist = this.Units[i].Distance(gameObject);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        minIdx = i;
                    }
                }
            }

            if (minIdx != -1)
            {
                return this.Units[minIdx];
            }

            return null;
        }

        /// <summary>
        /// Returns the closeset gold or lumber mine.
        /// </summary>
        /// <param name="unit">Closeset to this unit.</param>
        /// <param name="mines">Mine type.</param>
        /// <returns>Just read the text above.</returns>
        public GameObject FindMineWithMinDistance(Unit unit, List<GameObject> mines)
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
