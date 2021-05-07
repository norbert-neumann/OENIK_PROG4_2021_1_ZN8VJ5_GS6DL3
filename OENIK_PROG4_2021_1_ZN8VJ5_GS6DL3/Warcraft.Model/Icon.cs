namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Simple class that encapsulates a hitbox and a string icon type.
    /// </summary>
    public class Icon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        /// <param name="type">Icon type.</param>
        /// <param name="x">Hitbox x pos.</param>
        /// <param name="y">Hitbox y pos.</param>
        /// <param name="w">Hitbox's width.</param>
        /// <param name="h">Hitbox's height.</param>
        public Icon(string type, int x, int y, int w, int h)
        {
            this.IconType = type;
            this.Hitbox = new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Icon's hitbox.
        /// </summary>
        public Rectangle Hitbox { get; set; }

        /// <summary>
        /// Icon's type.
        /// </summary>
        public string IconType { get; set; }
    }
}
