using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iRSDKSharp;

namespace iRacingSdkWrapper.Examples.Drivers
{
    public partial class Form1 : Form
    {
        private SdkWrapper wrapper;

        private List<Driver> drivers;
        private bool isUpdatingDrivers;
        private BindingSource binding;

        private int currentSessionNum;

        public Form1()
        {
            InitializeComponent();

            // Create a new instance of the SdkWrapper object
            wrapper = new SdkWrapper();

            // Set some properties
            wrapper.EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread;
            wrapper.TelemetryUpdateFrequency = 10;

            // Listen for various events
            wrapper.Connected += wrapper_Connected;
            wrapper.Disconnected += wrapper_Disconnected;
            wrapper.SessionInfoUpdated += wrapper_SessionInfoUpdated;
            wrapper.TelemetryUpdated += wrapper_TelemetryUpdated;


            // Bind a list of drivers to the grid
            binding = new BindingSource();
            drivers = new List<Driver>();
            binding.DataSource = drivers;
            driversGrid.DataSource = binding;
        }

        #region Connecting, disconnecting, etc

        private void startButton_Click(object sender, EventArgs e)
        {
            // If the wrapper is running, stop it. Otherwise, start it.
            if (wrapper.IsRunning)
            {
                wrapper.Stop();
                startButton.Text = "Start";
            }
            else
            {
                wrapper.Start();
                startButton.Text = "Stop";
            }
            this.StatusChanged();
        }

        private void StatusChanged()
        {
            if (wrapper.IsConnected)
            {
                if (wrapper.IsRunning)
                {
                    statusLabel.Text = "Status: connected!";
                }
                else
                {
                    statusLabel.Text = "Status: disconnected.";
                }
            }
            else
            {
                if (wrapper.IsRunning)
                {
                    statusLabel.Text = "Status: disconnected, waiting for sim...";
                }
                else
                {
                    statusLabel.Text = "Status: disconnected";
                }
            }
        }

        private void wrapper_Connected(object sender, EventArgs e)
        {
            this.StatusChanged();
        }

        private void wrapper_Disconnected(object sender, EventArgs e)
        {
            this.StatusChanged();
        }

        #endregion

        #region Events
        
        private void wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            // Indicate that we are updating the drivers list
            isUpdatingDrivers = true;

            // Parse the Drivers section of the session info into a list of drivers
            this.ParseDrivers(e.SessionInfo);

            // Parse the ResultsPositions section of the session info to get the positions and times of drivers
            this.ParseTimes(e.SessionInfo);

            // Indicate we are finished updating drivers
            isUpdatingDrivers = false;
        }

        private void wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            // Besides the driver details found in the session info, there's also things in the telemetry
            // that are properties of a driver, such as their lap, lap distance, track surface, distance relative
            // to yourself and more.
            // We update the existing list of drivers with the telemetry values here.

            // If we are currently renewing the drivers list it makes little sense to update the existing drivers
            // because they will change anyway
            if (isUpdatingDrivers) return;

            // Store the current session number so we know which session to read 
            // There can be multiple sessions in a server (practice, Q, race, or warmup, race, etc).
            currentSessionNum = e.TelemetryInfo.SessionNum.Value;
            
            this.UpdateDriversTelemetry(e.TelemetryInfo);
        }

        #endregion

        #region Drivers

        // Parse the YAML DriverInfo section that contains information such as driver id, name, license, car number, etc.
        private void ParseDrivers(SessionInfo sessionInfo)
        {
            int id = 0;
            Driver driver;

            var newDrivers = new List<Driver>();

            // Loop through drivers until none are found anymore
            do
            {
                driver = null;

                
                // Construct a yaml query that finds each driver and his info in the Session Info yaml string
                // This query can be re-used for every property for one driver (with the specified id)
                YamlQuery query = sessionInfo["DriverInfo"]["Drivers"]["CarIdx", id];



                // Try to get the UserName of the driver (because its the first value given)
                // If the UserName value is not found (name == null) then we found all drivers and we can stop
                string name = query["UserName"].GetValue();
                if (name != null)
                {
                    // Find this driver in the list
                    // This strange " => " syntax is called a lambda expression and is short for a loop through all drivers
                    // Read as: select the first driver 'd', if any, whose Name is equal to name.
                    driver = drivers.FirstOrDefault(d => d.Name == name);

                    if (driver == null)
                    {
                        // Or create a new Driver if we didn't find him before
                        driver = new Driver();
                        driver.Id = id;
                        driver.Name = name;
                        driver.CustomerId = int.Parse(query["UserID"].GetValue("0")); // default value 0
                        driver.Number = query["CarNumber"].GetValue("").TrimStart('\"').TrimEnd('\"'); // trim the quotes
                        driver.ClassId = int.Parse(query["CarClassID"].GetValue("0"));
                        driver.CarPath = query["CarPath"].GetValue();
                        driver.CarClassRelSpeed = int.Parse(query["CarClassRelSpeed"].GetValue("0"));
                        driver.Rating = int.Parse(query["IRating"].GetValue("0"));
                    }
                    newDrivers.Add(driver);

                    id++;
                }
            } while (driver != null);

            // Replace old list of drivers with new list of drivers and update the grid
            drivers.Clear();
            drivers.AddRange(newDrivers);

            this.UpdateDriversGrid();
        }

        // Parse the YAML SessionInfo section that contains information such as lap times, position, etc.
        private void ParseTimes(SessionInfo sessionInfo)
        {            
            int position = 1;
            Driver driver = null;

            // Loop through positions starting at 1 until no more are found
            do
            {
                driver = null;

                // Construct a yaml query that we can re-use again
                YamlQuery query = sessionInfo["SessionInfo"]["Sessions"]["SessionNum", currentSessionNum]
                    ["ResultsPositions"]["Position", position];


                // Find the car id belonging to the current position
                string idString = query["CarIdx"].GetValue();
                if (idString != null)
                {
                    int id = int.Parse(idString);

                    // Find the corresponding driver from the list
                    // This strange " => " syntax is called a lambda expression and is short for a loop through all drivers
                    // Read as: select the first driver 'd', if any, whose Id is equal to id.
                    driver = drivers.FirstOrDefault(d => d.Id == id);

                    if (driver != null)
                    {
                        driver.Position = position;
                        driver.FastestLapTime = float.Parse(query["FastestTime"].GetValue("0"), CultureInfo.InvariantCulture);
                        driver.LastLapTime = float.Parse(query["LastTime"].GetValue("0"), CultureInfo.InvariantCulture);
                    }

                    position++;
                }

            } while (driver != null);
        }

        private void UpdateDriversTelemetry(TelemetryInfo info)
        {
            // Get your own driver entry
            // This strange " => " syntax is called a lambda expression and is short for a loop through all drivers
            Driver me = drivers.FirstOrDefault(d => d.Id == wrapper.DriverId);

            // Get arrays of the laps, distances, track surfaces of every driver
            var laps = info.CarIdxLap.Value;
            var lapDistances = info.CarIdxLapDistPct.Value;
            var trackSurfaces = info.CarIdxTrackSurface.Value;

            // Loop through the list of current drivers
            foreach (Driver driver in drivers)
            {
                // Set the lap, distance, tracksurface belonging to this driver
                driver.Lap = laps[driver.Id];
                driver.LapDistance = lapDistances[driver.Id];
                driver.TrackSurface = trackSurfaces[driver.Id];

                // If your own driver exists, use it to calculate the relative distance between you and the other driver
                if (me != null)
                {
                    var relative = driver.LapDistance - me.LapDistance;

                    // If driver is more than half the track behind, subtract 100% track length
                    // and vice versa
                    if (relative > 0.5) relative -= 1;
                    else if (relative < -0.5) relative += 1;

                    driver.RelativeLapDistance = relative;
                }
                else
                {
                    driver.RelativeLapDistance = -1;
                }
            }

            this.UpdateDriversGrid();
        }

        private void UpdateDriversGrid()
        {
            binding.ResetBindings(false);
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            wrapper.Stop();
        }
    }
}
