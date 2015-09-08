using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iRacingSdkWrapper.Broadcast
{
    public abstract class BroadcastBase
    {
        private readonly SdkWrapper _wrapper;

        internal BroadcastBase(SdkWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        protected void Broadcast(iRSDKSharp.BroadcastMessageTypes type, int var1, int var2)
        {
            if (!_wrapper.IsConnected) return;
            _wrapper.Sdk.BroadcastMessage(type, var1, var2);
        }
    }
}
