using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iRacingSdkWrapper;

namespace iRacingSimulator.Examples.WinformsLaptimeLogger
{
    public partial class Form1 : Form
    {
        // List of lap entries to show in the grid
        private List<LapEntry> laps;

        // BindingSource binds the list of laps to the grid
        private BindingSource bindingSource;

        // A dictionary (key = driver CarIdx) of the lap number of each driver,
        // so we can check if they have completed a new lap
        private Dictionary<int, int> previousLaps; 
        
        public Form1()
        {
            InitializeComponent();

            // Create the list of laps
            laps = new List<LapEntry>();

            // Create the previous laps dictionary
            previousLaps = new Dictionary<int, int>();

            // Create the binding source
            bindingSource = new BindingSource();

            // Bind the list to the grid
            bindingSource.DataSource = laps;
            grid.DataSource = bindingSource;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Listen for relevant events
            Sim.Instance.Connected += OnSimConnected;
            Sim.Instance.Disconnected += OnSimDisconnected;
            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            
            // Start looking for iRacing. The update frequency is only 1 Hz because we don't really need any live telemetry.
            Sim.Instance.Start(1);
        }

        private void OnSimConnected(object sender, EventArgs eventArgs)
        {
            lblStatus.Text = "Connected!";
        }

        private void OnSimDisconnected(object sender, EventArgs eventArgs)
        {
            lblStatus.Text = "Waiting for iRacing...";
        }

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            // The session info has updated
            bool shouldUpdateGrid = false;

            // Let's check every driver for a new lap
            foreach (var driver in Sim.Instance.Drivers)
            {
                // Ignore if there are no results yet
                if (driver.CurrentResults == null) continue;

                // Get the number of laps this driver has completed
                var currentLap = driver.CurrentResults.LapsComplete;

                // Check what lap this driver was on the last update
                int? previousLap = null;
                if (previousLaps.ContainsKey(driver.Id))
                {
                    // We already stored their previous lap
                    previousLap = previousLaps[driver.Id];
                    
                    // Then update the lap number to the current lap
                    previousLaps[driver.Id] = currentLap;
                }
                else
                {
                    // We didn't store their lap yet, so add it
                    previousLaps.Add(driver.Id, currentLap);
                }

                // Check if their lap number has changed
                if (previousLap == null || previousLap.Value < currentLap)
                {
                    // Lap has changed, grab their laptime and add to the list of lap entries
                    var lastTime = driver.CurrentResults.LastTime;

                    var entry = new LapEntry();
                    entry.CustomerId = driver.CustId;
                    entry.Name = driver.Name;
                    entry.CarNumber = driver.CarNumber;

                    entry.Lap = lastTime.LapNumber;
                    entry.Laptime = lastTime.Value;
                    entry.LaptimeDisplay = lastTime.Display;

                    laps.Add(entry);

                    // After we checked every driver we need to update the grid to reflect the changes
                    shouldUpdateGrid = true;
                }
            }

            if (shouldUpdateGrid)
            {
                bindingSource.ResetBindings(false);
            }
        }
    }
}
