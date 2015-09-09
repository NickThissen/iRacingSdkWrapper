using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;

namespace iRacingSdkWrapper.Broadcast
{
    /// <summary>
    /// Provides control over the chat window.
    /// </summary>
    public class ChatControl : BroadcastBase
    {
        internal ChatControl(SdkWrapper wrapper) : base(wrapper)
        {
        }

        /// <summary>
        /// Clear the chat window.
        /// </summary>
        public void Clear()
        {
            Broadcast(BroadcastMessageTypes.ChatCommand, (int)ChatCommandModeTypes.Cancel, 0);
        }

        /// <summary>
        /// Start a reply to the last private message.
        /// </summary>
        public void Reply()
        {
            Broadcast(BroadcastMessageTypes.ChatCommand, (int)ChatCommandModeTypes.Reply, 0);
        }

        /// <summary>
        /// Activate the chat window.
        /// </summary>
        public void Activate()
        {
            Broadcast(BroadcastMessageTypes.ChatCommand, (int)ChatCommandModeTypes.BeginChat, 0);
        }

        /// <summary>
        /// Send a macro to the chat window.
        /// </summary>
        /// <param name="macro">The macro to send.</param>
        public void SendMacro(int macro)
        {
            if (macro < 0 || macro > 14)
                throw new ArgumentOutOfRangeException("macro", "Macro must be between 0 and 14.");
            Broadcast(BroadcastMessageTypes.ChatCommand, (int)ChatCommandModeTypes.Macro, macro, 0);
        }
    }
}
