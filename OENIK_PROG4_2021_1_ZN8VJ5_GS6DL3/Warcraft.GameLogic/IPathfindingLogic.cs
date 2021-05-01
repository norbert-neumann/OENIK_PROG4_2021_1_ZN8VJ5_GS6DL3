namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Finds a path around a specific collision and a unit.
    /// </summary>
    public interface IPathfindingLogic
    {
        /// <summary>
        /// Finds a path.
        /// </summary>
        /// <param name="unit">Unit to find a path for.</param>
        /// <param name="collison">Unit collides with this gameObject.</param>
        /// <param name="trajectory">The units trajectory.</param>
        public void FindPath(Unit unit, GameObject collison, Point trajectory);
    }
}
