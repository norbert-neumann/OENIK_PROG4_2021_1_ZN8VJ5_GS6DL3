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
    using Warcraft.GameRenderer;
    using Warcraft.Model;
    using Warcraft.Renderer;

    /// <summary>
    /// Game controller.
    /// </summary>
    public class TestController : FrameworkElement
    {
        private GameModel model;
        private CoreLogic logic;
        private EnemyLogic enemyLogic;
        private MovementLogic movementLogic;
        private TestRenderer renderer;
        private HUDRenderer hudRenderer;

        private DispatcherTimer timer;
        private DispatcherTimer animationTimer;
        private DispatcherTimer enemyTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        public TestController()
        {
            this.Loaded += this.TestLoading;
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.renderer != null)
            {
                drawingContext.DrawDrawing(this.renderer.BuildDrawing());
                drawingContext.DrawDrawing(this.hudRenderer.BuildDrawing());
                this.hudRenderer.AddText(drawingContext);

                // Move this check insie BuildDrawing().
                if (this.model.NewBuilding != null)
                {
                    drawingContext.DrawDrawing(this.renderer.DisplayNewBuilding(this.logic.NewBuildingCollides()));
                }
            }
        }

        private void TestLoading(object sender, RoutedEventArgs e)
        {
            this.model = new GameModel(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            MapBuilder.Build(this.model, (int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
            this.model.Icons = HUDBuilder.BuildHUD(this.model);
            this.model.AddGold(OwnerEnum.ENEMY, 100);
            this.model.AddLumber(OwnerEnum.ENEMY, 50);

            this.movementLogic = new MovementLogic(this.model, new PathfindingLogic());

            this.logic = new CoreLogic(this.model, this.movementLogic);
            this.enemyLogic = new EnemyLogic(this.model, this.movementLogic);

            this.renderer = new TestRenderer(this.model);
            this.hudRenderer = new HUDRenderer(this.model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                this.timer = new DispatcherTimer();
                this.timer.Interval = TimeSpan.FromMilliseconds(30);
                this.timer.Tick += this.TimerTick;
                this.timer.Start();

                this.animationTimer = new DispatcherTimer();
                this.animationTimer.Interval = TimeSpan.FromMilliseconds(70);
                this.animationTimer.Tick += this.AnimationTimer_Tick;
                this.animationTimer.Start();

                this.enemyTimer = new DispatcherTimer();
                this.enemyTimer.Interval = TimeSpan.FromSeconds(3);
                this.enemyTimer.Tick += this.EnemyTimer_Tick;
                this.enemyTimer.Start();

                this.PreviewMouseLeftButtonDown += this.LeftMouseClicled;
            }

            this.InvalidateVisual();
        }

        private void EnemyTimer_Tick(object sender, EventArgs e)
        {
            this.enemyLogic.Step();
            this.InvalidateVisual();
        }

        private void LeftMouseClicled(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point mousePos = e.GetPosition(this);
            this.logic.SelectGameObject(new System.Drawing.Point((int)mousePos.X, (int)mousePos.Y));
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            this.logic.UpdateAnimation();
            this.InvalidateVisual();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            OwnerEnum winner = this.logic.GameStep();
            if (this.model.NewBuilding != null)
            {
                System.Windows.Point mousePos = this.PointToScreen(Mouse.GetPosition(this));
                this.model.NewBuilding.Position = new System.Drawing.Point((int)mousePos.X, (int)mousePos.Y);
            }

            this.InvalidateVisual();

            if (winner != OwnerEnum.EMPTY)
            {
                this.model.GameTime.Stop();
                this.timer.Stop();
                this.animationTimer.Stop();
                this.enemyTimer.Stop();

                if (winner == OwnerEnum.PLAYER)
                {
                    MessageBox.Show("You won! Game time: " + this.model.GameTime.Elapsed);
                }
                else
                {
                    MessageBox.Show("You lost! Game time: " + this.model.GameTime.Elapsed);
                }
            }
        }
    }
}
