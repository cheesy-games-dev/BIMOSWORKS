using UnityEngine;
using UnityEngine.InputSystem;

namespace KadenZombie8.BIMOS.Rig.Movement
{
    public class ScreenModeCamera : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _lookReference;

        [SerializeField]
        private ScreenModeController _leftHand;

        [SerializeField]
        private ScreenModeController _rightHand;

        private readonly float _lookSensitivity = 0.02f;

        private void Awake() => _lookReference.action.Enable();

        private void OnEnable() => Cursor.lockState = CursorLockMode.Locked;

        private void OnDisable() => Cursor.lockState = CursorLockMode.None;

        private void Update()
        {
            if (_leftHand.IsPositionUnlocked || _rightHand.IsPositionUnlocked) return;
            if (_leftHand.IsDepthUnlocked || _rightHand.IsDepthUnlocked) return;
            if (_leftHand.IsRotationUnlocked || _rightHand.IsRotationUnlocked) return;

            var look = _lookSensitivity * _lookReference.action.ReadValue<Vector2>();
            transform.Rotate(Vector3.up, look.x, Space.World);
            transform.Rotate(Vector3.left, look.y);
        }
    }
}
