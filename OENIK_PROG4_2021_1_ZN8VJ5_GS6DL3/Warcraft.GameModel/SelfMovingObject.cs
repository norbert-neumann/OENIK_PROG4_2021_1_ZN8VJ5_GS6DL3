namespace Warcraft
{
    using System.Drawing;

    /// <summary>
    /// Represents objects that move by themselves toward a target location.
    /// Units are made from these.
    /// </summary>
    public class SelfMovingObject : CombatObject
    {
        /// <summary>
        /// Target location.
        /// </summary>
        public Point Target { get; set; }

        /// <summary>
        /// Gives which direction is the unit facing.
        /// TODO: Write getter.
        /// </summary>
        public Direction Facing { get; set; }
    }
}
