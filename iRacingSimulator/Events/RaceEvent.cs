using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSimulator.Drivers;

namespace iRacingSimulator.Events
{
    /// <summary>
    /// Represents a generic event during the race.
    /// </summary>
    public abstract class RaceEvent
    {
        protected RaceEvent()
        {
            this.Time = DateTime.UtcNow;
        }
        
        /// <summary>
        /// The time (UTC) of the event.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The iRacing session time of the event.
        /// </summary>
        public double SessionTime { get; set; }

        /// <summary>
        /// The lap of the event.
        /// </summary>
        public int Lap { get; set; }

        /// <summary>
        /// The type of event.
        /// </summary>
        public abstract EventTypes Type { get; }
        
        public enum EventTypes
        {
            None = 0,
            GreenFlag = 1,
            YellowFlag = 2,
            Winner = 3,
            //PositionChange = 4,
            NewLeader = 5,
            BestLap = 6,
            DriverSwap = 7,
            PitEntry = 8,
            PitExit = 9
        }
    }

    /// <summary>
    /// Represents a generic event concerning a single driver.
    /// </summary>
    public abstract class DriverRaceEvent : RaceEvent
    {
        public Driver Driver { get; set; }
    }

    /// <summary>
    /// Represents a generic event concerning a set of two drivers.
    /// </summary>
    public abstract class DriverSetRaceEvent : RaceEvent
    {
        public Driver Driver1 { get; set; }
        public Driver Driver2 { get; set; }

        public override EventTypes Type { get { return EventTypes.DriverSwap;} }
    }

    public class GreenFlagRaceEvent : RaceEvent
    {
        public override EventTypes Type { get { return EventTypes.GreenFlag; } }
    }

    public class YellowFlagRaceEvent : RaceEvent
    {
        public override EventTypes Type { get { return EventTypes.YellowFlag; } }
    }

    public class WinnerRaceEvent : DriverRaceEvent
    {
        public override EventTypes Type { get { return EventTypes.Winner; } }
    }

//    public class PositionChangeRaceEvent : DriverSetRaceEvent
//    {
//        public override EventTypes Type { get { return EventTypes.PositionChange; } }
//    }

    public class NewLeaderRaceEvent : DriverRaceEvent
    {
        public override EventTypes Type { get { return EventTypes.NewLeader; } }
    }

    public class BestLapRaceEvent : DriverRaceEvent
    {
        public BestLap BestLap { get; set; }

        public override EventTypes Type { get { return EventTypes.BestLap; } }
    }

    public class DriverSwapRaceEvent : DriverRaceEvent
    {
        public int PreviousDriverId { get; set; }
        public string PreviousDriverName { get; set; }

        public int CurrentDriverId { get; set; }
        public string CurrentDriverName { get; set; }

        public override EventTypes Type { get { return EventTypes.DriverSwap; } }
    }

    public class PitEntryRaceEvent : DriverRaceEvent
    {
        public override EventTypes Type { get { return EventTypes.PitEntry; } }
    }

    public class PitExitRaceEvent : DriverRaceEvent
    {
        public override EventTypes Type { get { return EventTypes.PitExit; } }
    }
}
