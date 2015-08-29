using System;
using iRacingSdkWrapper;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public class DriverLiveInfo
    {
        public DriverLiveInfo(Driver driver)
        {
            _driver = driver;
        }

        private readonly Driver _driver;

        public Driver Driver
        {
            get { return _driver; }
        }

        public int Position { get; set; }
        public int ClassPosition { get; set; }
        public int Lap { get; set; }
        public float LapDistance { get; set; }

        public float TotalLapDistance
        {
            get { return Lap + LapDistance; }
        }

        public TrackSurfaces TrackSurface { get; set; }

        public double Speed { get; set; }

        public string DeltaToLeader { get; set; }
        public string DeltaToNext { get; set; }
        
        public void ParseTelemetry(TelemetryInfo e)
        {
            this.Lap = e.CarIdxLap.Value[this.Driver.Id];
            this.LapDistance = e.CarIdxLapDistPct.Value[this.Driver.Id];
            this.TrackSurface = e.CarIdxTrackSurface.Value[this.Driver.Id];

            this.Driver.PitInfo.CalculatePitInfo();
        }

        public void CalculateSpeed(TelemetryInfo previous, TelemetryInfo current, double? trackLengthKm)
        {
            if (previous == null || current == null) return;
            if (trackLengthKm == null) return;

            try
            {
                var distancePct = current.CarIdxLapDistPct.Value[this.Driver.Id] -
                                  previous.CarIdxLapDistPct.Value[this.Driver.Id];
                var distance = distancePct*trackLengthKm.GetValueOrDefault();

                var time = current.SessionTime.Value - previous.SessionTime.Value;

                if (time >= Double.Epsilon)
                {
                    this.Speed = distance/time;
                }
                else
                {
                    if (distance < 0)
                        this.Speed = Double.NegativeInfinity;
                    else
                        this.Speed = Double.PositiveInfinity;
                }
            }
            catch (Exception ex)
            {
                //Log.Instance.LogError("Calculating speed of car " + this.Driver.Id, ex);
                this.Speed = 0;
            }
        }
    }
}
