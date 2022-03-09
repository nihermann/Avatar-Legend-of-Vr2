using UnityEngine;
using Valve.VR;

namespace VRAvatar
{

    /// <summary>
    /// Script holding relevant references to the VR components of the participant
    /// </summary>
    public class VRPlayer : MonoBehaviour
    {
        /// <summary>
        /// Left hand of the VR player.
        /// </summary>
        public LeftHand leftHand;
        
        /// <summary>
        /// Right hand of the VR player.
        /// </summary>
        public RightHand rightHand;
    }
}
