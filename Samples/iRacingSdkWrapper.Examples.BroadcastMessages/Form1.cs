using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iRacingSdkWrapper.Bitfields;
using iRacingSdkWrapper.Broadcast;
using iRSDKSharp;

namespace iRacingSdkWrapper.Examples.BroadcastMessages
{
    public partial class Form1 : Form
    {
        private readonly SdkWrapper _wrapper;

        public Form1()
        {
            InitializeComponent();

            _wrapper = new SdkWrapper();
            _wrapper.Start();

            LoadDropdowns();
        }

        private void LoadDropdowns()
        {
            // Replay events
            cboReplayEvents.DataSource = Enum.GetValues(typeof (ReplaySearchModeTypes));
        }

        private void btnSetPos_Click(object sender, EventArgs e)
        {
            _wrapper.Replay.SetPosition((int)numSetPos.Value);
        }

        private void btnSetPosEnd_Click(object sender, EventArgs e)
        {
            _wrapper.Replay.SetPositionFromEnd((int)numSetPosEnd.Value);
        }

        private void btnJump_Click(object sender, EventArgs e)
        {
            var replayEvent = (ReplaySearchModeTypes)cboReplayEvents.SelectedValue;
            _wrapper.Replay.Jump(replayEvent);
        }

        private void btnSetSpeed_Click(object sender, EventArgs e)
        {
            _wrapper.Replay.SetPlaybackSpeed((int)numSpeed.Value, chkSlowmo.Checked);
        }
    }
}
