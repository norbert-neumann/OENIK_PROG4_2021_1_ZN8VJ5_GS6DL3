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
        public int Attack = 1;

        /// <summary>
        /// Unit's sprite index.
        /// </summary>
        public int AnimationIndex;

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
        public CombatObject Enemy;

        /// <summary>
        /// A list of points that the unit should visit in order.
        /// </summary>
        public Queue<Point> Path;

        /// <summary>
        /// The current point that the unit is trying to visit.
        /// </summary>
        public Point Target;

        /// <summary>
        /// Previoius position of the unit. This is needed in PathfindingLogic.
        /// </summary>
        public Point PrevPosition;

        /// <summary>
        /// Every unit attacks an enemy if it is inside of this range.
        /// </summary>
        public double Range = 40;

        /// <summary>
        /// Facing enum, used by the animation logic.
        /// </summary>
        public DirectionEnum Facing;

        /// <summary>
        /// This tells the renderer which sprite to display.
        /// </summary>
        public string AnimationString;

        /// <summary>
        /// Bool indicating if the unit is in idle (id does nothing).
        /// </summary>
        public bool InIdle = false;

        /// <summary>
        /// Bool indicating if the unit is hiding.
        /// This is true during the unit is mining.
        /// </summary>
        public bool Hiding = false;

        /// <summary>
        /// This saves the point before the unit enters an other object's hitbox.
        /// </summary>
        public Point EntryPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        /// <param name="race">Unit's race.</param>
        /// <param name="unitType">Unit's type.</param>
        /// <param name="x">X pos.</param>
        /// <param name="y">Y pos.</param>
        /// <param name="width">Hitbox width.</param>
        /// <param name="height">Hitbox height.</param>
        public Unit(OwnerEnum owner, RaceEnum race, UnitTypeEnum unitType, int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            this.Owner = owner;

            switch (race)
            {
                case RaceEnum.Human: this.AnimationString += "H";
                    break;
                case RaceEnum.Orc:
                    this.AnimationString += "O";
                    break;
                default:
                    break;
            }

            switch (unitType)
            {
                case UnitTypeEnum.Peasant:
                    this.AnimationString += "P";
                    break;
                case UnitTypeEnum.Footman:
                    this.AnimationString += "F";
                    break;
                case UnitTypeEnum.Knight:
                    this.AnimationString += "K";
                    break;
                default:
                    break;
            }

            this.UnitState = UnitStateEnum.Walking;
            this.AnimationString += "W";

            this.Facing = DirectionEnum.South;
            this.AnimationString += "S";

            this.AnimationIndex = 0;
            this.AnimationString += "0";
        }

        /// <summary>
        /// Puts the current target at the end of the Q, and sets the target to the next point.
        /// This is called after Pathfinding logic creates a new path for this unit.
        /// </summary>
        public void ResetTarget()
        {
            this.Path.Enqueue(this.Target);
            this.Target = this.Path.Dequeue();
        }
    }
}
