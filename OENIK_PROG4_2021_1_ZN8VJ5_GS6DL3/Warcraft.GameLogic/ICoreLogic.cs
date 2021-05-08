namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Contaions all funcitonalities that the CoreLogic should have.
    /// </summary>
    public interface ICoreLogic
    {
        /// <summary>
        /// One step of the game.
        /// </summary>
        /// <returns> Winner type.</returns>
        OwnerEnum Step();

        /// <summary>
        /// Updates each unit's sprite.
        /// </summary>
        void UpdateAnimation();

        /// <summary>
        /// Selects an Object, Subject, or a Point depoending on the cursorPos.
        /// </summary>
        /// <param name="cursorPos">User's cursor position.</param>
        void Select(Point cursorPos);

        /// <summary>
        /// Commands the SelectedSubject to mine gold at the SelectedObject.
        /// </summary>
        void MineGold();

        /// <summary>
        /// Commands the SelectedSubject to harvest lumber at the SelectedObject.
        /// </summary>
        void HarvestLumber();

        /// <summary>
        /// Commands the SelectedSubject to paroll between the current posoiton and the SelectedPoint.
        /// </summary>
        void PatrollUnit();

        /// <summary>
        /// Commands the SelectedSubject to attack the SelectedSubject.
        /// </summary>
        void SetUnitsEnemy();

        /// <summary>
        /// Commands the SelectedSubject to move to the SelectedPoint.
        /// </summary>
        void GoTo();
    }
}
