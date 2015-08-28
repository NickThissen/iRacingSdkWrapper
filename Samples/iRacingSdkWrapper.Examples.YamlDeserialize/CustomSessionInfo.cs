using System.Collections.Generic;

namespace iRacingSdkWrapper.Examples.YamlDeserialize
{
    public class CustomSessionInfo
    {
        public WeekendInfo WeekendInfo { get; set; }
        public DriverInfo DriverInfo { get; set; }
    }

    public class WeekendInfo
    {
        public string TrackName { get; set; }
        public int TrackID { get; set; }

        public WeekendOptions WeekendOptions { get; set; }
    }

    public class WeekendOptions
    {
        public int NumStarters { get; set; }
        public string StartingGrid { get; set; }
    }

    public class DriverInfo
    {
        public int DriverCarIdx { get; set; }
        public List<Driver> Drivers { get; set; } 
    }

    public class Driver
    {
        public int CarIdx { get; set; }
        public string UserName { get; set; }
    }
}
