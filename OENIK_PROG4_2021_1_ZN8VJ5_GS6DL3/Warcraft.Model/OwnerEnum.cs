namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Enum indicating the unit's owner.
    /// </summary>
    public enum OwnerEnum
    {
        /// <summary>
        /// Player.
        /// </summary>
        PLAYER,

        /// <summary>
        /// Enemy.
        /// </summary>
        ENEMY,

        /// <summary>
        /// Empty owner.
        /// </summary>
        EMPTY,
    }
}
