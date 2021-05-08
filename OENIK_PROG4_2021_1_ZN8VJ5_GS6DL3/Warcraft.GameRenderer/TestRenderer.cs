[assembly: System.CLSCompliant(false)]

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
            foreach (GoldMine mine in this.model.GoldMines)
            {
                GeometryDrawing geometry = this.mineToGeometry[mine.AnimationString].Clone();
                geometry.Geometry.Transform = new TranslateTransform(mine.Hitbox.X, mine.Hitbox.Y);
                dg.Children.Add(geometry);

                // Draw hitboxes
              /*  GeometryDrawing hitboxGeometry = new GeometryDrawing(
                    Brushes.Transparent,
                    new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(mine.Hitbox.X, mine.Hitbox.Y, mine.Hitbox.Width, mine.Hitbox.Height)));
                dg.Children.Add(hitboxGeometry);*/
            }

            // Trees
            foreach (CombatObject tree in this.model.LumberMines)
            {
                GeometryDrawing geometry = this.treeGeometry.Clone();
                geometry.Geometry.Transform = new TranslateTransform(tree.Hitbox.X, tree.Hitbox.Y);
                dg.Children.Add(geometry);

                // Draw hitboxes
               /* GeometryDrawing hitboxGeometry = new GeometryDrawing(
                    Brushes.Transparent,
                    new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(tree.Hitbox.X, tree.Hitbox.Y, tree.Hitbox.Width, tree.Hitbox.Height)));
                dg.Children.Add(hitboxGeometry);*/
            }

            // Buildings
            foreach (Building building in this.model.Buildings)
            {
                GeometryDrawing geometry = this.buildingToGeometry[building.AnimationString].Clone();
                geometry.Geometry.Transform = new TranslateTransform(building.Hitbox.X, building.Hitbox.Y);
                dg.Children.Add(geometry);

                // Draw hitboxes
               /*GeometryDrawing hitboxGeometry = new GeometryDrawing(
                    Brushes.Transparent,
                    new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(building.Hitbox.X, building.Hitbox.Y, building.Hitbox.Width, building.Hitbox.Height)));
                dg.Children.Add(hitboxGeometry);*/
            }

            // Units
            foreach (Unit unit in this.model.Units)
            {
                if (!unit.Hiding)
                {
                    GeometryDrawing geometry = this.stateToGeometry[unit.AnimationString].Clone();
                    Point offset = new Point(unit.Hitbox.Width / 3, unit.Hitbox.Height);
                    if (this.positionOffset.ContainsKey(unit.AnimationString))
                    {
                        offset = this.positionOffset[unit.AnimationString];
                    }

                    geometry.Geometry.Transform = new TranslateTransform(unit.Hitbox.X + (unit.Hitbox.Width / 3) - offset.X, unit.Hitbox.Y + unit.Hitbox.Height - offset.Y);
                    dg.Children.Add(geometry);
                }
            }

            // Selected subject
            if (this.model.SelectedSubject != null && !this.model.SelectedSubject.Hiding)
            {
                GeometryDrawing hitboxGeometry = new GeometryDrawing(
                Brushes.Transparent,
                new Pen(Brushes.Green, 2),
                new RectangleGeometry(new Rect(
                    this.model.SelectedSubject.Hitbox.X,
                    this.model.SelectedSubject.Hitbox.Y,
                    this.model.SelectedSubject.Hitbox.Width,
                    this.model.SelectedSubject.Hitbox.Height)));
                dg.Children.Add(hitboxGeometry);
            }

            if (this.model.SelectedObject != null)
            {
                GeometryDrawing hitboxGeometry = new GeometryDrawing(
                Brushes.Transparent,
                new Pen(Brushes.Green, 2),
                new RectangleGeometry(new Rect(
                    this.model.SelectedObject.Hitbox.X,
                    this.model.SelectedObject.Hitbox.Y,
                    this.model.SelectedObject.Hitbox.Width,
                    this.model.SelectedObject.Hitbox.Height)));
                dg.Children.Add(hitboxGeometry);
            }

            return dg;
        }

        /// <summary>
        /// Displays the new building with hitbox.
        /// Hitbox is red when the new building collides with somehing.
        /// </summary>
        /// <param name="collision">Bool indicating collison.</param>
        /// <returns>Drawing group containing the new building and it's hitbox.</returns>
        public Drawing DisplayNewBuilding(bool collision)
        {
            DrawingGroup dg = new DrawingGroup();

            GeometryDrawing geometry = this.buildingToGeometry[this.model.NewBuilding.AnimationString];
            geometry.Geometry.Transform = new TranslateTransform(this.model.NewBuilding.Hitbox.X + Config.HitboxExtension, this.model.NewBuilding.Hitbox.Y + Config.HitboxExtension);
            dg.Children.Add(geometry);

            // Hitbox
            GeometryDrawing hitboxGeometry = new GeometryDrawing(
                Brushes.Transparent,
                new Pen(collision ? Brushes.Red : Brushes.Black, 2),
                new RectangleGeometry(new Rect(this.model.NewBuilding.Hitbox.X, this.model.NewBuilding.Hitbox.Y, this.model.NewBuilding.Hitbox.Width, this.model.NewBuilding.Hitbox.Height)));
            dg.Children.Add(hitboxGeometry);

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
