using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSimulator.Drivers;

namespace iRacingSimulator.Examples.IncidentLog
{
    public class Incident
    {
        public Incident(Driver driver, string sessionType, double sessionTime, int delta)
        {
            this.SessionType = sessionType;
            this.SessionTime = sessionTime;
            this.TimeDisplay = TimeSpan.FromSeconds(sessionTime).ToString(@"hh\:mm\:ss");
            this.Driver = driver;
            this.DriverDisplay = driver.LongDisplay;
            this.IncDelta = delta;
        }

        public string SessionType { get; set; }

        public double SessionTime { get; set; }
        public string TimeDisplay { get; set; }

        public Driver Driver { get; set; }
        public string DriverDisplay { get; set; }

        public int IncDelta { get; set; }
    }
}
