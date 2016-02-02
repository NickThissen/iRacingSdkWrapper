using System;
using System.Collections.Generic;
using System.Linq;
using iRacingSdkWrapper;

namespace iRacingSimulator.Drivers
{
    /// <summary>
    /// Represents a dictionary of session results for a driver. Contains results for all sessions.
    /// </summary>
    public class DriverResults
    {
        private int _currentSessionNumber;

        public DriverResults(Driver driver)
        {
            _driver = driver;
            _sessions = new Dictionary<int, DriverSessionResults>();
        }

        private readonly Dictionary<int, DriverSessionResults> _sessions;
        /// <summary>
        /// Gets the dictionary of session results for this driver.
        /// </summary>
        public Dictionary<int, DriverSessionResults> Sessions { get { return _sessions; } }

        private readonly Driver _driver;
        /// <summary>
        /// Gets the driver object.
        /// </summary>
        public Driver Driver { get { return _driver; } }

        /// <summary>
        /// Checks if this driver is present in the results for the specified session.
        /// </summary>
        public bool HasResult(int sessionNumber)
        {
            return _sessions.ContainsKey(sessionNumber);
        }

        internal void SetResults(int sessionNumber, YamlQuery query, int position)
        {
            if (!this.HasResult(sessionNumber))
            {
                _sessions.Add(sessionNumber, new DriverSessionResults(_driver, sessionNumber));
            }
            _currentSessionNumber = sessionNumber;
            var results = this[sessionNumber];

            results.ParseYaml(query, position);
        }

        /// <summary>
        /// Gets the session results for this driver for the specified session number, or empty results if he does not appear in the results.
        /// </summary>
        public DriverSessionResults FromSession(int sessionNumber)
        {
            if (this.HasResult(sessionNumber)) return _sessions[sessionNumber];
            return new DriverSessionResults(_driver, sessionNumber);
        }

        /// <summary>
        /// Gets the session results for this driver for the specified session number, or empty results if he does not appear in the results.
        /// </summary>
        public DriverSessionResults this[int sessionNumber]
        {
            get { return this.FromSession(sessionNumber); }
        }

        public DriverSessionResults Current
        {
            get { return this.FromSession(_currentSessionNumber); }
        }
    }

    /// <summary>
    /// Represents the session results for a single driver in a single session.
    /// </summary>
    [Serializable]
    public class DriverSessionResults : NotifyPropertyChanged
    {
        public DriverSessionResults(Driver driver, int sessionNumber)
        {
            _driver = driver;
            _sessionNumber = sessionNumber;

            this.Laps = new LaptimeCollection();
            this.IsEmpty = true;

            this.FakeSectorTimes = new[]
                    {
                        new Sector() {Number = 0, StartPercentage = 0f},
                        new Sector() {Number = 1, StartPercentage = 0.333f},
                        new Sector() {Number = 2, StartPercentage = 0.666f}
                    };
        }

        private readonly Driver _driver;
        public Driver Driver { get { return _driver; } }

        private readonly int _sessionNumber;
        private int _position;
        private int _classPosition;
        private int _lap;
        private Laptime _time;
        private int _fastestLap;
        private Laptime _fastestTime;
        private Laptime _lastTime;
        private Laptime _averageTime;
        private int _lapsLed;
        private int _lapsComplete;
        private int _lapsDriven;
        private LaptimeCollection _laps;
        private Sector[] _sectorTimes;
        private Sector[] _fakeSectorTimes;
        private string _outReason;
        private int _outReasonId;
        public int SessionNumber { get { return _sessionNumber; } }

        public bool IsEmpty { get; set; }

        public int Position
        {
            get { return _position; }
            set
            {
                if (value == _position) return;
                _position = value;
                OnPropertyChanged();
            }
        }

        public int ClassPosition
        {
            get { return _classPosition; }
            set
            {
                if (value == _classPosition) return;
                _classPosition = value;
                OnPropertyChanged();
            }
        }

        public int Lap
        {
            get { return _lap; }
            set
            {
                if (value == _lap) return;
                _lap = value;
                OnPropertyChanged();
            }
        }

        public Laptime Time
        {
            get { return _time; }
            set
            {
                if (Equals(value, _time)) return;
                _time = value;
                OnPropertyChanged();
            }
        }

        public int FastestLap
        {
            get { return _fastestLap; }
            set
            {
                if (value == _fastestLap) return;
                _fastestLap = value;
                OnPropertyChanged();
            }
        }

        public Laptime FastestTime
        {
            get { return _fastestTime; }
            set
            {
                if (Equals(value, _fastestTime)) return;
                _fastestTime = value;
                OnPropertyChanged();
            }
        }

        public Laptime LastTime
        {
            get { return _lastTime; }
            set
            {
                if (Equals(value, _lastTime)) return;
                _lastTime = value;
                OnPropertyChanged();
            }
        }

        public Laptime AverageTime
        {
            get { return _averageTime; }
            set
            {
                if (Equals(value, _averageTime)) return;
                _averageTime = value;
                OnPropertyChanged();
            }
        }

        public int LapsLed
        {
            get { return _lapsLed; }
            set
            {
                if (value == _lapsLed) return;
                _lapsLed = value;
                OnPropertyChanged();
            }
        }

        public int LapsComplete
        {
            get { return _lapsComplete; }
            set
            {
                if (value == _lapsComplete) return;
                _lapsComplete = value;
                OnPropertyChanged();
            }
        }

        public int LapsDriven
        {
            get { return _lapsDriven; }
            set
            {
                if (value == _lapsDriven) return;
                _lapsDriven = value;
                OnPropertyChanged();
            }
        }

        public LaptimeCollection Laps
        {
            get { return _laps; }
            set
            {
                if (Equals(value, _laps)) return;
                _laps = value;
                OnPropertyChanged();
            }
        }

        public Sector[] SectorTimes
        {
            get { return _sectorTimes; }
            set
            {
                if (Equals(value, _sectorTimes)) return;
                _sectorTimes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SectorsDisplay));
            }
        }

        public Sector[] FakeSectorTimes
        {
            get { return _fakeSectorTimes; }
            set
            {
                if (Equals(value, _fakeSectorTimes)) return;
                _fakeSectorTimes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FakeSector1));
                OnPropertyChanged(nameof(FakeSector2));
                OnPropertyChanged(nameof(FakeSector3));
            }
        }

        public string SectorsDisplay
        {
            get
            {
                if (SectorTimes == null || SectorTimes.Length == 0) return "-";
                return string.Join(",  ", SectorTimes.Select(s => s.SectorTime == null || s.SectorTime.Value == 0 ? "0.00" : s.SectorTime.DisplayShort));
            }
        }

        public Sector FakeSector1
        {
            get { return FakeSectorTimes == null || FakeSectorTimes.Length == 0 ? null : FakeSectorTimes[0]; }
        }

        public Sector FakeSector2
        {
            get { return FakeSectorTimes == null || FakeSectorTimes.Length == 0 ? null : FakeSectorTimes[1]; }
        }
        
        public Sector FakeSector3
        {
            get { return FakeSectorTimes == null || FakeSectorTimes.Length == 0 ? null : FakeSectorTimes[2]; }
        }

        public string OutReason
        {
            get { return _outReason; }
            set
            {
                if (value == _outReason) return;
                _outReason = value;
                OnPropertyChanged();
            }
        }

        public int OutReasonId
        {
            get { return _outReasonId; }
            set
            {
                if (value == _outReasonId) return;
                _outReasonId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOut));
            }
        }

        public bool IsOut { get { return this.OutReasonId != 0; } }

        internal void ParseYaml(YamlQuery query, int position)
        {
            this.IsEmpty = false;

            this.Position = position;
            this.ClassPosition = Parser.ParseInt(query["ClassPosition"].GetValue()) + 1;

            this.Lap = Parser.ParseInt(query["Lap"].GetValue());
            this.Time = new Laptime(Parser.ParseFloat(query["Time"].GetValue()));
            this.FastestLap = Parser.ParseInt(query["FastestLap"].GetValue());
            this.FastestTime = new Laptime(Parser.ParseFloat(query["FastestTime"].GetValue()));
            this.LastTime = new Laptime(Parser.ParseFloat(query["LastTime"].GetValue()));
            this.LapsLed = Parser.ParseInt(query["LapsLed"].GetValue());

            var previousLaps = this.LapsComplete;
            this.LapsComplete = Parser.ParseInt(query["LapsComplete"].GetValue());
            this.LapsDriven = Parser.ParseInt(query["LapsDriven"].GetValue());

            this.FastestTime.LapNumber = this.FastestLap;
            this.LastTime.LapNumber = this.LapsComplete;

            // Check if a new lap is completed, and add it to Laps
            if (this.LapsComplete > previousLaps)
            {
                this.Laps.Add(this.LastTime);
                this.AverageTime = this.Laps.Average();
            }

            this.OutReasonId = Parser.ParseInt(query["ReasonOutId"].GetValue());
            this.OutReason = query["ReasonOutStr"].GetValue();
        }
    }
}
