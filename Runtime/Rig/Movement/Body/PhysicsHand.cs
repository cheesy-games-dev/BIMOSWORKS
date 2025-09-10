using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Movement
{
    public class PhysicsHand : MonoBehaviour
    {
        public Transform Target;
        public bool IsMantling;

        [SerializeField]
        private LocomotionSphere _locomotionSphere;

        private ConfigurableJoint _handJoint;
        private Rigidbody _pelvis;

        private void Awake()
        {
            _handJoint = GetComponent<ConfigurableJoint>();
            _pelvis = _handJoint.connectedBody;
        }

        private void FixedUpdate()
        {
            _handJoint.targetPosition = _pelvis.transform.InverseTransformPoint(Target.position);
            _handJoint.targetRotation = Quaternion.Inverse(_pelvis.rotation) * Target.rotation;
        }

        private void OnCollisionStay(Collision collision)
        {
            GetContactPoints(collision);
        }

        private void OnCollisionExit()
        {
            IsMantling = false;
        }

        private void GetContactPoints(Collision collision)
        {
            var contactPoints = new ContactPoint[collision.contactCount];
            collision.GetContacts(contactPoints);
            var resultantForce = Vector3.zero;
            
            foreach (var contactPoint in contactPoints)
                resultantForce += contactPoint.impulse / Time.fixedDeltaTime;
            
            Debug.DrawRay(transform.position, resultantForce);
            IsMantling = resultantForce.y > 0f;
        }
    }
}
