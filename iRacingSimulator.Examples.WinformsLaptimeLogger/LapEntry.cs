using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSimulator.Examples.WinformsLaptimeLogger
{
    public class LapEntry
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string CarNumber { get; set; }
        public int Lap { get; set; }
        public int Laptime { get; set; }
        public string LaptimeDisplay { get; set; }
    }
}
