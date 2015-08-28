using System.IO;
using System.Windows.Forms;
using YamlDotNet.Serialization;

namespace iRacingSdkWrapper.Examples.YamlDeserialize
{
    public partial class Form1 : Form
    {
        private SdkWrapper wrapper;

        public Form1()
        {
            InitializeComponent();

            wrapper = new SdkWrapper();
            wrapper.SessionInfoUpdated += wrapper_SessionInfoUpdated;
            wrapper.Start();
        }

        private void wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            // Create a Deserializer
            // Tell it to ignore unmatched properties unless you have mapped every single property
            var deserializer = new Deserializer(ignoreUnmatched: true);
            
            // Read the session info yaml
            using (var reader = new StringReader(e.SessionInfo.Yaml))
            {
                var customSessionInfo = deserializer.Deserialize<CustomSessionInfo>(reader);

                // Write it to a label for example
                label1.Text = "WeekendInfo:TrackName: " + customSessionInfo.WeekendInfo.TrackName
                              + "\n"
                              + "WeekendInfo:WeekendOptions:StartingGrid: " +
                              customSessionInfo.WeekendInfo.WeekendOptions.StartingGrid
                              + "\n"
                              + "DriverInfo:Drivers[0]:UserName: " + customSessionInfo.DriverInfo.Drivers[0].UserName;
            }
        }

    }
}
