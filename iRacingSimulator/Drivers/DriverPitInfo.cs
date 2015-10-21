using System;
using System.Diagnostics;
using iRacingSdkWrapper;
using iRacingSimulator.Events;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public class DriverPitInfo
    {
        private const float PIT_MINSPEED = 0.01f;

        public DriverPitInfo(Driver driver)
        {
            _driver = driver;
        }

        private readonly Driver _driver;
        private bool _hasIncrementedCounter;

        public int Pitstops { get; set; }

        public bool InPitLane { get; set; }
        public bool InPitStall { get; set; }

        public double? PitLaneEntryTime { get; set; }
        public double? PitLaneExitTime { get; set; }

        public double? PitStallEntryTime { get; set; }
        public double? PitStallExitTime { get; set; }

        public double LastPitLaneTimeSeconds { get; set; }
        public double LastPitStallTimeSeconds { get; set; }

        public double CurrentPitLaneTimeSeconds { get; set; }
        public double CurrentPitStallTimeSeconds { get; set; }

        public int LastPitLap { get; set; }
        public int CurrentStint { get; set; }

        public void CalculatePitInfo(double time)
        {
            // If we are not in the world (blinking?), stop checking
            if (_driver.Live.TrackSurface == TrackSurfaces.NotInWorld)
            {
                //Debug.WriteLine("PIT: driver not in world, stopping.");
                return;
            }


            //Debug.WriteLine("PIT: tracksurface: " + _driver.Live.TrackSurface);

            // Are we NOW in pit lane (pitstall includes pitlane)
            this.InPitLane = _driver.Live.TrackSurface == TrackSurfaces.AproachingPits ||
                        _driver.Live.TrackSurface == TrackSurfaces.InPitStall;

            // Are we NOW in pit stall?
            this.InPitStall = _driver.Live.TrackSurface == TrackSurfaces.InPitStall;


            this.CurrentStint = _driver.Results.Current.LapsComplete - this.LastPitLap;

            // Were we already in pitlane previously?
            if (this.PitLaneEntryTime == null)
            {
                //Debug.WriteLine("PIT: not previously in pitlane.");

                // We were not previously in pitlane
                if (this.InPitLane)
                {
                    //Debug.WriteLine("PIT: entered pitlane.");

                    // We have only just now entered pitlane
                    this.PitLaneEntryTime = time;
                    this.CurrentPitLaneTimeSeconds = 0;
                    
                    Sim.Instance.NotifyPitstop(RaceEvent.EventTypes.PitEntry, _driver);
                }
            }
            else
            {
                //Debug.WriteLine("PIT: was already in pitlane.");

                // We were already in pitlane but have not exited yet
                this.CurrentPitLaneTimeSeconds = time - this.PitLaneEntryTime.Value;

               // Debug.WriteLine("PIT: pit time: " + this.CurrentPitLaneTimeSeconds.ToString("0.0"));

                // Were we already in pit stall?
                if (this.PitStallEntryTime == null)
                {
                    //Debug.WriteLine("PIT: not previously in pitstall.");

                    // We were not previously in our pit stall yet
                    if (this.InPitStall)
                    {
                        if (Math.Abs(_driver.Live.Speed) > PIT_MINSPEED)
                        {
                            Debug.WriteLine("PIT: did not stop in pit stall, ignored.");
                        }
                        else
                        {
                            // We have only just now entered our pit stall
                            //Debug.WriteLine("PIT: entered pitstall.");

                            this.PitStallEntryTime = time;
                            this.CurrentPitStallTimeSeconds = 0;
                        }
                    }
                }
                else
                {
                    //Debug.WriteLine("PIT: was already in pitstall.");

                    // We already were in our pit stall
                    this.CurrentPitStallTimeSeconds = time - this.PitStallEntryTime.Value;

                    //Debug.WriteLine("PIT: pit stall time: " + this.CurrentPitStallTimeSeconds.ToString("0.0"));

                    if (!this.InPitStall)
                    {
                        // We have now left our pit stall
                        //Debug.WriteLine("PIT: left pitstall.");

                        this.LastPitStallTimeSeconds = time - this.PitStallEntryTime.Value;

                        this.CurrentPitStallTimeSeconds = 0;

                        if (this.PitStallExitTime != null)
                        {
                            var diff = this.PitStallExitTime.Value - time;
                            if (Math.Abs(diff) < 5)
                            {
                                Debug.WriteLine("PIT: DUPLICATE PIT EXIT DETECTED.");
                                // Sim detected pit stall exit again less than 5 seconds after previous exit.
                                // This is not possible?
                                return;
                            }
                        }

                        // Did we already count this stop?
                        if (!_hasIncrementedCounter)
                        {
                            // Now increment pitstop count
                            this.Pitstops += 1;
                            _hasIncrementedCounter = true;
                            //Debug.WriteLine("---- INCREMENTING PITSTOP! New stops: " + this.Pitstops);
                        }
                        else
                        {
                            //Debug.WriteLine("!! Duplicate pit stop prevented");
                        }
                        
                        this.LastPitLap = _driver.Results.Current.LapsComplete;
                        this.CurrentStint = 0;

                        // Reset
                        this.PitStallEntryTime = null;
                        this.PitStallExitTime = time;
                    }
                }
                
                if (!this.InPitLane)
                {
                    // We have now left pitlane
                    this.PitLaneExitTime = time;
                    _hasIncrementedCounter = false;

                    this.LastPitLaneTimeSeconds = this.PitLaneExitTime.Value - this.PitLaneEntryTime.Value;
                    this.CurrentPitLaneTimeSeconds = 0;

                    Sim.Instance.NotifyPitstop(RaceEvent.EventTypes.PitExit, _driver);

                    // Reset
                    this.PitLaneEntryTime = null;
                }
            }
        }
    }
}
