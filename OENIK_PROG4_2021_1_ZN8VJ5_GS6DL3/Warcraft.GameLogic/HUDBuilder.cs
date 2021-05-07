namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Builds the HUD.
    /// </summary>
    public class HUDBuilder
    {
        /// <summary>
        /// Add icons to the game model.
        /// </summary>
        /// <param name="model">Model to operate on.</param>
        /// <returns>A list of icons that will be on he hud.</returns>
        public static List<Icon> BuildHUD(GameModel model)
        {
            List<Icon> icons = new List<Icon>();

            int iconWidth = (int)(1920 / 46 * 1.5);
            int iconHeight = (int)(1080 / 38 * 1.5);

            int width = (int)(iconWidth + 5) * 3;
            int height = model.GameHeight / 3;
            int x = 0;
            int y = model.GameHeight - height;

            icons.Add(new Icon("mine_icon", x, y, iconWidth, iconHeight));
            icons.Add(new Icon("harvestLumber_icon", x + iconWidth + 5, y, iconWidth, iconHeight));
            icons.Add(new Icon("move_icon", x + (2 * (iconWidth + 5)), y, iconWidth, iconHeight));

            icons.Add(new Icon("attack_icon", x, y + iconHeight + 5, iconWidth, iconHeight));
            icons.Add(new Icon("stop_icon", x + iconWidth + 5, y + iconHeight + 5, iconWidth, iconHeight));
            icons.Add(new Icon("patroll_icon", x + (2 * (iconWidth + 5)), y + iconHeight + 5, iconWidth, iconHeight));

            icons.Add(new Icon("human_farm_icon", x, y + (2 * (iconHeight + 5)), iconWidth, iconHeight));
            icons.Add(new Icon("orc_farm_icon", x, y + (2 * (iconHeight + 5)), iconWidth, iconHeight));

            icons.Add(new Icon("human_barrack_icon", x + iconWidth + 5, y + (2 * (iconHeight + 5)), iconWidth, iconHeight));
            icons.Add(new Icon("orc_barrack_icon", x + iconWidth + 5, y + (2 * (iconHeight + 5)), iconWidth, iconHeight));

            icons.Add(new Icon("human_peasant_icon", x, y + (3 * (iconHeight + 5)), iconWidth, iconHeight));
            icons.Add(new Icon("orc_peasant_icon", x, y + (3 * (iconHeight + 5)), iconWidth, iconHeight));

            return icons;
        }
    }
}
