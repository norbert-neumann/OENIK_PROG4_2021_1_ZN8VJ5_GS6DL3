namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Static class containg configurable constants.
    /// Most fileds will be moved to a specific logic class so I wont document these yet.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Amount of pixels a unit should move in one time step.
        /// </summary>
        public static int Speed = 2;

        /// <summary>
        /// When navigating around a gameobject we need to specify how close we can get to it.
        /// </summary>
        public static int BorderWidth = 10;

        /// <summary>
        /// This threshold decides when to move in both X and Y directions.
        /// </summary>
        public static double DefaultThreshold = 0.8;

        /// <summary>
        /// Enemy units "see" eachother within this distance.
        /// </summary>
        public static double AggroRange = 40;

        /// <summary>
        /// Idk what is this.
        /// </summary>
        public static double DistanceThreshold = 5;

        /// <summary>
        /// All images are zoomed by this rate (except trees, they are a little bigger then the other images for some reason).
        /// </summary>
        public static double Zoom = 1.3;

        /// <summary>
        /// For pathfinding to operate we need to have a minimum space betweeen builings.
        /// During building placement the building's hitbox is extended by this much to guarantee that.
        /// </summary>
        public static int HitboxExtension = 20;

        /// <summary>
        /// Converts a unit state enum to it's "AnimationString" version used by the renderer.
        /// </summary>
        /// <param name="state">Unit state to convert.</param>
        /// <returns>"AnimationString" version of the given enum.</returns>
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
