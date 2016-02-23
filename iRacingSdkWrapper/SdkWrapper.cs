using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using iRSDKSharp;
using iRacingSdkWrapper.Broadcast;

namespace iRacingSdkWrapper
{
    /// <summary>
    /// Provides a useful wrapper of the iRacing SDK.
    /// </summary>
    public sealed class SdkWrapper
    {
        #region Fields

        internal readonly iRacingSDK sdk;
        private readonly SynchronizationContext context;
        private int waitTime;
        private Mutex readMutex;

        private Thread _looper;
        private bool _hasConnected;

        #endregion

        /// <summary>
        /// Creates a new instance of the SdkWrapper.
        /// </summary>
        public SdkWrapper()
        {
            this.context = SynchronizationContext.Current;
            this.sdk = new iRacingSDK();
            this.EventRaiseType = EventRaiseTypes.CurrentThread;

            readMutex = new Mutex(false);

            this.TelemetryUpdateFrequency = 60;
            this.ConnectSleepTime = 1000;
            _DriverId = -1;

            this.Replay = new ReplayControl(this);
            this.Camera = new CameraControl(this);
            this.PitCommands = new PitCommandControl(this);
            this.Chat = new ChatControl(this);
            this.Textures = new TextureControl(this);
            this.TelemetryRecording = new TelemetryRecordingControl(this);
        }

        #region Properties

        /// <summary>
        /// Gets the underlying iRacingSDK object.
        /// </summary>
        public iRacingSDK Sdk { get { return sdk; } }

        /// <summary>
        /// Gets or sets how events are raised. Choose 'CurrentThread' to raise the events on the thread you created this object on (typically the UI thread), 
        /// or choose 'BackgroundThread' to raise the events on a background thread, in which case you have to delegate any UI code to your UI thread to avoid cross-thread exceptions.
        /// </summary>
        public EventRaiseTypes EventRaiseType { get; set; }

        private bool _IsRunning;
        /// <summary>
        /// Is the main loop running?
        /// </summary>
        public bool IsRunning { get { return _IsRunning; } }

        private bool _IsConnected;
        /// <summary>
        /// Is the SDK connected to iRacing?
        /// </summary>
        public bool IsConnected { get { return _IsConnected; } }

        private double _TelemetryUpdateFrequency;
        /// <summary>
        /// Gets or sets the number of times the telemetry info is updated per second. The default and maximum is 60 times per second.
        /// </summary>
        public double TelemetryUpdateFrequency
        {
            get { return _TelemetryUpdateFrequency; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("TelemetryUpdateFrequency must be at least 1.");
                if (value > 60)
                    throw new ArgumentOutOfRangeException("TelemetryUpdateFrequency cannot be more than 60.");

                _TelemetryUpdateFrequency = value;
                
                waitTime = (int) Math.Floor(1000f/value) - 1;
            }
        }

        /// <summary>
        /// The time in milliseconds between each check if iRacing is running. Use a low value (hundreds of milliseconds) to respond quickly to iRacing startup.
        /// Use a high value (several seconds) to conserve resources if an immediate response to startup is not required.
        /// </summary>
        public int ConnectSleepTime
        {
            get; set;
        }

        private int _DriverId;
        /// <summary>
        /// Gets the Id (CarIdx) of yourself (the driver running this application).
        /// </summary>
        public int DriverId { get { return _DriverId; } }

        #region Broadcast messages

        /// <summary>
        /// Controls the replay playback system.
        /// </summary>
        public ReplayControl Replay { get; private set; }

        /// <summary>
        /// Provides control over the replay camera and where it is focused.
        /// </summary>
        public CameraControl Camera { get; private set; }

        /// <summary>
        /// Provides control over the pit commands.
        /// </summary>
        public PitCommandControl PitCommands { get; private set; }

        /// <summary>
        /// Provides control over the chat window.
        /// </summary>
        public ChatControl Chat { get; private set; }

        /// <summary>
        /// Provides control over reloading of car textures.
        /// </summary>
        public TextureControl Textures { get; private set; }

        /// <summary>
        /// Provides control over the telemetry recording system.
        /// </summary>
        public TelemetryRecordingControl TelemetryRecording { get; private set; }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Connects to iRacing and starts the main loop in a background thread.
        /// </summary>
        public void Start()
        {
            _IsRunning = true;

            if (_looper != null)
            {
                _looper.Abort();
            }

            _looper = new Thread(Loop);
            _looper.Start();
        }

        /// <summary>
        /// Stops the main loop
        /// </summary>
        public void Stop()
        {
            _IsRunning = false;
        }

        /// <summary>
        /// Return raw data object from the live telemetry.
        /// </summary>
        /// <param name="headerName">The name of the telemetry property to obtain.</param>
        public object GetData(string headerName)
        {
            if (!this.IsConnected) return null;

            return sdk.GetData(headerName);
        }

        /// <summary>
        /// Return live telemetry data wrapped in a TelemetryValue object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the desired object.</typeparam>
        /// <param name="name">The name of the desired object.</param>
        public TelemetryValue<T> GetTelemetryValue<T>(string name)
        {
            return new TelemetryValue<T>(sdk, name);
        }

        /// <summary>
        /// Reads new session info and raises the SessionInfoUpdated event, regardless of if the session info has changed.
        /// </summary>
        public void RequestSessionInfoUpdate()
        {
            var sessionInfo = sdk.GetSessionInfo();
            var time = (double) sdk.GetData("SessionTime");
            var sessionArgs = new SessionInfoUpdatedEventArgs(sessionInfo, time);
            this.RaiseEvent(OnSessionInfoUpdated, sessionArgs);
        }
        
        private object TryGetSessionNum()
        {
            try
            {
                var sessionnum = sdk.GetData("SessionNum");
                return sessionnum;
            }
            catch
            {
                return null;
            }
        }

        private void Loop()
        {
            int lastUpdate = -1;

            while (_IsRunning)
            {
                // Check if we can find the sim
                if (sdk.IsConnected())
                {
                    if (!_IsConnected)
                    {
                        // If this is the first time, raise the Connected event
                        this.RaiseEvent(OnConnected, EventArgs.Empty);
                    }

                    _hasConnected = true;
                    _IsConnected = true;

                    readMutex.WaitOne(8);

                    int attempts = 0;
                    const int maxAttempts = 99;

                    object sessionnum = this.TryGetSessionNum();
                    while (sessionnum == null && attempts <= maxAttempts)
                    {
                        attempts++;
                        sessionnum = this.TryGetSessionNum();
                    }
                    if (attempts >= maxAttempts)
                    {
                        Debug.WriteLine("Session num too many attempts");
                        continue;
                    }
                    
                    // Parse out your own driver Id
                    if (this.DriverId == -1)
                    {
                        string sessionInfoDriver = sdk.GetSessionInfo();
                        _DriverId = int.Parse(YamlParser.Parse(sessionInfoDriver, "DriverInfo:DriverCarIdx:"));
                    }

                    // Get the session time (in seconds) of this update
                    var time = (double) sdk.GetData("SessionTime");

                    // Raise the TelemetryUpdated event and pass along the lap info and session time
                    var telArgs = new TelemetryUpdatedEventArgs(new TelemetryInfo(sdk), time);
                    this.RaiseEvent(OnTelemetryUpdated, telArgs);

                    // Is the session info updated?
                    int newUpdate = sdk.Header.SessionInfoUpdate;
                    if (newUpdate != lastUpdate)
                    {
                        lastUpdate = newUpdate;

                        // Get the session info string
                        var sessionInfo = sdk.GetSessionInfo();

                        // Raise the SessionInfoUpdated event and pass along the session info and session time.
                        var sessionArgs = new SessionInfoUpdatedEventArgs(sessionInfo, time);
                        this.RaiseEvent(OnSessionInfoUpdated, sessionArgs);
                    }


                }
                else if (_hasConnected)
                {
                    // We have already been initialized before, so the sim is closing
                    this.RaiseEvent(OnDisconnected, EventArgs.Empty);

                    sdk.Shutdown();
                    _DriverId = -1;
                    lastUpdate = -1;
                    _IsConnected = false;
                    _hasConnected = false;
                }
                else
                {
                    _IsConnected = false;
                    _hasConnected = false;
                    _DriverId = -1;

                    //Try to find the sim
                    sdk.Startup();
                }

                // Sleep for a short amount of time until the next update is available
                if (_IsConnected)
                {
                    if (waitTime <= 0 || waitTime > 1000) waitTime = 15;
                    Thread.Sleep(waitTime);
                }
                else
                {
                    // Not connected yet, no need to check every 16 ms, let's try again in some time
                    Thread.Sleep(ConnectSleepTime);
                }
            }
        
            sdk.Shutdown();
            _DriverId = -1;
            _IsConnected = false;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the sim outputs telemetry information (60 times per second).
        /// </summary>
        public event EventHandler<TelemetryUpdatedEventArgs> TelemetryUpdated;

        /// <summary>
        /// Event raised when the sim refreshes the session info (few times per minute).
        /// </summary>
        public event EventHandler<SessionInfoUpdatedEventArgs> SessionInfoUpdated;

        /// <summary>
        /// Event raised when the SDK detects the sim for the first time.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Event raised when the SDK no longer detects the sim (sim closed).
        /// </summary>
        public event EventHandler Disconnected;

        private void RaiseEvent<T>(Action<T> del, T e)
            where T : EventArgs
        {
            var callback = new SendOrPostCallback(obj => del(obj as T));

            if (context != null && this.EventRaiseType == EventRaiseTypes.CurrentThread)
            {
                // Post the event method on the thread context, this raises the event on the thread on which the SdkWrapper object was created
                context.Post(callback, e);
            }
            else
            {
                // Simply invoke the method, this raises the event on the background thread that the SdkWrapper created
                // Care must be taken by the user to avoid cross-thread operations
                callback.Invoke(e);
            }
        }

        private void OnSessionInfoUpdated(SessionInfoUpdatedEventArgs e)
        {
            var handler = this.SessionInfoUpdated;
            if (handler != null) handler(this, e);
        }

        private void OnTelemetryUpdated(TelemetryUpdatedEventArgs e)
        {
            var handler = this.TelemetryUpdated;
            if (handler != null) handler(this, e);
        }

        private void OnConnected(EventArgs e)
        {
            var handler = this.Connected;
            if (handler != null) handler(this, e);
        }

        private void OnDisconnected(EventArgs e)
        {
            var handler = this.Disconnected;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Enums

        /// <summary>
        /// The way in which events of the SDK wrapper are raised.
        /// </summary>
        public enum EventRaiseTypes
        {
            /// <summary>
            /// Events are raised on the current thread (the thread on which the SdkWrapper object was created).
            /// </summary>
            CurrentThread,

            /// <summary>
            /// Events are raised on a separate background thread (synchronization / invokation required to update UI).
            /// </summary>
            BackgroundThread
        }

        #endregion

        #region Nested classes

        public class SdkUpdateEventArgs : EventArgs
        {
            public SdkUpdateEventArgs(double time)
            {
                _UpdateTime = time;
            }

            private readonly double _UpdateTime;
            /// <summary>
            /// Gets the time (in seconds) when this update occured.
            /// </summary>
            public double UpdateTime { get { return _UpdateTime; } }
        }

        public class SessionInfoUpdatedEventArgs : SdkUpdateEventArgs
        {
            public SessionInfoUpdatedEventArgs(string sessionInfo, double time) : base(time)
            {
                _SessionInfo = new SessionInfo(sessionInfo, time);
            }

            private readonly SessionInfo _SessionInfo;
            /// <summary>
            /// Gets the session info.
            /// </summary>
            public SessionInfo SessionInfo { get { return _SessionInfo; } }
        }

        public class TelemetryUpdatedEventArgs : SdkUpdateEventArgs
        {
            public TelemetryUpdatedEventArgs(TelemetryInfo info, double time) : base(time)
            {
                _TelemetryInfo = info;
            }

            private readonly TelemetryInfo _TelemetryInfo;
            /// <summary>
            /// Gets the telemetry info object.
            /// </summary>
            public TelemetryInfo TelemetryInfo { get { return _TelemetryInfo; } }
        }

        #endregion
    }
}
