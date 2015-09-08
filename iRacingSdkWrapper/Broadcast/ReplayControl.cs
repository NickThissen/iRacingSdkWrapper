using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iRacingSdkWrapper.Broadcast
{
    public class ReplayControl : BroadcastBase
    {
        public ReplayControl(SdkWrapper wrapper) : base(wrapper)
        {
        }

        public void SetPosition(int frameFromBeginning)
        {
            Broadcast(iRSDKSharp.BroadcastMessageTypes.ReplaySetPlayPosition,
                (int)iRSDKSharp.ReplayPositionModeTypes.Begin,
                frameFromBeginning);
        }

        public void SetPositionFromEnd(int frameFromEnd)
        {
            Broadcast(iRSDKSharp.BroadcastMessageTypes.ReplaySetPlayPosition,
                (int)iRSDKSharp.ReplayPositionModeTypes.End,
                frameFromEnd);
        }
    }
}
