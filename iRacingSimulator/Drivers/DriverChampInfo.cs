using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSimulator.Drivers
{
    public class DriverChampInfo
    {
        private readonly Driver _driver;

        public DriverChampInfo(Driver driver)
        {
            _driver = driver;
        }

        public int LivePosition { get; set; }
        public int PreviousPosition { get; set; }
        public int LivePoints { get; set; }
        public int PreviousPoints { get; set; }
        public int CurrentRacePoints { get; set; }
    }
}
