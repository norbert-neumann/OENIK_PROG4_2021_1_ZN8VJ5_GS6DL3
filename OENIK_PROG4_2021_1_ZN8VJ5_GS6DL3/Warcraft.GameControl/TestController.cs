namespace Warcraft.GameControl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Text;
    using System.Windows;
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
            BuildingFactory factory = new BuildingFactory(this.model);

            this.pathfindingLogic = new PathfindingLogic();
            this.combatLogic = new CombatLogic(this.model);
            this.movementLogic = new MovementLogic(this.model, this.pathfindingLogic);
            this.animationLogic = new AnimationLogic(this.model);
            this.logic = new CoreLogic(this.model, this.combatLogic, this.movementLogic, this.animationLogic, this.pathfindingLogic);

            this.renderer = new TestRenderer(model);


            Unit human = new Unit(OwnerEnum.PLAYER, RaceEnum.Human, UnitTypeEnum.Peasant, 600, 600, (int)(30 * Config.Zoom), (int)(30 * Config.Zoom));
            human.Path = new Queue<System.Drawing.Point>();
            human.Target = new System.Drawing.Point(10, 10);

            GoldMine mine = new GoldMine(150, 150, 140, 140, 500);

            Building hall = factory.Create("player human hall", 600, 400);
            model.GoldMines.Add(mine);

            movementLogic.Routines.Add(human, new GoldMiningRoutine(human, TimeSpan.FromSeconds(2), hall, mine));
            //logic.routines.Add(human, new GoldMiningRoutine(human, TimeSpan.FromSeconds(2), new System.Drawing.Point(100, 100), new System.Drawing.Point(300, 300)));



            model.Units.Add(human);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += this.Win_KeyDown;
                this.timer = new DispatcherTimer();
                this.timer.Interval = TimeSpan.FromMilliseconds(45);
                this.timer.Tick += this.TimerTick;
                this.timer.Start();

                this.animationTimer = new DispatcherTimer();
                this.animationTimer.Interval = TimeSpan.FromMilliseconds(90);
                this.animationTimer.Tick += this.AnimationTimer_Tick;
                this.animationTimer.Start();
            }

            this.InvalidateVisual();
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.renderer != null)
            {
                drawingContext.DrawDrawing(renderer.BuildDrawing());
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
            this.InvalidateVisual();
        }

        private void Win_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.logic.Step();
                this.InvalidateVisual();
            }
        }
    }
}
