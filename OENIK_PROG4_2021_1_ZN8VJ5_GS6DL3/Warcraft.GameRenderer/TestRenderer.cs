namespace Warcraft.Renderer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Warcraft.Model;

    /// <summary>
    /// Game scene renderer.
    /// </summary>
    public class TestRenderer
    {
        private GameModel model;
        private Dictionary<string, Brush> stateToBrush = new Dictionary<string, Brush>();

        private GeometryDrawing treeGeometry;

        private Dictionary<string, GeometryDrawing> stateToGeometry = new Dictionary<string, GeometryDrawing>();

        private Dictionary<string, Point> positionOffset = new Dictionary<string, Point>();
        private Dictionary<string, GeometryDrawing> buildingToGeometry = new Dictionary<string, GeometryDrawing>();
        private Dictionary<string, GeometryDrawing> mineToGeometry = new Dictionary<string, GeometryDrawing>();
        private GeometryDrawing background;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRenderer"/> class.
        /// </summary>
        /// <param name="model">Gamemodel to operate on.</param>
        public TestRenderer(GameModel model)
        {
            this.model = model;
            this.background = new GeometryDrawing(Brushes.DarkOrange, null, new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight)));

            string[] races = new string[] { "H", "O" };
            string[] buildingTypes = new string[] { "H", "F", "B" };
            string[] directions = new string[] { "N", "NE", "E", "SE", "S", "NW", "W", "SW" };
            string[] unitTypes = new string[] { "P" };

            foreach (string race in races)
            {
                foreach (string unitType in unitTypes)
                {
                    foreach (UnitStateEnum uState in Enum.GetValues(typeof(UnitStateEnum)))
                    {
                        string state = Config.AsString(uState);
                        foreach (string dir in directions)
                        {
                            for (int animationIndex = 0; animationIndex < Config.GetAnimationLength(uState); animationIndex++)
                            {
                                string name = race + unitType + state + dir + animationIndex;
                                this.LoadAndSave(name, this.stateToGeometry);
                            }
                        }
                    }
                }

                foreach (string buildingType in buildingTypes)
                {
                    this.LoadAndSave(race + buildingType, this.buildingToGeometry);
                }
            }

            StreamReader sr = new StreamReader("coordinates.txt");

            foreach (string line in sr.ReadToEnd().Split('\n'))
            {
                if (line == string.Empty)
                {
                    continue;
                }

                double x = int.Parse(line.Split(':')[1].Split(' ')[0]) * Config.Zoom;
                double y = int.Parse(line.Split(':')[1].Split(' ')[1]) * Config.Zoom;
                string name = line.Split(':')[0].Split('.')[0];

                this.positionOffset.Add(name, new Point(x, y));
            }

            sr.Close();

            this.LoadAndSave("IM", this.mineToGeometry);
            this.LoadAndSave("AM", this.mineToGeometry);

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Warcraft.GameRenderer.Images.Trees.png");
            bmp.EndInit();
            this.treeGeometry = new GeometryDrawing(new ImageBrush(bmp), null, new RectangleGeometry(new Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight)));
        }

        /// <summary>
        /// Builds the game state: backround - mines - buildings - units.
        /// </summary>
        /// <returns>Drawing group containing the elements above.</returns>
        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();

            // Background
            dg.Children.Add(this.background);

            // Gold Mines
            foreach (GoldMine mine in this.model.goldMines)
            {
                GeometryDrawing geometry = this.mineToGeometry[mine.animationString];
                geometry.Geometry.Transform = new TranslateTransform(mine.hitbox.X, mine.hitbox.Y);
                dg.Children.Add(geometry);

                // Draw hitboxes
                GeometryDrawing hitboxGeometry = new GeometryDrawing(
                    Brushes.Transparent,
                    new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(mine.hitbox.X, mine.hitbox.Y, mine.hitbox.Width, mine.hitbox.Height)));
                dg.Children.Add(hitboxGeometry);
            }

            // Trees
            foreach (CombatObject tree in this.model.lumberMines)
            {
                this.treeGeometry.Geometry.Transform = new TranslateTransform(tree.hitbox.X, tree.hitbox.Y);
                dg.Children.Add(this.treeGeometry);

                // Draw hitboxes
                GeometryDrawing hitboxGeometry = new GeometryDrawing(
                    Brushes.Transparent,
                    new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(tree.hitbox.X, tree.hitbox.Y, tree.hitbox.Width, tree.hitbox.Height)));
                dg.Children.Add(hitboxGeometry);
            }

            // Buildings
            foreach (Building building in this.model.buildings)
            {
                GeometryDrawing geometry = this.buildingToGeometry[building.animationString];
                geometry.Geometry.Transform = new TranslateTransform(building.hitbox.X, building.hitbox.Y);
                dg.Children.Add(geometry);

                // Draw hitboxes
                GeometryDrawing hitboxGeometry = new GeometryDrawing(
                    Brushes.Transparent,
                    new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(building.hitbox.X, building.hitbox.Y, building.hitbox.Width, building.hitbox.Height)));
                dg.Children.Add(hitboxGeometry);
            }

            // Units
            foreach (Unit unit in this.model.units)
            {
                if (!unit.hiding)
                {
                    GeometryDrawing geometry = this.stateToGeometry[unit.animationString];
                    Point offset = new Point(unit.hitbox.Width / 3, unit.hitbox.Height);
                    if (this.positionOffset.ContainsKey(unit.animationString))
                    {
                        offset = this.positionOffset[unit.animationString];
                    }

                    geometry.Geometry.Transform = new TranslateTransform(unit.hitbox.X + (unit.hitbox.Width / 3) - offset.X, unit.hitbox.Y + unit.hitbox.Height - offset.Y);
                    dg.Children.Add(geometry);
                }
            }

            return dg;
        }

        private void LoadAndSave(string name, Dictionary<string, GeometryDrawing> saveTo)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Warcraft.GameRenderer.Images.{name}.png");
            bmp.EndInit();
            GeometryDrawing g = new GeometryDrawing(new ImageBrush(bmp), null, new RectangleGeometry(new Rect(0, 0, bmp.PixelWidth * Config.Zoom, bmp.PixelHeight * Config.Zoom)));
            saveTo.Add($"{name}", g);
        }
    }
}
