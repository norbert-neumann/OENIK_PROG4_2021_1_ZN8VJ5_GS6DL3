namespace Warcraft.GameControl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Warcraft.GameLogic;
    using Warcraft.Model;
    using Warcraft.Renderer;

    /// <summary>
    /// Game controller.
    /// </summary>
    public class TestController : FrameworkElement
    {
        GameModel model;
        CoreLogic logic;
        CombatLogic combatLogic;
        MovementLogic movementLogic;
        PathfindingLogic pathfindingLogic;
        AnimationLogic animationLogic;
        TestRenderer renderer;

        private DispatcherTimer timer;
        private DispatcherTimer animationTimer;

        public TestController()
        {
            this.Loaded += this.TestLoading;
        }

        private void TestLoading(object sender, RoutedEventArgs e)
        {
            this.model = new GameModel(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            MapBuilder.Build(this.model, (int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);

            this.pathfindingLogic = new PathfindingLogic();
            this.combatLogic = new CombatLogic(this.model);
            this.movementLogic = new MovementLogic(this.model, this.pathfindingLogic);
            this.animationLogic = new AnimationLogic(this.model);
            this.logic = new CoreLogic(this.model, this.combatLogic, this.movementLogic, this.animationLogic, this.pathfindingLogic);

            this.renderer = new TestRenderer(this.model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += this.Win_KeyDown;
                this.timer = new DispatcherTimer();
                this.timer.Interval = TimeSpan.FromMilliseconds(30);
                this.timer.Tick += this.TimerTick;
                this.timer.Start();

                this.animationTimer = new DispatcherTimer();
                this.animationTimer.Interval = TimeSpan.FromMilliseconds(90);
                this.animationTimer.Tick += this.AnimationTimer_Tick;
                this.animationTimer.Start();

                this.PreviewMouseLeftButtonDown += LeftMouseClicled;
            }

            this.InvalidateVisual();
        }

        private void LeftMouseClicled(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point mousePos = e.GetPosition(this);
            this.logic.Select(new System.Drawing.Point((int)mousePos.X, (int)mousePos.Y));
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.renderer != null)
            {
                drawingContext.DrawDrawing(renderer.BuildDrawing());

                // Move this check insie BuildDrawing().
                if (this.model.NewBuilding != null)
                {
                    drawingContext.DrawDrawing(renderer.DisplayNewBuilding(this.logic.NewBuildingCollides()));
                }
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            this.logic.UpdateAnimation();
            this.InvalidateVisual();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            this.logic.Step();
            if (this.model.NewBuilding != null)
            {
                System.Windows.Point mousePos = this.PointToScreen(Mouse.GetPosition(this));
                this.model.NewBuilding.SetCenterPositon(new System.Drawing.Point((int)mousePos.X, (int)mousePos.Y));
            }

            this.InvalidateVisual();
        }

        private void Win_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.G)
            {
                this.logic.MineGold();
            }
            else if (e.Key == System.Windows.Input.Key.B)
            {
                this.logic.InitNewBuilding("player human farm", new System.Drawing.Point(100, 100));
            }
            else if (e.Key == System.Windows.Input.Key.H)
            {
                this.logic.HarvestLumber();
            }
            else if (e.Key == System.Windows.Input.Key.P)
            {
                this.logic.PatrollUnit();
            }
            else if (e.Key == System.Windows.Input.Key.M)
            {
                this.logic.GoTo();
            }
            else if (e.Key == System.Windows.Input.Key.A)
            {
                this.logic.SetUnitsEnemy();
            }
            else if (e.Key == System.Windows.Input.Key.U)
            {
                this.logic.CreateUnit("player human peasant");
            }
        }
    }
}
