using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iRSDKSharp;

namespace iRacingSdkWrapper.Examples.Cameras
{
    public partial class Form1 : Form
    {
        private readonly SdkWrapper wrapper;

        public Form1()
        {
            InitializeComponent();

            wrapper = new SdkWrapper();
            wrapper.TelemetryUpdateFrequency = 5;
            wrapper.SessionInfoUpdated += OnSessionInfoUpdated;
            wrapper.TelemetryUpdated += OnTelemetryUpdated;

            wrapper.Start();
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            var replayFrame = e.TelemetryInfo.ReplayFrameNum.Value;
            var replayEndFrame = wrapper.GetTelemetryValue<int>("ReplayFrameNumEnd").Value;
            var sum = replayFrame + replayEndFrame;

            label1.Text = "ReplayFrameNum: " + replayFrame
                          + "\n\nReplayFrameNumEnd: " + replayEndFrame
                          + "\n\nSum: " + sum
                          + "\n\nSessionTime: " + e.TelemetryInfo.SessionTime.Value
                          + "\n\nReplaySessionTime: " + e.TelemetryInfo.ReplaySessionTime.Value;


            progressBar1.Minimum = 0;
            progressBar1.Maximum = sum;
            progressBar1.Value = replayFrame;
        }

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var time = int.Parse(textBox1.Text);
            var curTime = wrapper.GetTelemetryValue<double>("SessionTime").Value;

            var diff = curTime - time;

            var frames = (int)(diff*60);

            wrapper.Sdk.BroadcastMessage(BroadcastMessageTypes.ReplaySetPlayPosition, (int)ReplayPositionModeTypes.End,
                frames);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var frame = int.Parse(textBox2.Text);
            wrapper.Sdk.BroadcastMessage(BroadcastMessageTypes.ReplaySetPlayPosition, (int)ReplayPositionModeTypes.Current,
                frame);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var frame = int.Parse(textBox3.Text);
            wrapper.Sdk.BroadcastMessage(BroadcastMessageTypes.ReplaySetPlayPosition, (int)ReplayPositionModeTypes.End,
                frame);
        }
    }
}
