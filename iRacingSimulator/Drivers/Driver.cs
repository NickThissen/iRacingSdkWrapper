using System;
using iRacingSdkWrapper;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public partial class Driver
    {
        private const string PACECAR_NAME = "safety pcfr500s";

        public Driver()
        {
            this.Car = new DriverCarInfo();
            this.PitInfo = new DriverPitInfo(this);
            this.Results = new DriverResults(this);
            this.QualyResults = new DriverQualyResults(this);
            this.Live = new DriverLiveInfo(this);
            this.Championship = new DriverChampInfo(this);
            this.Private = new DriverPrivateInfo(this);
        }

        /// <summary>
        /// If true, this is your driver on track.
        /// </summary>
        public bool IsCurrentDriver { get; set; }

        public int Id { get; set; }
        public int CustId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string CarNumber { get { return this.Car.CarNumber; } }

        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int IRating { get; set; }
        public License License { get; set; }

        public bool IsSpectator { get; set; }
        public bool IsPacecar { get; set; }

        public string HelmetDesign { get; set; }

        public string ClubName { get; set; }
        public string DivisionName { get; set; }

        public DriverCarInfo Car { get; set; }
        public DriverPitInfo PitInfo { get; set; }
        public DriverResults Results { get; private set; }
        public DriverSessionResults CurrentResults { get; set; }
        public DriverQualyResults QualyResults { get; set; }
        public DriverLiveInfo Live { get; private set; }
        public DriverChampInfo Championship { get; private set; }
        public DriverPrivateInfo Private { get; private set; }

        public string LongDisplay
        {
            get { return string.Format("#{0} {1}{2}",
                this.Car.CarNumber,
                this.Name,
                this.TeamId > 0 ? " (" + this.TeamName + ")" : ""); }
        }

        public void ParseDynamicSessionInfo(SessionInfo info)
        {
            // Parse only session info that could have changed (driver dependent)
            var query = info["DriverInfo"]["Drivers"]["CarIdx", this.Id];

            this.Name = query["UserName"].GetValue("");
            this.CustId = Parser.ParseInt(query["UserID"].GetValue("0"));
            this.ShortName = query["AbbrevName"].GetValue();

            this.IRating = Parser.ParseInt(query["IRating"].GetValue());
            var licenseLevel = Parser.ParseInt(query["LicLevel"].GetValue());
            var licenseSublevel = Parser.ParseInt(query["LicSubLevel"].GetValue());
            var licenseColor = Parser.ParseColor(query["LicColor"].GetValue());
            this.License = new License(licenseLevel, licenseSublevel, licenseColor);

            this.IsSpectator = Parser.ParseInt(query["IsSpectator"].GetValue()) == 1;

            this.HelmetDesign = query["HelmetDesignStr"].GetValue();
            this.ClubName = query["ClubName"].GetValue();
            this.DivisionName = query["DivisionName"].GetValue();
        }

        public void ParseStaticSessionInfo(SessionInfo info)
        {
            // Parse only static session info that never changes (car dependent)
            var query = info["DriverInfo"]["Drivers"]["CarIdx", this.Id];
            
            this.TeamId = Parser.ParseInt(query["TeamID"].GetValue());
            this.TeamName = query["TeamName"].GetValue();

            this.Car.CarId = Parser.ParseInt(query["CarID"].GetValue());
            this.Car.CarNumber = query["CarNumber"].GetValue();
            this.Car.CarNumberRaw = Parser.ParseInt(query["CarNumberRaw"].GetValue());
            this.Car.CarName = query["CarPath"].GetValue();
            this.Car.CarClassId = Parser.ParseInt(query["CarClassID"].GetValue());
            this.Car.CarClassRelSpeed = Parser.ParseInt(query["CarClassRelSpeed"].GetValue());
            this.Car.CarClassColor = Parser.ParseColor(query["CarClassColor"].GetValue());

            this.IsPacecar = this.CustId == -1 || this.Car.CarName == PACECAR_NAME;
        }

        public static Driver FromSessionInfo(SessionInfo info, int carIdx)
        {
            var query = info["DriverInfo"]["Drivers"]["CarIdx", carIdx];

            string name;
            if (!query["UserName"].TryGetValue(out name))
            {
                // Driver not found
                return null;
            }

            var driver = new Driver();
            driver.Id = carIdx;
            driver.ParseStaticSessionInfo(info);
            driver.ParseDynamicSessionInfo(info);

            return driver;
        }

        internal void UpdateResultsInfo(int sessionNumber, YamlQuery query, int position)
        {
            this.Results.SetResults(sessionNumber, query, position);
            this.CurrentResults = this.Results.Current;
        }

        internal void UpdateQualyResultsInfo(YamlQuery query, int position)
        {
            this.QualyResults.ParseYaml(query, position);
        }

        internal void UpdateLiveInfo(TelemetryInfo e)
        {
            this.Live.ParseTelemetry(e);
        }

        internal void UpdatePrivateInfo(TelemetryInfo e)
        {
            this.Private.ParseTelemetry(e);
        }
    }
}
