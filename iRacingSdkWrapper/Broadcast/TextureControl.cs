using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;

namespace iRacingSdkWrapper.Broadcast
{
    /// <summary>
    /// Provides control over reloading of car textures.
    /// </summary>
    public class TextureControl : BroadcastBase
    {
        internal  TextureControl(SdkWrapper wrapper) : base(wrapper)
        {
        }

        /// <summary>
        /// Reload all car textures.
        /// </summary>
        public void Reload()
        {
            Broadcast(BroadcastMessageTypes.ReloadTextures, (int)ReloadTexturesModeTypes.All, 0, 0);
        }

        /// <summary>
        /// Reload car textures for the specified car.
        /// </summary>
        /// <param name="carIdx">The ID (0-64) of the car to reload.</param>
        public void Reload(int carIdx)
        {
            Broadcast(BroadcastMessageTypes.ReloadTextures, (int)ReloadTexturesModeTypes.CarIdx, carIdx, 0);
        }
    }
}
