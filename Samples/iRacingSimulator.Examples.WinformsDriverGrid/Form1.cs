using iRacingSimulator.Drivers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iRacingSimulator.Events;

namespace iRacingSimulator.Examples.WinformsDriverGrid
{
    public partial class Form1 : Form
    {
        private List<Driver> _drivers;
        private BindingSource _source;

        public Form1()
        {
            InitializeComponent();

            // Initialize the grid
            this.SetupGrid();

            // Initialize the sim communication
            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            Sim.Instance.TelemetryUpdated += OnTelemetryUpdated;
            Sim.Instance.RaceEvent += OnRaceEvent;
            Sim.Instance.Start();
        }

        private void SetupGrid()
        {
            // Create a new list to hold the drivers
            _drivers = new List<Driver>();

            // Create a new binding source and bind the list of drivers
            _source = new BindingSource();
            _source.DataSource = _drivers;

            // We will set our own columns
            dataGridView1.AutoGenerateColumns = false;

            // Bind it to the grid
            dataGridView1.DataSource = _source;

            // Setup the columns
            // Note: winforms datagrid cannot simply show nested properties (such as Driver.Live.Position) hence I'm omitting those.
            // Look at WPF example for a better grid
            var col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Custid";
            col.HeaderText = "Customer ID";
            col.Name = "colCustid";
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Name";
            col.HeaderText = "Name";
            col.Name = "colName";
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "CarNumber";
            col.HeaderText = "Car #";
            col.Name = "colCarnumber";
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "IRating";
            col.HeaderText = "iRating";
            col.Name = "colIRating";
            dataGridView1.Columns.Add(col);
        }

        private void RefreshGrid()
        {
            // Refresh the grid by resetting the binding source
            // (There are probably better ways to do this, this will lose selection and scroll position etc)
            _source.ResetBindings(false);
        }

        private void OnSessionInfoUpdated(object sender, iRacingSdkWrapper.SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            // Update the list of drivers: simply clear the old list and re-fill it with the drivers from iRacingSimulator
            _drivers.Clear();
            _drivers.AddRange(Sim.Instance.Drivers);

            // Update the grid
            this.RefreshGrid();
        }

        private void OnTelemetryUpdated(object sender, iRacingSdkWrapper.SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            // No need to do anything here in this example, but you can access all telemetry:
            double sessionTime = e.TelemetryInfo.SessionTime.Value;
        }

        private void OnRaceEvent(object sender, Sim.RaceEventArgs e)
        {
            if (e.Event.Type == RaceEvent.EventTypes.DriverSwap)
            {
                var swapEvent = (DriverSwapRaceEvent) e.Event;
                MessageBox.Show(string.Format("Driver {0} replaced driver {1}.", swapEvent.PreviousDriverName, swapEvent.CurrentDriverName));
            }
        }

    }
}
