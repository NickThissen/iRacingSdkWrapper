using System;
using System.Collections.Generic;
using System.Diagnostics;
using iRacingSdkWrapper;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public class DriverLiveInfo : NotifyPropertyChanged
    {
        private const float SPEED_CALC_INTERVAL = 0.5f;

        private double _prevSpeedUpdateTime;
        private double _prevSpeedUpdateDist;
        private int _position;
        private int _classPosition;
        private int _lap;
        private float _lapDistance;
        private TrackSurfaces _trackSurface;
        private int _gear;
        private float _rpm;
        private double _steeringAngle;
        private double _speed;
        private double _speedKph;
        private string _deltaToLeader;
        private string _deltaToNext;
        private int _currentSector;
        private int _currentFakeSector;

        public DriverLiveInfo(Driver driver)
        {
            _driver = driver;
        }

        private readonly Driver _driver;

        public Driver Driver
        {
            get { return _driver; }
        }

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
            private set
            {
                if (value == _lap) return;
                _lap = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalLapDistance));
            }
        }

        public float LapDistance
        {
            get { return _lapDistance; }
            private set
            {
                if (value.Equals(_lapDistance)) return;
                _lapDistance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalLapDistance));
            }
        }

        public float TotalLapDistance
        {
            get { return Lap + LapDistance; }
        }

        public TrackSurfaces TrackSurface
        {
            get { return _trackSurface; }
            private set
            {
                if (value == _trackSurface) return;
                _trackSurface = value;
                OnPropertyChanged();
            }
        }

        public int Gear
        {
            get { return _gear; }
            private set
            {
                if (value == _gear) return;
                _gear = value;
                OnPropertyChanged();
            }
        }

        public float Rpm
        {
            get { return _rpm; }
            private set
            {
                if (value.Equals(_rpm)) return;
                _rpm = value;
                OnPropertyChanged();
            }
        }

        public double SteeringAngle
        {
            get { return _steeringAngle; }
            private set
            {
                if (value.Equals(_steeringAngle)) return;
                _steeringAngle = value;
                OnPropertyChanged();
            }
        }

        public double Speed
        {
            get { return _speed; }
            private set
            {
                if (value.Equals(_speed)) return;
                _speed = value;
                OnPropertyChanged();
            }
        }

        public double SpeedKph
        {
            get { return _speedKph; }
            private set
            {
                if (value.Equals(_speedKph)) return;
                _speedKph = value;
                OnPropertyChanged();
            }
        }

        public string DeltaToLeader
        {
            get { return _deltaToLeader; }
            set
            {
                if (value == _deltaToLeader) return;
                _deltaToLeader = value;
                OnPropertyChanged();
            }
        }

        public string DeltaToNext
        {
            get { return _deltaToNext; }
            set
            {
                if (value == _deltaToNext) return;
                _deltaToNext = value;
                OnPropertyChanged();
            }
        }

        public int CurrentSector
        {
            get { return _currentSector; }
            set
            {
                if (value == _currentSector) return;
                _currentSector = value;
                OnPropertyChanged();
            }
        }

        public int CurrentFakeSector
        {
            get { return _currentFakeSector; }
            set
            {
                if (value == _currentFakeSector) return;
                _currentFakeSector = value;
                OnPropertyChanged();
            }
        }

        public void ParseTelemetry(TelemetryInfo e)
        {
            this.Lap = e.CarIdxLap.Value[this.Driver.Id];
            this.LapDistance = e.CarIdxLapDistPct.Value[this.Driver.Id];
            this.TrackSurface = e.CarIdxTrackSurface.Value[this.Driver.Id];

            this.Gear = e.CarIdxGear.Value[this.Driver.Id];
            this.Rpm = e.CarIdxRPM.Value[this.Driver.Id];
            this.SteeringAngle = e.CarIdxSteer.Value[this.Driver.Id];

            this.Driver.PitInfo.CalculatePitInfo(e.SessionTime.Value);
        }

        public void CalculateSpeed(TelemetryInfo current, double? trackLengthKm)
        {
            if (current == null) return;
            if (trackLengthKm == null) return;

            try
            {
                var t1 = current.SessionTime.Value;
                var t0 = _prevSpeedUpdateTime;
                var time = t1 - t0;

                if (time < SPEED_CALC_INTERVAL)
                {
                    // Ignore
                    return;
                }

                var p1 = current.CarIdxLapDistPct.Value[this.Driver.Id];
                var p0 = _prevSpeedUpdateDist;

                if (p1 < -0.5 || _driver.Live.TrackSurface == TrackSurfaces.NotInWorld)
                {
                    // Not in world?
                    return;
                }

                if (p0 - p1 > 0.5)
                {
                    // Lap crossing
                    p1 += 1;
                }
                var distancePct = p1 - p0;

                var distance = distancePct*trackLengthKm.GetValueOrDefault()*1000; //meters


                if (time >= Double.Epsilon)
                {
                    this.Speed = distance/(time); // m/s
                }
                else
                {
                    if (distance < 0)
                        this.Speed = Double.NegativeInfinity;
                    else
                        this.Speed = Double.PositiveInfinity;
                }
                this.SpeedKph = this.Speed * 3.6;

                _prevSpeedUpdateTime = t1;
                _prevSpeedUpdateDist = p1;
            }
            catch (Exception ex)
            {
                //Log.Instance.LogError("Calculating speed of car " + this.Driver.Id, ex);
                this.Speed = 0;
            }
        }
    }
}
