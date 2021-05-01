namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// This interface deals with animationString and animationIndex.
    /// </summary>
    public interface IAnimationLogic
    {
        /// <summary>
        /// Sets the animationString for all units.
        /// </summary>
        public void UpdateSprites();

        /// <summary>
        /// Resets a unit's animation index.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        public void ResetAnimationIndex(Unit unit);

        /// <summary>
        /// Increments and resets the animation index.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        public void IncrementAnimationIndex(Unit unit);
    }
}
