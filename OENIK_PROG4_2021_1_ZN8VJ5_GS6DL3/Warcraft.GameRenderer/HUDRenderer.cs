namespace Warcraft.GameRenderer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Warcraft.Model;

    /// <summary>
    /// Renders the HUD.
    /// </summary>
    public class HUDRenderer
    {
        private GameModel model;

        private GeometryDrawing background;
        private Typeface font = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

        private Point resourcesTextLocation;
        private Point gameTimeTextLocation;
        private Point bestGameTimeTextLocation;
        private Point selectedObjectTextLocation;

        private Dictionary<string, GeometryDrawing> iconToGeometry = new Dictionary<string, GeometryDrawing>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HUDRenderer"/> class.
        /// </summary>
        /// <param name="model">Game model to operate on.</param>
        public HUDRenderer(GameModel model)
        {
            this.model = model;

            double iconWidth = 1920 / 46 * 1.5;

            int width = (int)(iconWidth + 5) * 3;
            int height = model.GameHeight / 3;

            int y = model.GameHeight - height;

            this.resourcesTextLocation = new Point(5, y + (3 * (iconWidth + 5)));
            this.gameTimeTextLocation = new Point(5, y + (3 * (iconWidth + 5)) + 20);
            this.bestGameTimeTextLocation = new Point(5, y + (3 * (iconWidth + 5)) + 40);
            this.selectedObjectTextLocation = new Point(5, y + (3 * (iconWidth + 5)) + 60);

            this.background = new GeometryDrawing(Brushes.SaddleBrown, null, new RectangleGeometry(new Rect(0, this.model.GameHeight - height, width, height)));

            foreach (Icon icon in this.model.Icons)
            {
                GeometryDrawing geometry = this.LoadAndSave(icon.IconType, icon.Hitbox.Width, icon.Hitbox.Height);
                geometry.Geometry.Transform = new TranslateTransform(icon.Hitbox.X, icon.Hitbox.Y);
                this.iconToGeometry.Add(icon.IconType, geometry);
            }
        }

        /// <summary>
        /// Adds necessary icons to HUD.
        /// </summary>
        /// <returns>Drawing containing the needed icon images.</returns>
        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();

            dg.Children.Add(this.background);
            dg.Children.Add(this.iconToGeometry["mine_icon"]);
            dg.Children.Add(this.iconToGeometry["harvestLumber_icon"]);
            dg.Children.Add(this.iconToGeometry["move_icon"]);
            dg.Children.Add(this.iconToGeometry["attack_icon"]);
            dg.Children.Add(this.iconToGeometry["stop_icon"]);
            dg.Children.Add(this.iconToGeometry["patroll_icon"]);

            if (this.model.SelectedObject is Building && (this.model.SelectedObject as Building).Type == BuildingEnum.Barracks
                && (this.model.SelectedObject as Building).Owner == OwnerEnum.PLAYER)
            {
                if (this.model.PlayerRace == RaceEnum.Human)
                {
                    dg.Children.Add(this.iconToGeometry["human_peasant_icon"]);
                }
                else
                {
                    dg.Children.Add(this.iconToGeometry["orc_peasant_icon"]);
                }
            }

            if (this.model.SelectedObject is Building && (this.model.SelectedObject as Building).Type == BuildingEnum.Hall
                && (this.model.SelectedObject as Building).Owner == OwnerEnum.PLAYER)
            {
                if (this.model.PlayerRace == RaceEnum.Human)
                {
                    dg.Children.Add(this.iconToGeometry["human_barrack_icon"]);
                    dg.Children.Add(this.iconToGeometry["human_farm_icon"]);
                }
                else
                {
                    dg.Children.Add(this.iconToGeometry["orc_barrack_icon"]);
                    dg.Children.Add(this.iconToGeometry["orc_farm_icon"]);
                }
            }

            return dg;
        }

        /// <summary>
        /// Draws all hud information to the given context.
        /// </summary>
        /// <param name="ctx">Context to operate on.</param>
        public void AddText(DrawingContext ctx)
        {
            FormattedText formattedText = new FormattedText(
                    string.Format($"Gold: {this.model.PlayerGold}   Lumber {this.model.PlayerLumber}"),
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    this.font,
                    16,
                    Brushes.Black,
                    1);

            ctx.DrawText(formattedText, this.resourcesTextLocation);

            formattedText = new FormattedText(
                    string.Format($"Game time: {this.model.GameTime.Elapsed.Minutes}:{this.model.GameTime.Elapsed.Seconds}"),
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    this.font,
                    16,
                    Brushes.Black,
                    1);

            ctx.DrawText(formattedText, this.gameTimeTextLocation);

            formattedText = new FormattedText(
                    this.model.CommandResult,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    this.font,
                    16,
                    Brushes.Black,
                    1);

            ctx.DrawText(formattedText, this.bestGameTimeTextLocation);

            if (this.model.SelectedObject != null)
            {
                formattedText = new FormattedText(
                this.model.SelectedObject.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                this.font,
                16,
                Brushes.Black,
                1);

                ctx.DrawText(formattedText, this.selectedObjectTextLocation);
            }
            else if (this.model.SelectedSubject != null)
            {
                formattedText = new FormattedText(
                this.model.SelectedSubject.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                this.font,
                16,
                Brushes.Black,
                1);

                ctx.DrawText(formattedText, this.selectedObjectTextLocation);
            }
        }

        private GeometryDrawing LoadAndSave(string name, double iconWidth, double iconHeight)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Warcraft.GameRenderer.Images.{name}.png");
            bmp.EndInit();
            return new GeometryDrawing(new ImageBrush(bmp), null, new RectangleGeometry(new Rect(0, 0, iconWidth, iconHeight)));
        }
    }
}
