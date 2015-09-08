using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;

namespace iRacingSdkWrapper.Broadcast
{
    /// <summary>
    /// Controls the replay system.
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
    }
}
