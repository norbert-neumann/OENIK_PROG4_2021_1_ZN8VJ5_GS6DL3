using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Warcraft.Model;

namespace Warcraft.Renderer
{
    public class TestRenderer
    {
        GameModel model;
        Dictionary<string, Brush> stateToBrush = new Dictionary<string, Brush>();


        Dictionary<string, GeometryDrawing> stateToGeometry = new Dictionary<string, GeometryDrawing>();



        Dictionary<string, Point> positionOffset = new Dictionary<string, Point>();
        Dictionary<string, GeometryDrawing> buildingToGeometry = new Dictionary<string, GeometryDrawing>();
        GeometryDrawing background;

        public TestRenderer(GameModel model)
        {
            this.model = model;
            background = new GeometryDrawing(Brushes.DarkOrange, null, new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight)));

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
                                LoadAndSave(name, stateToGeometry);
                            }
                        }
                    }                   
                }

                foreach (string buildingType in buildingTypes)
                {
                    LoadAndSave(race + buildingType, buildingToGeometry);
                }
            }

            StreamReader sr = new StreamReader("coordinates.txt");

            foreach (string line in sr.ReadToEnd().Split('\n'))
            {
                if (line == "")
                {
                    continue;
                }

                int x = int.Parse(line.Split(':')[1].Split(' ')[0]);
                int y = int.Parse(line.Split(':')[1].Split(' ')[1]);
                string name = line.Split(':')[0].Split('.')[0];

                // Get image size
                /*BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"WarcraftDemo.Renderer.Images.{name}.png");
                bmp.EndInit();*/

                positionOffset.Add(name, new Point(x, y));
            }

            sr.Close();

        }

        private void LoadAndSave(string name, Dictionary<string, GeometryDrawing> saveTo)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Warcraft.GameRenderer.Images.{name}.png");
            bmp.EndInit();
            GeometryDrawing g = new GeometryDrawing(new ImageBrush(bmp), null, new RectangleGeometry(new Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight)));
            saveTo.Add($"{name}", g);
        }

        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();

            // Background
            dg.Children.Add(background);

            // Buildings
            foreach (Building building in model.playerBuildings)
            {
                GeometryDrawing geometry = buildingToGeometry[building.animationString];
                geometry.Geometry.Transform = new TranslateTransform(building.hitbox.X, building.hitbox.Y);
                dg.Children.Add(geometry);

                // Draw hitboxes
                GeometryDrawing hitboxGeometry = new GeometryDrawing(Brushes.Transparent, new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(building.hitbox.X, building.hitbox.Y, building.hitbox.Width, building.hitbox.Height)));
                dg.Children.Add(hitboxGeometry);
            }

            // Buildings
            foreach (Building building in model.enemyBuildings)
            {
                GeometryDrawing geometry = buildingToGeometry[building.animationString];
                geometry.Geometry.Transform = new TranslateTransform(building.hitbox.X, building.hitbox.Y);
                dg.Children.Add(geometry);

                // Draw hitboxes
                GeometryDrawing hitboxGeometry = new GeometryDrawing(Brushes.Transparent, new Pen(Brushes.Black, 2),
                    new RectangleGeometry(new Rect(building.hitbox.X, building.hitbox.Y, building.hitbox.Width, building.hitbox.Height)));
                dg.Children.Add(hitboxGeometry);
            }

            // Units
            foreach (Unit unit in model.playerUnits)
            {
                GeometryDrawing geometry = stateToGeometry[unit.animationString];
                if (positionOffset.ContainsKey(unit.animationString))
                {
                    Point offset = positionOffset[unit.animationString];
                    double footX = unit.hitbox.X + offset.X;
                    double footY = unit.hitbox.Y + offset.Y;

                    double imageX = unit.hitbox.X + 10 - offset.X;
                    double imageY = unit.hitbox.Bottom - offset.Y;

                    geometry.Geometry.Transform = new TranslateTransform(imageX, imageY);

                }
                else
                {
                    geometry.Geometry.Transform = new TranslateTransform(unit.hitbox.X, unit.hitbox.Y);
                }

                dg.Children.Add(geometry);
            }

            foreach (Unit unit in model.enemyUnits)
            {
                GeometryDrawing geometry = stateToGeometry[unit.animationString];
                Point offset = new Point(0, 0);
                if (positionOffset.ContainsKey(unit.animationString))
                {
                    offset = positionOffset[unit.animationString];
                }
                geometry.Geometry.Transform = new TranslateTransform(unit.hitbox.X + (unit.hitbox.Width / 3) - offset.X, unit.hitbox.Y + offset.Y);
                dg.Children.Add(geometry);
            }


            return dg;
        }
    }
}
