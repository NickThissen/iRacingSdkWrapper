using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSimulator
{
    public class Sector : NotifyPropertyChanged
    {
        private double _enterSessionTime;
        private Laptime _sectorTime;

        public int Number { get; set; }
        public float StartPercentage { get; set; }

        public double EnterSessionTime
        {
            get { return _enterSessionTime; }
            set
            {
                if (value.Equals(_enterSessionTime)) return;
                _enterSessionTime = value;
                OnPropertyChanged();
            }
        }

        public Laptime SectorTime
        {
            get { return _sectorTime; }
            set
            {
                if (Equals(value, _sectorTime)) return;
                _sectorTime = value;
                OnPropertyChanged();
            }
        }

        public Sector Copy()
        {
            var s = new Sector();
            s.Number = this.Number;
            s.StartPercentage = this.StartPercentage;
            return s;
        }
    }
}
