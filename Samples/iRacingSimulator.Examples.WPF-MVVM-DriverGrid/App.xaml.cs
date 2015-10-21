using iRacingSimulator.Examples.WPF_MVVM_DriverGrid.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iRacingSimulator.Examples.WPF_MVVM_DriverGrid
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private List<SdkViewModel> _sdkViewModels;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _sdkViewModels = new List<SdkViewModel>();

            // Connect viewmodel and view
            var model = new MainViewModel();
            var view = new MainWindow();
            view.DataContext = model;

            // Register viewmodel
            this.RegisterSdkViewModel(model);

            // Setup sim communication
            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            Sim.Instance.TelemetryUpdated += OnTelemetryUpdated;
            Sim.Instance.Start();

            view.Show();
        }

        private void OnSessionInfoUpdated(object sender, iRacingSdkWrapper.SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            foreach (var model in _sdkViewModels)
            {
                model.OnSessionInfoUpdated(e.SessionInfo, e.UpdateTime);
            }
        }

        private void OnTelemetryUpdated(object sender, iRacingSdkWrapper.SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            foreach (var model in _sdkViewModels)
            {
                model.OnTelemetryUpdated(e.TelemetryInfo, e.UpdateTime);
            }
        }

        private void RegisterSdkViewModel(SdkViewModel model)
        {
            _sdkViewModels.Add(model);
        }

        private void UnregisterSdkViewModel(SdkViewModel model)
        {
            _sdkViewModels.Remove(model);
        }
    }
}
