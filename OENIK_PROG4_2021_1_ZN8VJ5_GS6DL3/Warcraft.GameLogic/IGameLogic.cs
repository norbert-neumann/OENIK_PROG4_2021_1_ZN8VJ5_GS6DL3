namespace Warcraft
{
    using System.Drawing;

    /// <summary>
    /// Bridges the player and the GameModel.
    /// </summary>
    public interface IGameLogic
    {
        /// <summary>
        /// Builds a new building.
        /// </summary>
        /// <param name="building">Building to build.</param>
        /// <param name="builder">Unit to build.</param>
        /// <returns>True if the building was successful, false otherwise.</returns>
        bool Build(Building building, SelfMovingObject builder); // TODO: Replace SMO with Unit

        /// <summary>
        /// Trains a new unit.
        /// </summary>
        /// <param name="unit">Unit to train.</param>
        /// <param name="barrack">Building where the unit should be trained.</param>
        /// <returns>True if the training was successful.</returns>
        bool Train(SelfMovingObject unit, Building barrack); // TODO: Need more lower level classes

        /// <summary>
        /// Selects a unit.
        /// </summary>
        /// <param name="unit">Unit to select.</param>
        void SelectUnit(SelfMovingObject unit);

        /// <summary>
        /// Selects a building.
        /// </summary>
        /// <param name="building">Building to select.</param>
        void SelectBuilding(Building building);

        /// <summary>
        /// Moves a unit to a given location.
        /// </summary>
        /// <param name="targetLocation">Where the unit should move to.</param>
        void MoveTo(Point targetLocation);

        // TODO: Repair, add routine and so much more...
    }
}
