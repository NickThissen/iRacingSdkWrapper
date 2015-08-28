using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iRSDKSharp;
using iRacingSdkWrapper;
using iRacingSdkWrapper.Bitfields;

namespace iRacingSdkWrapperExample
{
    public partial class Form1 : Form
    {
        private SdkWrapper wrapper;

        public Form1()
        {
            InitializeComponent();

            // Create a new instance of the SdkWrapper object
            wrapper = new SdkWrapper();

            // Tell it to raise events on the current thread (don't worry if you don't know what a thread is)
            wrapper.EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread;

            // Only update telemetry 10 times per second
            wrapper.TelemetryUpdateFrequency = 10;

            // Attach some useful events so you can respond when they get raised
            wrapper.Connected += wrapper_Connected;
            wrapper.Disconnected += wrapper_Disconnected;
            wrapper.SessionInfoUpdated += wrapper_SessionInfoUpdated;
            wrapper.TelemetryUpdated += wrapper_TelemetryUpdated;
        }

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

        // Event handler called when the sdk wrapper connects (eg, you start it, or the sim is started)
        private void wrapper_Connected(object sender, EventArgs e)
        {
            this.StatusChanged();
        }

        // Event handler called when the sdk wrapper disconnects (eg, the sim closes)
        private void wrapper_Disconnected(object sender, EventArgs e)
        {
            this.StatusChanged();
        }

        // Event handler called when the session info is updated
        // This typically happens when a car crosses the finish line for example
        private void wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            sessionInfoLabel.Text = string.Format("Session info (last update time: {0})", e.UpdateTime);

            // Let's just dump the session info:
            sessionInfoTextBox.Text = e.SessionInfo.Yaml;

            // Read some values using a YAML query
            string trackName = e.SessionInfo.GetValue("WeekendInfo:TrackName:");
            int sessionId = int.Parse(e.SessionInfo.GetValue("WeekendInfo:SessionId:"));
            string windSpeed = e.SessionInfo.GetValue("WeekendInfo:WeekendOptions:WindSpeed:");

            // Read some values with the easier notation (no need to write the YAML query)
            var trackLength = e.SessionInfo["WeekendInfo"]["TrackLength"].GetValue();
            var driver1Name = e.SessionInfo["DriverInfo"]["Drivers"]["CarIdx", 1]["UserName"].GetValue();

            // Attempt to read the username of a random driver safely
            var random = new Random();
            var id = random.Next(0, 60);

            string driverName;
            if (e.SessionInfo["DriverInfo"]["Drivers"]["CarIdx", id]["UserName"].TryGetValue(out driverName))
            {
                // Success
                MessageBox.Show(driverName);
            }
            else
            {
                // not found
                MessageBox.Show("Error retrieving name of driver " + id);
            }
        }

        // Event handler called when the telemetry is updated
        // This happens (max) 60 times per second
        private void wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            telemetryLabel.Text = string.Format("Telemetry (last updated {0})", DateTime.Now.TimeOfDay.ToString());

            // Let's just write some random values:
            StringBuilder sb = new StringBuilder();

            this.TelemetryExample(sb, e);
            this.ArrayExample(sb, e);
            this.BitfieldsExample(sb, e);

            // Get a (fictional) value that is not covered in the TelemetryInfo properties
            var fictionalObject = wrapper.GetTelemetryValue<int>("VariableName");
            var fictionalValue = fictionalObject.Value;

            telemetryTextBox.Text = sb.ToString();
        }

        #region Simple telemetry examples

        // Example method that adds speed and roll values to the string builder
        private void TelemetryExample(StringBuilder sb, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            // Important distinction: TelemetryInfo.Speed (for example) returns an object of type TelemetryValue. 
            // This object contains more than just the value; also the unit, name and a description.
            // To get just the value, you use the Value property.
            // This goes for every property of the TelemetryInfo class.

            sb.AppendLine("Speed: " + e.TelemetryInfo.Speed.Value); // Without unit
            sb.AppendLine("Speed: " + e.TelemetryInfo.Speed.ToString()); // with unit
            sb.AppendLine("Roll: " + e.TelemetryInfo.Roll.ToString());
        }

        // Example method that adds some data such as your lap distance and track surface to the string builder
        private void ArrayExample(StringBuilder sb, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            // Get your own CarIdx:
            int myId = wrapper.DriverId;

            // Get the arrays you want
            float[] lapDistances = e.TelemetryInfo.CarIdxLapDistPct.Value;
            TrackSurfaces[] surfaces = e.TelemetryInfo.CarIdxTrackSurface.Value;

            // Your car data is at your id index;
            float myLapDistance = lapDistances[myId];
            TrackSurfaces mySurface = surfaces[myId];

            sb.AppendLine("My lap distance: " + myLapDistance);
            sb.AppendLine("My track surface: " + mySurface.ToString());
        }
    
        // Example method that adds some caution flags to the string builder if they are displayed in the sim
        private void BitfieldsExample(StringBuilder sb, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            // The value of SessionFlags returns a SessionFlag object which contains information about all currently active flags
            // Use the Contains method to check if it contains a specific flag.

            // EngineWarnings and CameraStates behave similarly.

            SessionFlag flags = e.TelemetryInfo.SessionFlags.Value;

            if (flags.Contains(SessionFlags.Black))
            {
                sb.AppendLine("Black flag!");
            }
            if (flags.Contains(SessionFlags.Disqualify))
            {
                sb.AppendLine("DQ");
            }
            if (flags.Contains(SessionFlags.Repair))
            {
                sb.AppendLine("Repair");
            }
            if (flags.Contains(SessionFlags.Checkered))
            {
                sb.AppendLine("Checkered");
            }
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            wrapper.Stop();
        }


    }
}
