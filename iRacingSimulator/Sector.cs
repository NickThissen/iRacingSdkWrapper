using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSimulator
{
    public class Sector
    {
        public int Number { get; set; }
        public float StartPercentage { get; set; }

        public double EnterSessionTime { get; set; }
        public Laptime SectorTime { get; set; }
    }
}
