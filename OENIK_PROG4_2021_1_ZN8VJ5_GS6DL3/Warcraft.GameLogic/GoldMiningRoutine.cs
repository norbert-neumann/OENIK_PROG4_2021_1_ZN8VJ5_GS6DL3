﻿namespace Warcraft.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Moves to gold mine, mines, then returns home.
    /// </summary>
    public class GoldMiningRoutine : Routine
    {
        /// <summary>
        /// The current target object (hall or gold mine).
        /// </summary>
        public GameObject TargetObject;

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
        /// Gold mine to mine in.
        /// </summary>
        private GoldMine mine;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoldMiningRoutine"/> class.
        /// </summary>
        /// <param name="unit">Unit to operate on.</param>
        /// <param name="waitTime">Number of seconds to mine.</param>
        /// <param name="hall">Hall.</param>
        /// <param name="mine">Gold mine.</param>
        /// <param name="model">Game model to operate on.</param>
        /// <param name="owner">Reciever of the mined gold.</param>
        public GoldMiningRoutine(Unit unit, TimeSpan waitTime, Building hall, GoldMine mine, GameModel model, OwnerEnum owner)
            : base(unit, hall.Position, mine.Position)
        {
            this.unit = unit;
            this.waitTime = waitTime;
            this.hall = hall;
            this.mine = mine;
            this.TargetObject = mine;
            this.model = model;
            this.owner = owner;
        }

        /// <summary>
        /// Updates the unit's state and target.
        /// </summary>
        /// <returns>Returns true if the routine is active.</returns>
        public override bool Update()
        {
            if (this.unit.Collides(this.hall))
            {
                return this.ReachedTargetA();
            }
            else if (this.unit.Collides(this.mine))
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
            this.model.AddGold(this.owner, 5);

            this.unit.UnitState = UnitStateEnum.Walking;
            this.TargetObject = this.mine;
            this.unit.Target = this.targetB;

            return false;
        }

        /// <summary>
        /// Checks if the mining is over or not. If it's over commands the unit to hall.
        /// Else kepps minig.
        /// </summary>
        /// <returns>True if the unit is minig.</returns>
        protected override bool ReachedTargetB()
        {
            if (this.mine.CurrentCapacity <= 0)
            {
                this.unit.UnitState = UnitStateEnum.Walking;
                this.unit.InIdle = true;
                return false;
            }

            if (this.mining)
            {
                if (DateTime.Now - this.startTime >= this.waitTime)
                {
                    this.unit.UnitState = UnitStateEnum.WalkingWithGold;
                    this.unit.Target = this.targetA;
                    this.mining = false;
                    this.unit.Position = this.unit.EntryPoint;
                    this.unit.Hiding = false;
                    this.TargetObject = this.hall;
                    this.mine.NumberOfUsers--;
                    this.unit.AnimationIndex = 0;

                    if (!this.mine.Take(5))
                    {
                        this.model.GoldMinesToRemove.Add(this.mine);
                    }

                    return false;
                }

                return true;
            }
            else
            {
                this.mine.NumberOfUsers++;
                this.startTime = DateTime.Now;
                this.mining = true;
                this.unit.Hiding = true;
                return true;
            }
        }
    }
}
