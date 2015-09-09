using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRacingSdkWrapper.Bitfields;
using iRSDKSharp;

namespace iRacingSdkWrapper.Broadcast
{
    /// <summary>
    /// Provides control over the replay camera and where it is focused.
    /// </summary>
    public class CameraControl : BroadcastBase
    {
        internal CameraControl(SdkWrapper wrapper) : base(wrapper)
        {
        }

        /// <summary>
        /// Switch to the 'focus on crashes' dynamic camera.
        /// </summary>
        public void FocusOnCrashes()
        {
            SwitchToPosition(-3);
        }

        /// <summary>
        /// Switch to the 'focus on leader' dynamic camera.
        /// </summary>
        public void FocusOnLeader()
        {
            SwitchToPosition(-2);
        }

        /// <summary>
        /// Switch to the 'focus on most exciting' dynamic camera.
        /// </summary>
        public void FocusMostExciting()
        {
            SwitchToPosition(-1);
        }

        /// <summary>
        /// Switch the camera to the specified position and set the camera group.
        /// </summary>
        /// <param name="position">The position of the car to switch to.</param>
        /// <param name="group">The camera group to use.</param>
        public void SwitchToPosition(int position, int group)
        {
            Broadcast(BroadcastMessageTypes.CamSwitchPos, position, group, 0);
        }

        /// <summary>
        /// Switch the camera to the specified position.
        /// </summary>
        /// <param name="position">The position of the car to switch to.</param>
        public void SwitchToPosition(int position)
        {
            SwitchToPosition(position, 0);
        }

        /// <summary>
        /// Switch the camera to the specified (raw) car number and set the camera group.
        /// </summary>
        /// <param name="carNumberRaw">The car number of the car to switch to. This must be the RAW car number.</param>
        /// <param name="group">The camera group to use.</param>
        public void SwitchToCar(int carNumberRaw, int group)
        {
            Broadcast(BroadcastMessageTypes.CamSwitchNum, carNumberRaw, group, 0);
        }

        /// <summary>
        /// Switch the camera to the specified (raw) car number and set the camera group.
        /// </summary>
        /// <param name="carNumberRaw">The car number of the car to switch to. This must be the RAW car number.</param>
        public void SwitchToCar(int carNumberRaw)
        {
            SwitchToCar(carNumberRaw, 0);
        }

        /// <summary>
        /// Switch the camera group to use.
        /// </summary>
        /// <param name="group">The camera group to use.</param>
        public void SwitchGroup(int group)
        {
            Broadcast(BroadcastMessageTypes.CamSwitchPos, 0, group, 0);
        }

        /// <summary>
        /// Set the state of the camera to a (combination of) specified states.
        /// </summary>
        /// <param name="state">A combination of specified states as a bitfield.</param>
        public void SetCameraState(CameraState state)
        {
            Broadcast(BroadcastMessageTypes.CamSetState, (int) state.Value, 0);
        }
    }
}
