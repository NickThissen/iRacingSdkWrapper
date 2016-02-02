using System;
using System.Diagnostics;
using System.Linq;
using iRacingSdkWrapper;

namespace iRacingSimulator.Drivers
{
    [Serializable]
    public partial class Driver : NotifyPropertyChanged
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
        public bool IsCurrentDriver
        {
            get { return _isCurrentDriver; }
            set
            {
                if (value == _isCurrentDriver) return;
                _isCurrentDriver = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }

        public int CustId
        {
            get { return _custId; }
            set
            {
                if (value == _custId) return;
                _custId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LongDisplay));
            }
        }

        public string ShortName
        {
            get { return _shortName; }
            set
            {
                if (value == _shortName) return;
                _shortName = value;
                OnPropertyChanged();
            }
        }

        public string CarNumber { get { return this.Car.CarNumber; } }

        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int IRating
        {
            get { return _rating; }
            set
            {
                if (value == _rating) return;
                _rating = value;
                OnPropertyChanged();
            }
        }

        public License License
        {
            get { return _license; }
            set
            {
                if (Equals(value, _license)) return;
                _license = value;
                OnPropertyChanged();
            }
        }

        public bool IsSpectator
        {
            get { return _isSpectator; }
            set
            {
                if (value == _isSpectator) return;
                _isSpectator = value;
                OnPropertyChanged();
            }
        }

        public bool IsPacecar { get; set; }

        public string HelmetDesign
        {
            get { return _helmetDesign; }
            set
            {
                if (value == _helmetDesign) return;
                _helmetDesign = value;
                OnPropertyChanged();
            }
        }

        public string CarDesign
        {
            get { return _carDesign; }
            set
            {
                if (value == _carDesign) return;
                _carDesign = value;
                OnPropertyChanged();
            }
        }

        public string SuitDesign
        {
            get { return _suitDesign; }
            set
            {
                if (value == _suitDesign) return;
                _suitDesign = value;
                OnPropertyChanged();
            }
        }

        public string CarNumberDesign
        {
            get { return _carNumberDesign; }
            set
            {
                if (value == _carNumberDesign) return;
                _carNumberDesign = value;
                OnPropertyChanged();
            }
        }

        public string CarSponsor1
        {
            get { return _carSponsor1; }
            set
            {
                if (value == _carSponsor1) return;
                _carSponsor1 = value;
                OnPropertyChanged();
            }
        }

        public string CarSponsor2
        {
            get { return _carSponsor2; }
            set
            {
                if (value == _carSponsor2) return;
                _carSponsor2 = value;
                OnPropertyChanged();
            }
        }

        public string ClubName
        {
            get { return _clubName; }
            set
            {
                if (value == _clubName) return;
                _clubName = value;
                OnPropertyChanged();
            }
        }

        public string DivisionName
        {
            get { return _divisionName; }
            set
            {
                if (value == _divisionName) return;
                _divisionName = value;
                OnPropertyChanged();
            }
        }

        public DriverCarInfo Car { get; private set; }
        public DriverPitInfo PitInfo { get; private set; }
        public DriverResults Results { get; private set; }
        public DriverSessionResults CurrentResults { get; private set; }
        public DriverQualyResults QualyResults { get; private set; }
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
            this.CarDesign = query["CarDesignStr"].GetValue();
            this.SuitDesign = query["SuitDesignStr"].GetValue();
            this.CarNumberDesign = query["CarNumberDesignStr"].GetValue();
            this.CarSponsor1 = query["CarSponsor_1"].GetValue();
            this.CarSponsor2 = query["CarSponsor_2"].GetValue();
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

        private double _prevPos;
        private bool _isCurrentDriver;
        private string _name;
        private int _custId;
        private string _shortName;
        private int _rating;
        private License _license;
        private bool _isSpectator;
        private string _helmetDesign;
        private string _carDesign;
        private string _suitDesign;
        private string _carNumberDesign;
        private string _carSponsor1;
        private string _carSponsor2;
        private string _clubName;
        private string _divisionName;

        public void UpdateSectorTimes(Track track, TelemetryInfo telemetry)
        {
            if (track == null) return;
            if (track.Sectors.Count == 0) return;

            var results = this.CurrentResults;
            if (results != null)
            {
                var sectorcount = track.Sectors.Count;

                // Set arrays
                if (results.SectorTimes == null || results.SectorTimes.Length == 0)
                {
                    results.SectorTimes = track.Sectors.Select(s => s.Copy()).ToArray();
                }

                var p0 = _prevPos;
                var p1 = telemetry.CarIdxLapDistPct.Value[this.Id];
                var dp = p1 - p0;

                if (p1 < -0.5)
                {
                    // Not in world?
                    return;
                }

                var t = telemetry.SessionTime.Value;
                
                // Check lap crossing
                if (p0 - p1 > 0.5) // more than 50% jump in track distance == lap crossing occurred from 0.99xx -> 0.00x
                {
                    this.Live.CurrentSector = 0;
                    this.Live.CurrentFakeSector = 0;
                    p0 -= 1;
                }
                    
                // Check all real sectors
                foreach (var s in results.SectorTimes)
                {
                    if (p1 > s.StartPercentage && p0 <= s.StartPercentage)
                    {
                        // Crossed into new sector
                        var crossTime = (float)(t - (p1 - s.StartPercentage) * dp);

                        // Finish previous
                        var prevNum = s.Number <= 0 ? sectorcount - 1 : s.Number - 1;
                        var sector = results.SectorTimes[prevNum];
                        if (sector != null && sector.EnterSessionTime > 0)
                        {
                            sector.SectorTime = new Laptime((float)(crossTime - sector.EnterSessionTime));
                        }

                        // Begin next sector
                        s.EnterSessionTime = crossTime;

                        this.Live.CurrentSector = s.Number;

                        break;
                    }
                }

                // Check 'fake' sectors (divide track into thirds)
                sectorcount = 3;
                foreach (var s in results.FakeSectorTimes)
                {
                    if (p1 > s.StartPercentage && p0 <= s.StartPercentage)
                    {
                        // Crossed into new sector
                        var crossTime = (float)(t - (p1 - s.StartPercentage) * dp);

                        // Finish previous
                        var prevNum = s.Number <= 0 ? sectorcount - 1 : s.Number - 1;
                        var sector = results.FakeSectorTimes[prevNum];
                        if (sector != null && sector.EnterSessionTime > 0)
                        {
                            sector.SectorTime = new Laptime((float)(crossTime - sector.EnterSessionTime));
                        }

                        // Begin next sector
                        s.EnterSessionTime = crossTime;

                        this.Live.CurrentFakeSector = s.Number;

                        break;
                    }
                }
                
                _prevPos = p1;
            }
        }
    }
}
