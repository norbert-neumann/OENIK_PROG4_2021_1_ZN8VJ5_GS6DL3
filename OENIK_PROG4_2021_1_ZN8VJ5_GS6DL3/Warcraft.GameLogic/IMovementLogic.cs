namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Deals with everything realted to movement.
    /// </summary>
    public interface IMovementLogic
    {
        /// <summary>
        /// Update each unit's positions.
        /// </summary>
        public void UpdatePositions();
    }
}
