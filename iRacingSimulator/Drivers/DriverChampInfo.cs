using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSimulator.Drivers
{
    public class DriverChampInfo : NotifyPropertyChanged
    {
        private readonly Driver _driver;
        private int _livePosition;
        private int _previousPosition;
        private int _livePoints;
        private int _previousPoints;
        private int _currentRacePoints;

        public DriverChampInfo(Driver driver)
        {
            _driver = driver;
        }

        public int LivePosition
        {
            get { return _livePosition; }
            set
            {
                if (value == _livePosition) return;
                _livePosition = value;
                OnPropertyChanged();
            }
        }

        public int PreviousPosition
        {
            get { return _previousPosition; }
            set
            {
                if (value == _previousPosition) return;
                _previousPosition = value;
                OnPropertyChanged();
            }
        }

        public int LivePoints
        {
            get { return _livePoints; }
            set
            {
                if (value == _livePoints) return;
                _livePoints = value;
                OnPropertyChanged();
            }
        }

        public int PreviousPoints
        {
            get { return _previousPoints; }
            set
            {
                if (value == _previousPoints) return;
                _previousPoints = value;
                OnPropertyChanged();
            }
        }

        public int CurrentRacePoints
        {
            get { return _currentRacePoints; }
            set
            {
                if (value == _currentRacePoints) return;
                _currentRacePoints = value;
                OnPropertyChanged();
            }
        }
    }
}
