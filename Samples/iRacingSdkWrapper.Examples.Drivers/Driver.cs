namespace iRacingSdkWrapper.Examples.Drivers
{
    /// <summary>
    /// Represents a driver in the current session.
    /// </summary>
    public class Driver
    {
        public Driver()
        {
        }

        /// <summary>
        /// The identifier (CarIdx) of this driver (unique to this session)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The current position of the driver
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// The name of the driver
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The customer ID (custid) of the driver
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// The car number of this driver
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// A unique identifier for the car class this driver is using
        /// </summary>
        public int ClassId { get; set; }

        /// <summary>
        /// The name of the car of this driver
        /// </summary>
        public string CarPath { get; set; }

        /// <summary>
        /// The relative speed of this class in a multiclass session
        /// </summary>
        public int CarClassRelSpeed { get; set; }

        /// <summary>
        /// Used to determine if a driver is in the pits, off or on track
        /// </summary>
        public TrackSurfaces TrackSurface { get; set; }

        /// <summary>
        /// Whether or not the driver is currently in or approaching the pit stall
        /// </summary>
        public bool IsInPits
        {
            get { return this.TrackSurface == TrackSurfaces.AproachingPits || this.TrackSurface == TrackSurfaces.InPitStall; }
        }

        /// <summary>
        /// The lap this driver is currently in
        /// </summary>
        public int Lap { get; set; }

        /// <summary>
        /// The distance along the current lap of this driver (in percentage)
        /// </summary>
        public float LapDistance { get; set; }

        /// <summary>
        /// The relative distance between you and this driver (in percentage).
        /// </summary>
        public float RelativeLapDistance { get; set; }

        /// <summary>
        /// The fastest lap time of this driver
        /// </summary>
        public float FastestLapTime { get; set; }

        /// <summary>
        /// The last lap time of this driver
        /// </summary>
        public float LastLapTime { get; set; }

        /// <summary>
        /// The iRating of this driver
        /// </summary>
        public int Rating { get; set; }
    }
}
