using iRacingSdkWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using iRacingSimulator.Examples.WPF_MVVM_DriverGrid.Annotations;

namespace iRacingSimulator.Examples.WPF_MVVM_DriverGrid.ViewModels
{
    public abstract class SdkViewModel : INotifyPropertyChanged
    {
        public abstract void OnSessionInfoUpdated(SessionInfo info, double updateTime);
        public abstract void OnTelemetryUpdated(TelemetryInfo info, double updateTime);
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
