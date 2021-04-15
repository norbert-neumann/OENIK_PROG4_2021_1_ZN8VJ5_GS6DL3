using System;
using System.Collections.Generic;
using System.Text;
using Warcraft.Model;

namespace Warcraft.GameLogic
{
    public class AnimationLogic
    {
        private GameModel model;

        public AnimationLogic(GameModel model)
        {
            this.model = model;
        }

        public void UpdateSprites()
        {
            foreach (Unit unit in model.playerUnits)
            {
                // Check if changed is true should be here
                SetAnimationString(unit);
                AnimationLogic.IncrementAnimationIndex(unit);
            }

            foreach (Unit unit in model.enemyUnits)
            {
                // Check if changed is true should be here
                SetAnimationString(unit);
                AnimationLogic.IncrementAnimationIndex(unit);
            }
        }

        public static void IncrementAnimationIndex(Unit unit)
        {
            unit.animationIndex++;
            if (unit.animationIndex > (unit.UnitState == UnitStateEnum.WalkingWithGold ? 3 : 4))
            {
                ResetAnimationIndex(unit);
            }
        }

        public static void ResetAnimationIndex(Unit unit)
        {
            unit.animationIndex = 0;
        }

        public static void SetAnimationString(Unit unit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(unit.animationString.Substring(0, 2)); // Race and type is always the same

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

            switch (unit.facing)
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

            sb.Append(unit.animationIndex.ToString());

            unit.animationString = sb.ToString();
        }
    }
}
