using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Warcraft.GameLogic;
using Warcraft.Model;
using Warcraft.Renderer;
using System.Drawing;

namespace Warcraft.GameControl
{
    public class TestController : FrameworkElement
    {
        GameModel model;
        CoreLogic logic;
        CombatLogic combatLogic;
        MovementLogic movementLogic;
        PathfindingLogic pathfindingLogic;
        AnimationLogic animationLogic;
        TestRenderer renderer;

        public TestController()
        {
            Loaded += TestLoading;
        }

        private void TestLoading(object sender, RoutedEventArgs e)
        {
            model = new GameModel(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

            pathfindingLogic = new PathfindingLogic();
            combatLogic = new CombatLogic(model);
            movementLogic = new MovementLogic(model, pathfindingLogic);
            animationLogic = new AnimationLogic(model);
            logic = new CoreLogic(model, combatLogic, movementLogic, animationLogic, pathfindingLogic);

            renderer = new TestRenderer(model);

            
            Building orcHall = new Building(BuildingEnum.Hall, RaceEnum.Orc, 100, 100, 120, 120);
            Building orcFarm = new Building(BuildingEnum.Farm, RaceEnum.Orc, 100, 290, 90, 80);
            Building orcBarrack = new Building(BuildingEnum.Barracks, RaceEnum.Orc, 300, 150, 120, 120);

            Building humanHall = new Building(BuildingEnum.Hall, RaceEnum.Human, 1500, 600, 120, 120);
            Building humanFarm = new Building(BuildingEnum.Farm, RaceEnum.Human, 1500, 800, 90, 80);
            Building humanBarrack = new Building(BuildingEnum.Barracks, RaceEnum.Human, 1700, 600, 120, 120);

            Unit human = new Unit(RaceEnum.Human, UnitTypeEnum.Peasant, 1700, 810, 30, 30);
            Unit orc = new Unit(RaceEnum.Orc, UnitTypeEnum.Peasant, 300, 350, 30, 30);

            human.path = new Queue<System.Drawing.Point>();
            orc.path = new Queue<System.Drawing.Point>();

            human.target = new System.Drawing.Point(450, 450);
            orc.target = new System.Drawing.Point(450, 450);

            model.playerUnits.Add(human);
            model.enemyUnits.Add(orc);

            model.playerBuildings.Add(humanFarm);
            model.playerBuildings.Add(humanBarrack);
            model.playerBuildings.Add(humanHall);

            model.enemyBuildings.Add(orcFarm);
            model.enemyBuildings.Add(orcHall);
            model.enemyBuildings.Add(orcBarrack);


            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += Win_KeyDown;
            }

            InvalidateVisual();
        }

        private void Win_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                logic.Step();
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (renderer != null) drawingContext.DrawDrawing(renderer.BuildDrawing());
        }
    }
}
