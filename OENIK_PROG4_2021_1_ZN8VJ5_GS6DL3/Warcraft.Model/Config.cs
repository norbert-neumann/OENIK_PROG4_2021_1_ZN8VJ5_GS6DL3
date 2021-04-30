using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Warcraft.Model
{
    public static class Config
    {
        public static Point HumanTownHallPosition = new Point(5, 5);
        public static Point OrcTownHallPosition = new Point(5, 5);
        public static int GoldUnit = 21;
        public static int LumberUnit = 17;
        public static int Speed = 3;
        public static int Speed = 2;
        public static int BorderWidth = 10;
        public static double DefaultThreshold = 0.8;
        public static double AggroRange = 50;
        public static double DistanceThreshold = 5;

        public static string AsString(UnitStateEnum state)
        {
            switch (state)
            {
                case UnitStateEnum.Walking:
                    return "W";
                case UnitStateEnum.WalkingWithLumber:
                    return "WT";
                case UnitStateEnum.WalkingWithGold:
                    return "WG";
                case UnitStateEnum.Fighting:
                    return "F";
                default:
                    return "";
            }
        }

        // Move this to logic
        public static int GetAnimationLength(UnitStateEnum state)
        {
            if (state == UnitStateEnum.WalkingWithGold)
            {
                return 4;
            }
            return 5;
        } 
    }
}
