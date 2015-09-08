using iRacingSdkWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSimulator.Examples.WPF_MVVM_DriverGrid.ViewModels
{
    public abstract class SdkViewModel
    {
        public abstract void OnSessionInfoUpdated(SessionInfo info, double updateTime);
        public abstract void OnTelemetryUpdated(TelemetryInfo info, double updateTime);
    }
}
