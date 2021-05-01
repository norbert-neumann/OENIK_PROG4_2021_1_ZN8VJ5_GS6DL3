namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This have the necessary props to store a game state.
    /// </summary>
    public interface IGameModel
    {
        /// <summary>
        /// A user action will be executed in this object.
        /// This can be an enemy, a tree, a building, anything.
        /// </summary>
        GameObject SelectedObject { get; set; }

        /// <summary>
        /// A user action will be executed by this unit.
        /// </summary>
        Unit SelectedSubject { get; set; }

        /// <summary>
        /// A user action will be executed on this point.
        /// </summary>
        Point SelectedPoint { get; set; }

        /// <summary>
        /// Player's Hall building. If this is destroyed (=null) then the game is over.
        /// </summary>
        Building PlayerHall { get; set; }

        /// <summary>
        /// Enemy's Hall building. If this is destroyed (=null) then the game is over.
        /// </summary>
        Building EnemyHall { get; set; }

        /// <summary>
        /// List containg ALL units.
        /// </summary>
        List<Unit> Units { get; set; }

        /// <summary>
        /// List containg ALL buildings.
        /// </summary>
        List<Building> Buildings { get; set; }

        /// <summary>
        /// List containing all gold mines.
        /// </summary>
        List<GoldMine> GoldMines { get; set; }

        /// <summary>
        /// List containg ALL lumber mines..
        /// </summary>
        List<CombatObject> LumberMines { get; set; }

        /// <summary>
        /// After a unit's HP is below 0 the unit should be removed.
        /// We can't directly remove it form 'units' bcs we iterate over it with a foreach.
        /// </summary>
        List<Unit> UnitsToRemove { get; set; }

        /// <summary>
        /// Same as above but with buildings.
        /// </summary>
        List<Building> BuildingsToRemove { get; set; }

        /// <summary>
        /// Same as above but with trees.
        /// </summary>
        List<CombatObject> TreesToRemove { get; set; }

        /// <summary>
        /// Same as above but with gold mines.
        /// </summary>
        List<GoldMine> GoldMinesToRemove { get; set; }

        /// <summary>
        /// Game widht.
        /// </summary>
        int GameWidth { get; set; }

        /// <summary>
        /// Game height.
        /// </summary>
        int GameHeight { get; set; }

        /// <summary>
        /// Amount of gold the player has.
        /// </summary>
        int PlayerGold { get; set; }

        /// <summary>
        /// Amount of lumber the player has.
        /// </summary>
        int PlayerLumber { get; set; }

        /// <summary>
        /// Amount of gold the enemy has.
        /// </summary>
        int EnemyGold { get; set; }

        /// <summary>
        /// Amount of lumber the enemy has.
        /// </summary>
        int EnemyLumber { get; set; }

        /// <summary>
        /// Adds some amount of gold to some1.
        /// </summary>
        /// <param name="to">Add gold to this entity.</param>
        /// <param name="amount">Amount of gold added.</param>
        void AddGold(OwnerEnum to, int amount);

        /// <summary>
        /// Adds some amount of lumber to some1.
        /// </summary>
        /// <param name="to">Add lumber to this entity.</param>
        /// <param name="amount">Amount of lumbner added.</param>
        void AddLumber(OwnerEnum to, int amount);

        /// <summary>
        /// Returns the closest lumber mine. Used by routines.
        /// </summary>
        /// <param name="unit">Closest lumber mine to this unit.</param>
        /// <returns>Closeset lumber mine.</returns>
        public Mine GetClosestLumberMine(Unit unit);
    }
}
