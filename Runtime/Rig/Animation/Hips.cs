using UnityEngine;

namespace KadenZombie8.BIMOS.Rig
{
    public class Hips : MonoBehaviour
    {
        private BIMOSRig _player;

        [SerializeField]
        private Transform _hipStandTransform, _hipCrouchTransform;

        private Transform _camera;
        private Rigidbody _locomotionSphere;

        private void Start()
        {
            _player = BIMOSRig.Instance;
            _camera = _player.ControllerRig.Transforms.Camera;
            _locomotionSphere = _player.PhysicsRig.Rigidbodies.LocomotionSphere;
        }

        void Update()
        {
            float standingPercent = (_camera.position.y - _locomotionSphere.position.y + 0.2f) / 1.65f;
            transform.position = Vector3.Lerp(_hipCrouchTransform.position, _hipStandTransform.position, standingPercent);
        }
    }
}