using UnityEngine;

namespace KadenZombie8.BIMOS.Rig
{
    public class PhysicsHand : MonoBehaviour
    {
        public Transform Target, Controller;
        public Vector3 TargetOffsetPosition;
        public Quaternion TargetOffsetRotation;

        [SerializeField]
        private float _pGain;

        [SerializeField]
        private float _dGain;

        private PDVector3 _pdVector3;

        private BIMOSRig _player;

        private Rigidbody _rigidbody;
        private ConfigurableJoint _handJoint;

        private void Start()
        {
            _player = BIMOSRig.Instance;

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.solverIterations = 60;
            _rigidbody.solverVelocityIterations = 10;

            TargetOffsetRotation = Quaternion.identity;
            _handJoint = GetComponent<ConfigurableJoint>();

            _pdVector3 = new PDVector3(_pGain, _dGain);
        }

        private void FixedUpdate()
        {
            Vector3 targetPosition = Target.TransformPoint(TargetOffsetPosition);
            Vector3 headOffset = targetPosition - _player.PhysicsRig.Rigidbodies.Head.position;
            _handJoint.targetPosition = headOffset;

            // SM Target Velocity Logic.
            _pdVector3.UpdateProportionalGain(_pGain);
            _pdVector3.UpdateDerivativeGain(_dGain);
            _handJoint.targetVelocity = _pdVector3.CalculatePD(_handJoint.transform.position, Target.TransformPoint(TargetOffsetPosition),
                Time.fixedDeltaTime);

            //Rotation
            _handJoint.targetRotation = Target.rotation * TargetOffsetRotation;
        }
    }
}