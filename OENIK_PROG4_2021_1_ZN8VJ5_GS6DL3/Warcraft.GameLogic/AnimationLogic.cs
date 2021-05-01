namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Warcraft.Model;

    /// <summary>
    /// This logic deals with animationString and animationIndex.
    /// </summary>
    public class AnimationLogic
    {
        private GameModel model;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationLogic"/> class.
        /// </summary>
        /// <param name="model">Game model to operate on.</param>
        public AnimationLogic(GameModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Increments and resets the animation index.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        public void IncrementAnimationIndex(Unit unit)
        {
            unit.AnimationIndex++;
            if (unit.AnimationIndex > (unit.UnitState == UnitStateEnum.WalkingWithGold ? 3 : 4))
            {
                ResetAnimationIndex(unit);
            }
        }

        /// <summary>
        /// Resets a unit's animation index.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        public void ResetAnimationIndex(Unit unit)
        {
            unit.AnimationIndex = 0;
        }

        /// <summary>
        /// Sets a unit's animationString.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        public void SetAnimationString(Unit unit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(unit.AnimationString.Substring(0, 2)); // Race and type is always the same

            switch (unit.UnitState)
            {
                case UnitStateEnum.Walking:
                    sb.Append("W");
                    break;
                case UnitStateEnum.WalkingWithLumber:
                    sb.Append("WT");
                    break;
                case UnitStateEnum.WalkingWithGold:
                    sb.Append("WG");
                    break;
                case UnitStateEnum.Fighting:
                    sb.Append("F");
                    break;
                default:
                    break;
            }

            switch (unit.Facing)
            {
                case DirectionEnum.North:
                    sb.Append("N");
                    break;
                case DirectionEnum.NorthEast:
                    sb.Append("NE");
                    break;
                case DirectionEnum.East:
                    sb.Append("E");
                    break;
                case DirectionEnum.SouthEast:
                    sb.Append("SE");
                    break;
                case DirectionEnum.South:
                    sb.Append("S");
                    break;
                case DirectionEnum.SouthWest:
                    sb.Append("SW");
                    break;
                case DirectionEnum.West:
                    sb.Append("W");
                    break;
                case DirectionEnum.NorthWest:
                    sb.Append("NW");
                    break;
                default:
                    break;
            }

            sb.Append(unit.AnimationIndex.ToString());

            unit.AnimationString = sb.ToString();
        }

        /// <summary>
        /// Sets the animationString for all units.
        /// </summary>
        public void UpdateSprites()
        {
            foreach (Unit unit in this.model.Units)
            {
                // Check if changed is true should be here
                if (!unit.InIdle)
                {
                    this.SetAnimationString(unit);
                    this.IncrementAnimationIndex(unit);
                }
            }
        }
    }
}
