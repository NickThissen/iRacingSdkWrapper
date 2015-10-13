using System;
using System.Windows.Media;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public class DriverCarInfo
    {
        public string CarNumber { get; set; }
        public int CarNumberRaw { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int CarClassId { get; set; }
        public int CarClassRelSpeed { get; set; }
        public Color CarClassColor { get; set; }
        public string CarClassShortName { get; set; }
    }
}
