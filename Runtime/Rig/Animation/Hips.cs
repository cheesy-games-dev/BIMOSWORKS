using UnityEngine;

namespace KadenZombie8.BIMOS.Rig
{
    public class Hips : MonoBehaviour
    {
        private BIMOSRig _rig;

        private Transform _camera;
        private Rigidbody _locomotionSphere;

        private float _spineLength;
        private Transform _headCameraOffset;

        private void Start()
        {
            _rig = BIMOSRig.Instance;
            _camera = _rig.ControllerRig.Transforms.Camera;
            _locomotionSphere = _rig.PhysicsRig.Rigidbodies.LocomotionSphere;
            _headCameraOffset = _rig.ControllerRig.Transforms.HeadCameraOffset;
            _spineLength = Vector3.Distance(_rig.AnimationRig.Transforms.Head.position, _rig.AnimationRig.Transforms.Hips.position);
        }

        void Update()
        {
            float standingPercent = (_camera.position.y - _locomotionSphere.position.y + 0.2f) / _rig.AnimationRig.AvatarEyeHeight;
            transform.position = _headCameraOffset.position + Vector3.down * _spineLength;
        }
    }
}