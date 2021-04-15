using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Warcraft.Model
{
    // With properties we should store a bool "changed" field that is true if unit type/state/facing's setter is called
    // For now we just gonna call GetAnimationString every renderer loop
    public class Unit : CombatObject
    {
        public int attack;
        public int animationIndex;
        public UnitTypeEnum UnitType;
        public UnitStateEnum UnitState;
        public CombatObject enemy;
        public Queue<Point> path;
        public Point target;
        public Point prevPosition;
        public double range = 15;
        public DirectionEnum facing;
        public string animationString;

        // RaceEnum.Human, UnitTypeEnum.Peasant, 250, 300, 30, 30);

        public Unit(RaceEnum race, UnitTypeEnum unitType, int X, int Y, int width, int height)
        {
            this.hitbox = new Rectangle(X, Y, width, height);

            switch (race)
            {
                case RaceEnum.Human: animationString += "H";
                    break;
                case RaceEnum.Orc:
                    animationString += "O";
                    break;
                default:
                    break;
            }

            switch (unitType)
            {
                case UnitTypeEnum.Peasant:
                    animationString += "P";
                    break;
                case UnitTypeEnum.Footman:
                    animationString += "F";
                    break;
                case UnitTypeEnum.Knight:
                    animationString += "K";
                    break;
                default:
                    break;
            }

            UnitState = UnitStateEnum.Walking;
            animationString += "W";

            facing = DirectionEnum.South;
            animationString += "S";

            animationIndex = 0;
            animationString += "0";
        }

        public void ResetTarget()
        {
            path.Enqueue(target);
            target = path.Dequeue();
        }
    }
}
