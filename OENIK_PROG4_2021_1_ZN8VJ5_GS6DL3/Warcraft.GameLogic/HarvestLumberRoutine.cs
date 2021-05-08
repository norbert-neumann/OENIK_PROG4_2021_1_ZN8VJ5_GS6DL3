namespace Warcraft.Model
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Controls a unit to move to a tree, harvest it, then return to Hall.
    /// </summary>
    public class HarvestLumberRoutine : Routine
    {
        /// <summary>
        /// The current target object (hall or gold mine).
        /// </summary>
        public GameObject TargetObject;

        // Hall -> TargetA
        // Mine -> TargetB
        private DateTime startTime;
        private TimeSpan waitTime;
        private bool mining = false;
        private GameModel model;
        private OwnerEnum owner;

        /// <summary>
        /// Return gold to this object.
        /// </summary>
        private Building hall;

        /// <summary>
        /// Tree to harvest.
        /// </summary>
        private CombatObject tree;

        /// <summary>
        /// Initializes a new instance of the <see cref="HarvestLumberRoutine"/> class.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        /// <param name="waitTime">Number of seconds to mine.</param>
        /// <param name="hall">Hall.</param>
        /// <param name="tree">Treee.</param>
        /// <param name="model">Game model to operate on.</param>
        /// <param name="owner">Reciever of the lumber.</param>
        public HarvestLumberRoutine(Unit unit, TimeSpan waitTime, Building hall, CombatObject tree, GameModel model, OwnerEnum owner)
            : base(unit, hall.Position, tree.Position)
        {
            this.waitTime = waitTime;
            this.hall = hall;
            this.tree = tree;
            this.TargetObject = tree;
            this.model = model;
            this.owner = owner;
        }

        /// <summary>
        /// Updates the unit's state and target.
        /// </summary>
        /// <returns>Returns true if the routine is active.</returns>
        public override bool Update()
        {
            if (this.unit.IsPositionInHitbox(this.hall))
            {
                return this.ReachedTargetA();
            }
            else if (this.unit.IsPositionInHitbox(this.tree))
            {
                return this.ReachedTargetB();
            }
            else
            {
                this.unit.Target = this.targetB;
            }

            return false;
        }

        /// <summary>
        /// Commands the unit to mine.
        /// </summary>
        /// <returns>False always.</returns>
        protected override bool ReachedTargetA()
        {
            this.model.AddLumber(this.owner, 17);

            this.unit.UnitState = UnitStateEnum.Walking;
            this.unit.Target = this.targetB;
            this.TargetObject = this.tree;

            return false;
        }

        /// <summary>
        /// Checks if the mining is over or not. If it's over commands the unit to hall.
        /// Else kepps minig.
        /// </summary>
        /// <returns>True if the unit is minig.</returns>
        protected override bool ReachedTargetB()
        {
            if (this.tree.Health <= 0)
            {
                this.unit.UnitState = UnitStateEnum.Walking;
                this.unit.InIdle = true;
                return false;
            }

            if (this.mining)
            {
                if (DateTime.Now - this.startTime >= this.waitTime)
                {
                    this.unit.UnitState = UnitStateEnum.WalkingWithLumber;
                    this.unit.Target = this.targetA;
                    this.mining = false;
                    this.unit.Position = this.unit.EntryPoint;
                    this.TargetObject = this.hall;

                    if (this.tree.AcceptDamage(17))
                    {
                        if (this.tree is Building)
                        {
                            this.model.BuildingsToRemove.Add(this.tree as Building);
                        }
                        else
                        {
                            this.model.TreesToRemove.Add(this.tree);
                        }
                    }

                    return false;
                }

                return true;
            }
            else
            {
                this.startTime = DateTime.Now;
                this.mining = true;
                this.unit.UnitState = UnitStateEnum.Fighting;
                this.unit.Target = this.targetB;
                return true;
            }
        }
    }
}
