using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;

namespace iRacingSimulator.Drivers
{
    public class DriverQualyResults : NotifyPropertyChanged
    {
        public DriverQualyResults(Driver driver)
        {
            _driver = driver;
        }

        private readonly Driver _driver;
        private int _position;
        private int _classPosition;
        private Laptime _lap;

        /// <summary>
        /// Gets the driver object.
        /// </summary>
        public Driver Driver { get { return _driver; } }

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

        public Laptime Lap
        {
            get { return _lap; }
            set
            {
                if (Equals(value, _lap)) return;
                _lap = value;
                OnPropertyChanged();
            }
        }

        internal void ParseYaml(YamlQuery query, int position)
        {
            this.Position = position + 1;
            this.ClassPosition = Parser.ParseInt(query["ClassPosition"].GetValue()) + 1;
            this.Lap = new Laptime(Parser.ParseFloat(query["FastestTime"].GetValue()));
            this.Lap.LapNumber = Parser.ParseInt(query["FastestLap"].GetValue());
        }
    }
}
