using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;

namespace iRacingSdkWrapper.Broadcast
{
    /// <summary>
    /// Provides control over the pit commands.
    /// </summary>
    public class PitCommandControl : BroadcastBase
    {
        internal PitCommandControl(SdkWrapper wrapper) : base(wrapper)
        {
        }

        /// <summary>
        /// Schedule to add the specified amount of fuel (in liters) in the next pitstop.
        /// </summary>
        /// <param name="amount">The amount of fuel (in liters) to add. Use 0 to leave at current value.</param>
        public void AddFuel(int amount)
        {
            Broadcast(BroadcastMessageTypes.PitCommand, (int)PitCommandModeTypes.Fuel, amount, 0);
        }

        private void ChangeTire(PitCommandModeTypes type, int pressure)
        {
            Broadcast(BroadcastMessageTypes.PitCommand, (int)type, pressure);
        }

        /// <summary>
        /// Schedule to change one or more tires and set their new pressures.
        /// </summary>
        /// <param name="change">The scheduled tire changes.</param>
        public void ChangeTires(TireChange change)
        {
            if (change.LeftFront != null && change.LeftFront.Change)
                ChangeTire(PitCommandModeTypes.LF, change.LeftFront.Pressure);

            if (change.RightFront != null && change.RightFront.Change)
                ChangeTire(PitCommandModeTypes.RF, change.RightFront.Pressure);

            if (change.LeftRear != null && change.LeftRear.Change)
                ChangeTire(PitCommandModeTypes.LR, change.LeftRear.Pressure);

            if (change.RightRear != null && change.RightRear.Change)
                ChangeTire(PitCommandModeTypes.RR, change.RightRear.Pressure);
        }
        
        /// <summary>
        /// Schedule to use a windshield tear-off in the next pitstop.
        /// </summary>
        public void Tearoff()
        {
            Broadcast(BroadcastMessageTypes.PitCommand, (int)PitCommandModeTypes.WS, 0);
        }

        /// <summary>
        /// Schedule to use a fast repair in the next pitstop.
        /// </summary>
        public void FastRepair()
        {
            Broadcast(BroadcastMessageTypes.PitCommand, (int) PitCommandModeTypes.FastRepair, 0);
        }

        /// <summary>
        /// Clear all pit commands.
        /// </summary>
        public void Clear()
        {
            Broadcast(BroadcastMessageTypes.PitCommand, (int)PitCommandModeTypes.Clear, 0);
        }

        /// <summary>
        /// Clear all tire changes.
        /// </summary>
        public void ClearTires()
        {
            Broadcast(BroadcastMessageTypes.PitCommand, (int)PitCommandModeTypes.ClearTires, 0);
        }

        public class Tire
        {
            internal Tire() { }

            /// <summary>
            /// Whether or not to change this tire.
            /// </summary>
            public bool Change { get; set; }

            /// <summary>
            /// The new pressure (in kPa) of this tire.
            /// </summary>
            public int Pressure { get; set; }
        }

        /// <summary>
        /// Encapsulates scheduled tire changes for each of the four tires separately.
        /// </summary>
        public class TireChange
        {
            public TireChange()
            {
                this.LeftFront = new Tire();
                this.RightFront = new Tire();
                this.LeftRear = new Tire();
                this.RightRear = new Tire();
            }

            public Tire LeftFront { get; set; }
            public Tire RightFront { get; set; }
            public Tire LeftRear { get; set; }
            public Tire RightRear { get; set; }
        }


    }
}
