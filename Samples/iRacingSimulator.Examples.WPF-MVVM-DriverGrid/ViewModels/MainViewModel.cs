using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;
using System.ComponentModel;
using System.Collections.ObjectModel;
using iRacingSimulator.Drivers;
using System.Windows.Data;

namespace iRacingSimulator.Examples.WPF_MVVM_DriverGrid.ViewModels
{
    public class MainViewModel : SdkViewModel
    {
        private readonly ObservableCollection<Driver> _drivers;
        public ObservableCollection<Driver> Drivers { get { return _drivers; } }

        private readonly ICollectionView _driversView;
        public ICollectionView DriversView { get { return _driversView; } }

        public MainViewModel()
        {
            _drivers = new ObservableCollection<Driver>();
            _driversView = CollectionViewSource.GetDefaultView(_drivers);
        }

        public override void OnSessionInfoUpdated(SessionInfo info, double updateTime)
        {
            _drivers.Clear();

            // Perhaps should add some locking to prevent entering this loop twice when session info updates very quickly
            foreach (var driver in Sim.Instance.Drivers)
            {
                _drivers.Add(driver);
            }
        }

        private double _lastUpdate;

        public override void OnTelemetryUpdated(TelemetryInfo info, double updateTime)
        {
            if (updateTime - _lastUpdate > 1)
            {
                _driversView.Refresh();
                _lastUpdate = updateTime;
            }
        }
    }
}
