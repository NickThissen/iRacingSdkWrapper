using System.Collections.Generic;
using System.Linq;
using iRacingSdkWrapper;
using iRacingSdkWrapper.Bitfields;
using iRacingSimulator.Drivers;

namespace iRacingSimulator
{
    public class SessionData : NotifyPropertyChanged
    {
        private Track _track;
        private string _eventType;
        private int _subsessionId;
        private double _sessionTime;
        private double _timeRemaining;
        private int _leaderLap;
        private bool _trackCleanup;
        private bool _dynamicTrack;
        private TrackConditions.TrackUsageTypes _trackUsage;
        private string _trackUsageText;
        private string _raceLaps;
        private double _raceTime;
        private BestLap _overallBestLap;
        private SessionFlag _flags;
        private SessionStates _state;
        private bool _isFinished;

        public SessionData()
        {
            this.ClassBestLaps = new Dictionary<int, BestLap>();
        }

        public Track Track
        {
            get { return _track; }
            set
            {
                if (Equals(value, _track)) return;
                _track = value;
                OnPropertyChanged();
            }
        }

        public string EventType
        {
            get { return _eventType; }
            set
            {
                if (value == _eventType) return;
                _eventType = value;
                OnPropertyChanged();
            }
        }

        public int SubsessionId
        {
            get { return _subsessionId; }
            set
            {
                if (value == _subsessionId) return;
                _subsessionId = value;
                OnPropertyChanged();
            }
        }

        public double SessionTime
        {
            get { return _sessionTime; }
            set
            {
                if (value.Equals(_sessionTime)) return;
                _sessionTime = value;
                OnPropertyChanged();
            }
        }

        public double TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                if (value.Equals(_timeRemaining)) return;
                _timeRemaining = value;
                OnPropertyChanged();
            }
        }

        public int LeaderLap
        {
            get { return _leaderLap; }
            set
            {
                if (value == _leaderLap) return;
                _leaderLap = value;
                OnPropertyChanged();
            }
        }

        public bool TrackCleanup
        {
            get { return _trackCleanup; }
            set
            {
                if (value == _trackCleanup) return;
                _trackCleanup = value;
                OnPropertyChanged();
            }
        }

        public bool DynamicTrack
        {
            get { return _dynamicTrack; }
            set
            {
                if (value == _dynamicTrack) return;
                _dynamicTrack = value;
                OnPropertyChanged();
            }
        }

        public TrackConditions.TrackUsageTypes TrackUsage
        {
            get { return _trackUsage; }
            set
            {
                if (value == _trackUsage) return;
                _trackUsage = value;
                OnPropertyChanged();
            }
        }

        public string TrackUsageText
        {
            get { return _trackUsageText; }
            set
            {
                if (value == _trackUsageText) return;
                _trackUsageText = value;
                OnPropertyChanged();
            }
        }

        public string RaceLaps
        {
            get { return _raceLaps; }
            set
            {
                if (value == _raceLaps) return;
                _raceLaps = value;
                OnPropertyChanged();
            }
        }

        public double RaceTime
        {
            get { return _raceTime; }
            set
            {
                if (value.Equals(_raceTime)) return;
                _raceTime = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<int, BestLap> ClassBestLaps { get; private set; }

        public BestLap OverallBestLap
        {
            get { return _overallBestLap; }
            set
            {
                if (Equals(value, _overallBestLap)) return;
                _overallBestLap = value;
                OnPropertyChanged();
            }
        }

        public SessionFlag Flags
        {
            get { return _flags; }
            set
            {
                if (Equals(value, _flags)) return;
                _flags = value;
                OnPropertyChanged();
            }
        }

        public SessionStates State
        {
            get { return _state; }
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public bool IsFinished
        {
            get { return _isFinished; }
            set
            {
                if (value == _isFinished) return;
                _isFinished = value;
                OnPropertyChanged();
            }
        }

        public void Update(SessionInfo info)
        {
            this.Track = Track.FromSessionInfo(info);

            var weekend = info["WeekendInfo"];
            this.SubsessionId = Parser.ParseInt(weekend["SubSessionID"].GetValue());

            var session = info["SessionInfo"]["Sessions"]["SessionNum", Sim.Instance.CurrentSessionNumber];
            this.EventType = session["SessionType"].GetValue();

            this.TrackUsageText = session["SessionTrackRubberState"].GetValue();
            this.TrackUsage = TrackConditions.TrackUsageFromString(this.TrackUsageText);
            
            this.TrackCleanup = weekend["TrackCleanup"].GetValue() == "1"; 
            this.DynamicTrack = weekend["TrackDynamicTrack"].GetValue() == "1";

            var laps = session["SessionLaps"].GetValue();
            var time = Parser.ParseSec(session["SessionTime"].GetValue());
            
            this.RaceLaps = laps;
            this.RaceTime = time;
        }

        public void Update(TelemetryInfo telemetry)
        {
            this.SessionTime = telemetry.SessionTime.Value;
            this.TimeRemaining = telemetry.SessionTimeRemain.Value;
            this.Flags = telemetry.SessionFlags.Value;
        }

        public void UpdateState(SessionStates state)
        {
            this.State = state;
            this.IsFinished = state == SessionStates.CoolDown;
        }

        public BestLap UpdateFastestLap(Laptime lap, Driver driver)
        {
            var classId = driver.Car.CarClassId;
            if (!this.ClassBestLaps.ContainsKey(classId))
            {
                this.ClassBestLaps.Add(classId, BestLap.Default);
            }

            if (lap.Value > 0 && this.ClassBestLaps[classId].Laptime.Value > lap.Value)
            {
                var bestlap = new BestLap(lap, driver);
                this.ClassBestLaps[classId] = bestlap;

                this.OverallBestLap =
                    this.ClassBestLaps.Values.Where(l => l.Laptime.Value > 0)
                        .OrderBy(l => l.Laptime.Value)
                        .FirstOrDefault();

                return bestlap;
            }
            return null;
        }

    }
}
