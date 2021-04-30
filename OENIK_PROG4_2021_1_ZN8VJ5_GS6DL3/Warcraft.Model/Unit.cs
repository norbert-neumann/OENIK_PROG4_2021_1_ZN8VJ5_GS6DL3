namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Peasants, footmans, and knights. Basically every combat object that is not a building or a tree.
    /// </summary>
    public class Unit : CombatObject
    {
        /// <summary>
        /// Unit's attack damage.
        /// </summary>
        public int attack = 1;

        /// <summary>
        /// Unit's sprite index.
        /// </summary>
        public int animationIndex;

        /// <summary>
        /// Unit type (peasant, footman, knight).
        /// </summary>
        public UnitTypeEnum UnitType;

        /// <summary>
        /// Unit's game state.
        /// </summary>
        public UnitStateEnum UnitState;

        /// <summary>
        /// Unit's enemy. This field is null if the unit is not in combat.
        /// </summary>
        public CombatObject enemy;

        /// <summary>
        /// A list of points that the unit should visit in order.
        /// </summary>
        public Queue<Point> path;

        /// <summary>
        /// The current point that the unit is trying to visit.
        /// </summary>
        public Point target;

        /// <summary>
        /// Previoius position of the unit. This is needed in PathfindingLogic.
        /// </summary>
        public Point prevPosition;

        /// <summary>
        /// Every unit attacks an enemy if it is inside of this range.
        /// </summary>
        public double range = 40;

        /// <summary>
        /// Facing enum, used by the animation logic.
        /// </summary>
        public DirectionEnum facing;

        /// <summary>
        /// This tells the renderer which sprite to display.
        /// </summary>
        public string animationString;

        /// <summary>
        /// Bool indicating if the unit is in idle (id does nothing).
        /// </summary>
        public bool inIdle = false;

        /// <summary>
        /// Bool indicating if the unit is hiding.
        /// This is true during the unit is mining.
        /// </summary>
        public bool hiding = false;

        /// <summary>
        /// This saves the point before the unit enters an other object's hitbox.
        /// </summary>
        public Point entryPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        /// <param name="race">Unit's race.</param>
        /// <param name="unitType">Unit's type.</param>
        /// <param name="X">X pos.</param>
        /// <param name="Y">Y pos.</param>
        /// <param name="width">Hitbox width.</param>
        /// <param name="height">Hitbox height.</param>
        public Unit(OwnerEnum owner, RaceEnum race, UnitTypeEnum unitType, int X, int Y, int width, int height)
            : base(X, Y, width, height)
        {
            this.Owner = owner;

            switch (race)
            {
                case RaceEnum.Human: this.animationString += "H";
                    break;
                case RaceEnum.Orc:
                    this.animationString += "O";
                    break;
                default:
                    break;
            }

            switch (unitType)
            {
                case UnitTypeEnum.Peasant:
                    this.animationString += "P";
                    break;
                case UnitTypeEnum.Footman:
                    this.animationString += "F";
                    break;
                case UnitTypeEnum.Knight:
                    this.animationString += "K";
                    break;
                default:
                    break;
            }

            this.UnitState = UnitStateEnum.Walking;
            this.animationString += "W";

            this.facing = DirectionEnum.South;
            this.animationString += "S";

            this.animationIndex = 0;
            this.animationString += "0";
        }

        /// <summary>
        /// Puts the current target at the end of the Q, and sets the target to the next point.
        /// This is called after Pathfinding logic creates a new path for this unit.
        /// </summary>
        public void ResetTarget()
        {
            this.path.Enqueue(this.target);
            this.target = this.path.Dequeue();
        }
    }
}
