namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Deals with everything realted to combat.
    /// </summary>
    public interface ICombatLogic
    {
        /// <summary>
        /// Check if agressive object reached it's enemy. If not set target. If yes flip UnitStateEnum.
        /// </summary>
        void SetTarget();
    }
}
