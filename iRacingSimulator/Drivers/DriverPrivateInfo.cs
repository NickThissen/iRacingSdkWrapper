using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;

namespace iRacingSimulator.Drivers
{
    public class DriverPrivateInfo : NotifyPropertyChanged
    {
        public DriverPrivateInfo(Driver driver)
        {
            _driver = driver;
        }

        private readonly Driver _driver;
        private double _speed;
        private float _throttle;
        private float _brake;
        private float _clutch;
        private float _fuel;
        private float _fuelPercentage;
        private float _fuelPressure;

        public Driver Driver
        {
            get { return _driver; }
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

        public float Throttle
        {
            get { return _throttle; }
            private set
            {
                if (value.Equals(_throttle)) return;
                _throttle = value;
                OnPropertyChanged();
            }
        }

        public float Brake
        {
            get { return _brake; }
            private set
            {
                if (value.Equals(_brake)) return;
                _brake = value;
                OnPropertyChanged();
            }
        }

        public float Clutch
        {
            get { return _clutch; }
            private set
            {
                if (value.Equals(_clutch)) return;
                _clutch = value;
                OnPropertyChanged();
            }
        }

        public float Fuel
        {
            get { return _fuel; }
            private set
            {
                if (value.Equals(_fuel)) return;
                _fuel = value;
                OnPropertyChanged();
            }
        }

        public float FuelPercentage
        {
            get { return _fuelPercentage; }
            private set
            {
                if (value.Equals(_fuelPercentage)) return;
                _fuelPercentage = value;
                OnPropertyChanged();
            }
        }

        public float FuelPressure
        {
            get { return _fuelPressure; }
            private set
            {
                if (value.Equals(_fuelPressure)) return;
                _fuelPressure = value;
                OnPropertyChanged();
            }
        }

        public void ParseTelemetry(TelemetryInfo e)
        {
            this.Speed = e.Speed.Value;
            this.Throttle = e.Throttle.Value;
            this.Brake = e.Brake.Value;
            this.Clutch = e.Clutch.Value;
            this.Fuel = e.FuelLevel.Value;
            this.FuelPercentage = e.FuelLevelPct.Value;
            this.FuelPressure = e.FuelPress.Value;

            // TODO: add remaining parameters
        }
    }
}
