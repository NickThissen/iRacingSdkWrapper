using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;

namespace iRacingSdkWrapper.Broadcast
{
    /// <summary>
    /// Controls the replay playback system.
    /// </summary>
    public class ReplayControl : BroadcastBase
    {
        public ReplayControl(SdkWrapper wrapper) : base(wrapper)
        {
        }

        /// <summary>
        /// Set the replay tape position in number of frames since the session start.
        /// </summary>
        /// <param name="frameFromBeginning"></param>
        public void SetPosition(int frameFromBeginning)
        {
            Broadcast(BroadcastMessageTypes.ReplaySetPlayPosition,
                (int)ReplayPositionModeTypes.Begin,
                frameFromBeginning);
        }

        /// <summary>
        /// Set the replay tape position in number of frames from the session end.
        /// </summary>
        /// <param name="frameFromEnd"></param>
        public void SetPositionFromEnd(int frameFromEnd)
        {
            Broadcast(BroadcastMessageTypes.ReplaySetPlayPosition,
                (int)ReplayPositionModeTypes.End,
                frameFromEnd);
        }

        /// <summary>
        /// Jump to live.
        /// </summary>
        public void JumpToLive()
        {
            Jump(ReplaySearchModeTypes.ToEnd);
        }

        /// <summary>
        /// Jump to the start.
        /// </summary>
        public void JumpToStart()
        {
            Jump(ReplaySearchModeTypes.ToStart);
        }

        /// <summary>
        /// Jump to a specific event.
        /// </summary>
        /// <param name="replayEvent">The event to jump to.</param>
        public void Jump(ReplaySearchModeTypes replayEvent)
        {
            Broadcast(BroadcastMessageTypes.ReplaySearch,
                (int)replayEvent, 0);
        }

        /// <summary>
        /// Pause the replay.
        /// </summary>
        public void Pause()
        {
            SetPlaybackSpeed(0, false);
        }

        /// <summary>
        /// Set the playback speed of the replay.
        /// </summary>
        /// <param name="speed">The playback speed between -16 (reverse) and 16, with 1 realtime speed and 0 being paused.</param>
        public void SetPlaybackSpeed(int speed)
        {
            SetPlaybackSpeed(speed, false);
        }

        /// <summary>
        /// Set the playback to slow motion and set the speed of the replay.
        /// </summary>
        /// <param name="slowmoSpeed">The playback speed between -16 (reverse) and 16, with 1 realtime speed and 0 being paused.</param>
        public void SetSlowmotionPlaybackSpeed(int slowmoSpeed)
        {
            SetPlaybackSpeed(slowmoSpeed, true);
        }

        public void SetPlaybackSpeed(int speed, bool slowmo)
        {
            if (speed < -16 || speed > 16)
                throw new ArgumentOutOfRangeException("speed", "Replay speed must be between -16 and 16.");
            
            Broadcast(BroadcastMessageTypes.ReplaySetPlaySpeed,
                speed, slowmo ? 1 : 0, 0);
        }
    }
}
