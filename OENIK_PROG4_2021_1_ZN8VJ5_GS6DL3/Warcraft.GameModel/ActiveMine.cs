namespace Warcraft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an active mine. The difference between this and it's parent, is that this mine can tell if it's currently being used.
    /// </summary>
    public class ActiveMine : Mine
    {
        /// <summary>
        /// Returns true if the mine is being used.
        /// </summary>
        public bool IsUsed { get; set; }
    }
}
