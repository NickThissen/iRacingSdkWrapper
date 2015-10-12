using System;
using System.Diagnostics;
using iRacingSdkWrapper;
using iRacingSimulator.Events;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public class DriverPitInfo
    {
        public DriverPitInfo(Driver driver)
        {
            _driver = driver;
        }

        private readonly Driver _driver;

        public int Pitstops { get; set; }

        public bool InPitLane { get; set; }
        public bool InPitStall { get; set; }

        public DateTime? PitLaneEntryTime { get; set; }
        public DateTime? PitStallEntryTime { get; set; }
        public DateTime? PitStallExitTime { get; set; }

        public double LastPitLaneTimeSeconds { get; set; }
        public double LastPitStallTimeSeconds { get; set; }

        public double CurrentPitLaneTimeSeconds { get; set; }
        public double CurrentPitStallTimeSeconds { get; set; }

        public int LastPitLap { get; set; }
        public int CurrentStint { get; set; }

        public void CalculatePitInfo()
        {
            if (_driver.Live.TrackSurface == TrackSurfaces.NotInWorld) return;
            
            this.InPitLane = _driver.Live.TrackSurface == TrackSurfaces.AproachingPits ||
                        _driver.Live.TrackSurface == TrackSurfaces.InPitStall;
            this.InPitStall = _driver.Live.TrackSurface == TrackSurfaces.InPitStall;

            this.CurrentStint = _driver.Results.Current.LapsComplete - this.LastPitLap;

            // Are we already in pitlane?
            if (this.PitLaneEntryTime == null)
            {
                // We were not previously in pitlane

                if (this.InPitLane)
                {
                    // We have now entered pitlane
                    this.PitLaneEntryTime = DateTime.UtcNow;
                    this.CurrentPitLaneTimeSeconds = 0;
                    
                    Sim.Instance.NotifyPitstop(RaceEvent.EventTypes.PitEntry, _driver);
                }
            }
            else
            {
                // We were already in pitlane but have not exited yet
                this.CurrentPitLaneTimeSeconds = (DateTime.UtcNow - this.PitLaneEntryTime.Value).TotalSeconds;

                // Are we already in pit stall?
                if (this.PitStallEntryTime == null)
                {
                    // We were not previously in our pit stall yet

                    if (this.InPitStall)
                    {
                        // We have just entered our pit stall
                        this.PitStallEntryTime = DateTime.UtcNow;
                        this.CurrentPitStallTimeSeconds = 0;
                    }
                }
                else
                {
                    // We already were in our pit stall
                    this.CurrentPitStallTimeSeconds =
                            (DateTime.UtcNow - this.PitStallEntryTime.Value).TotalSeconds;

                    if (!this.InPitStall)
                    {
                        // We have now left our pit stall
                        this.LastPitStallTimeSeconds =
                                (DateTime.UtcNow - this.PitStallEntryTime.Value).TotalSeconds;

                        this.CurrentPitStallTimeSeconds = 0;

                        if (this.PitStallExitTime != null)
                        {
                            var diff = this.PitStallExitTime.Value - DateTime.UtcNow;
                            if (Math.Abs(diff.TotalSeconds) < 5)
                            {
                                // Sim detected pit stall exit again less than 5 seconds after previous exit.
                                // This is not possible
                                return;
                            }
                        }
                        
                        this.Pitstops += 1;
                        this.LastPitLap = _driver.Results.Current.LapsComplete;
                        this.CurrentStint = 0;

                        // Reset
                        this.PitStallEntryTime = null;
                        this.PitStallExitTime = DateTime.UtcNow;
                    }
                }
                
                if (!this.InPitLane)
                {
                    // We have now left pitlane

                    this.LastPitLaneTimeSeconds =
                        (DateTime.UtcNow - this.PitLaneEntryTime.Value).TotalSeconds;
                    this.CurrentPitLaneTimeSeconds = 0;

                    Sim.Instance.NotifyPitstop(RaceEvent.EventTypes.PitExit, _driver);

                    // Reset
                    this.PitLaneEntryTime = null;
                }
            }
        }
    }
}
