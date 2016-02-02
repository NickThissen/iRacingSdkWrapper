using System;
using System.Diagnostics;
using iRacingSdkWrapper;
using iRacingSimulator.Events;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public class DriverPitInfo : NotifyPropertyChanged
    {
        private const float PIT_MINSPEED = 0.01f;

        public DriverPitInfo(Driver driver)
        {
            _driver = driver;
        }

        private readonly Driver _driver;
        private bool _hasIncrementedCounter;
        private int _pitstops;
        private bool _inPitLane;
        private bool _inPitStall;
        private double? _pitLaneEntryTime;
        private double? _pitLaneExitTime;
        private double? _pitStallEntryTime;
        private double? _pitStallExitTime;
        private double _lastPitLaneTimeSeconds;
        private double _lastPitStallTimeSeconds;
        private double _currentPitLaneTimeSeconds;
        private double _currentPitStallTimeSeconds;
        private int _lastPitLap;
        private int _currentStint;

        public int Pitstops
        {
            get { return _pitstops; }
            set
            {
                if (value == _pitstops) return;
                _pitstops = value;
                OnPropertyChanged();
            }
        }

        public bool InPitLane
        {
            get { return _inPitLane; }
            set
            {
                if (value == _inPitLane) return;
                _inPitLane = value;
                OnPropertyChanged();
            }
        }

        public bool InPitStall
        {
            get { return _inPitStall; }
            set
            {
                if (value == _inPitStall) return;
                _inPitStall = value;
                OnPropertyChanged();
            }
        }

        public double? PitLaneEntryTime
        {
            get { return _pitLaneEntryTime; }
            set
            {
                if (value.Equals(_pitLaneEntryTime)) return;
                _pitLaneEntryTime = value;
                OnPropertyChanged();
            }
        }

        public double? PitLaneExitTime
        {
            get { return _pitLaneExitTime; }
            set
            {
                if (value.Equals(_pitLaneExitTime)) return;
                _pitLaneExitTime = value;
                OnPropertyChanged();
            }
        }

        public double? PitStallEntryTime
        {
            get { return _pitStallEntryTime; }
            set
            {
                if (value.Equals(_pitStallEntryTime)) return;
                _pitStallEntryTime = value;
                OnPropertyChanged();
            }
        }

        public double? PitStallExitTime
        {
            get { return _pitStallExitTime; }
            set
            {
                if (value.Equals(_pitStallExitTime)) return;
                _pitStallExitTime = value;
                OnPropertyChanged();
            }
        }

        public double LastPitLaneTimeSeconds
        {
            get { return _lastPitLaneTimeSeconds; }
            set
            {
                if (value.Equals(_lastPitLaneTimeSeconds)) return;
                _lastPitLaneTimeSeconds = value;
                OnPropertyChanged();
            }
        }

        public double LastPitStallTimeSeconds
        {
            get { return _lastPitStallTimeSeconds; }
            set
            {
                if (value.Equals(_lastPitStallTimeSeconds)) return;
                _lastPitStallTimeSeconds = value;
                OnPropertyChanged();
            }
        }

        public double CurrentPitLaneTimeSeconds
        {
            get { return _currentPitLaneTimeSeconds; }
            set
            {
                if (value.Equals(_currentPitLaneTimeSeconds)) return;
                _currentPitLaneTimeSeconds = value;
                OnPropertyChanged();
            }
        }

        public double CurrentPitStallTimeSeconds
        {
            get { return _currentPitStallTimeSeconds; }
            set
            {
                if (value.Equals(_currentPitStallTimeSeconds)) return;
                _currentPitStallTimeSeconds = value;
                OnPropertyChanged();
            }
        }

        public int LastPitLap
        {
            get { return _lastPitLap; }
            set
            {
                if (value == _lastPitLap) return;
                _lastPitLap = value;
                OnPropertyChanged();
            }
        }

        public int CurrentStint
        {
            get { return _currentStint; }
            set
            {
                if (value == _currentStint) return;
                _currentStint = value;
                OnPropertyChanged();
            }
        }

        public void CalculatePitInfo(double time)
        {
            // If we are not in the world (blinking?), stop checking
            if (_driver.Live.TrackSurface == TrackSurfaces.NotInWorld)
            {
                return;
            }

            // Are we NOW in pit lane (pitstall includes pitlane)
            this.InPitLane = _driver.Live.TrackSurface == TrackSurfaces.AproachingPits ||
                        _driver.Live.TrackSurface == TrackSurfaces.InPitStall;

            // Are we NOW in pit stall?
            this.InPitStall = _driver.Live.TrackSurface == TrackSurfaces.InPitStall;


            this.CurrentStint = _driver.Results.Current.LapsComplete - this.LastPitLap;

            // Were we already in pitlane previously?
            if (this.PitLaneEntryTime == null)
            {
                // We were not previously in pitlane
                if (this.InPitLane)
                {
                    // We have only just now entered pitlane
                    this.PitLaneEntryTime = time;
                    this.CurrentPitLaneTimeSeconds = 0;
                    
                    Sim.Instance.NotifyPitstop(RaceEvent.EventTypes.PitEntry, _driver);
                }
            }
            else
            {
                // We were already in pitlane but have not exited yet
                this.CurrentPitLaneTimeSeconds = time - this.PitLaneEntryTime.Value;
                
                // Were we already in pit stall?
                if (this.PitStallEntryTime == null)
                {
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

                            this.PitStallEntryTime = time;
                            this.CurrentPitStallTimeSeconds = 0;
                        }
                    }
                }
                else
                {
                    // We already were in our pit stall
                    this.CurrentPitStallTimeSeconds = time - this.PitStallEntryTime.Value;
                    
                    if (!this.InPitStall)
                    {
                        // We have now left our pit stall

                        this.LastPitStallTimeSeconds = time - this.PitStallEntryTime.Value;

                        this.CurrentPitStallTimeSeconds = 0;

                        if (this.PitStallExitTime != null)
                        {
                            var diff = this.PitStallExitTime.Value - time;
                            if (Math.Abs(diff) < 5)
                            {
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
