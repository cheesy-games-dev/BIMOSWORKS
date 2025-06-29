using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

namespace KadenZombie8.BIMOS.Rig
{
    public class Hand : MonoBehaviour
    {
        public HandAnimator HandAnimator;
        public Grabbable CurrentGrab;
        public HandInputReader HandInputReader;
        public Transform PalmTransform;
        public PhysicsHand PhysicsHand;
        public Transform PhysicsHandTransform;
        public GrabHandler GrabHandler;
        public bool IsLeftHand;
        public Hand OtherHand;
        public Collider PhysicsHandCollider;
        public Joint GrabJoint;

        [SerializeField]
        private InputActionReference _hapticAction;

        public void SendHapticImpulse(float amplitude, float duration)
            => OpenXRInput.SendHapticImpulse(_hapticAction, amplitude, duration);
    }
}
