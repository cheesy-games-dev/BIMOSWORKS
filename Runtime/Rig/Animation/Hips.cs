using UnityEngine;

namespace KadenZombie8.BIMOS.Rig
{
    public class Hips : MonoBehaviour
    {
        private BIMOSRig _rig;

        [SerializeField]
        private AnimationCurve _hipsBackCurve = AnimationCurve.Linear(0f, 0f, 1f, 0.5f);

        private Transform _camera;
        private Rigidbody _locomotionSphereRigidbody;
        private SphereCollider _locomotionSphereCollider;

        private float _spineLength;
        private Transform _headCameraOffset;

        private void Start()
        {
            _rig = BIMOSRig.Instance;
            _camera = _rig.ControllerRig.Transforms.Camera;
            _locomotionSphereRigidbody = _rig.PhysicsRig.Rigidbodies.LocomotionSphere;
            _locomotionSphereCollider = _rig.PhysicsRig.Colliders.LocomotionSphere;
            _headCameraOffset = _rig.ControllerRig.Transforms.HeadCameraOffset;
            _spineLength = Vector3.Distance(_rig.AnimationRig.Transforms.Head.position, _rig.AnimationRig.Transforms.Hips.position);
        }

        void Update()
        {
            var floor = _locomotionSphereRigidbody.position.y + 0.2f;
            var crouchingPercent = 1f - (_headCameraOffset.position.y - floor) / 1.1f;
            var back = _hipsBackCurve.Evaluate(crouchingPercent);
            var rotation = transform.rotation * Quaternion.Euler(back * 90f, 0f, 0f);
            print(crouchingPercent);
            transform.position = _headCameraOffset.position + rotation * (Vector3.down * _spineLength);
        }
    }
}