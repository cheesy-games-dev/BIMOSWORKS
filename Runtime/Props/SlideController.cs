using UnityEngine;

namespace KadenZombie8.BIMOS {
    public class SlideController : MonoBehaviour {
        private ConfigurableJoint _joint;
        public Rigidbody ConnectedBody;
        [Header("Slide Drive")]
        public float SlideSpring = 100f;
        public float SlideDamper = 50f;
        [Range(0, int.MaxValue)]
        public float SlideMaxForce = int.MaxValue;
        [Header("Slide Soft Limit")]
        private Vector3 baseOrigin;
        private Vector3 endOrigin;

        public Transform EndOriginTransform;
        public float SlideBounciness = 0f;
        [SerializeField] bool _frozen;
        public bool Frozen {
            get { return _frozen; }
            set {
                _frozen = value;
                _joint.zMotion = _frozen ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
            }
        }
        public void Freeze() {
            Frozen = true;
        }
        public void UnFreeze() {
            Frozen = false;
        }
        private void Awake() {
            baseOrigin = transform.localPosition;
            endOrigin = EndOriginTransform.localPosition;
            if (!ConnectedBody || ConnectedBody == GetComponent<Rigidbody>()) {
                ConnectedBody = GetComponentInParent<Rigidbody>();
            }
        }
        private void Start() {        
            _joint = GetComponent<ConfigurableJoint>();
            if (!_joint)
                _joint = gameObject.AddComponent<ConfigurableJoint>();
            _joint.connectedBody = ConnectedBody;
            _joint.autoConfigureConnectedAnchor = false;
            FreezeMotions();
            FixSlideLimitsAndDrives();
            Vector3 anchor = baseOrigin;
            anchor.z = endOrigin.z;
            _joint.connectedAnchor = anchor;
            Vector3 target = new();
            target.z = endOrigin.z;
            _joint.targetPosition = target;
        }

        private void FixSlideLimitsAndDrives() {
            JointDrive SlideDrive = new() {
                positionSpring = SlideSpring,
                positionDamper = SlideDamper,
                maximumForce = SlideMaxForce,
            };
            SoftJointLimit SlideJointLimit = new() {
                limit = Vector3.Distance(baseOrigin, endOrigin),
                bounciness = SlideBounciness,
            };
            _joint.linearLimit = SlideJointLimit;
            _joint.zDrive = SlideDrive;
        }

        private void FreezeMotions() {
            _joint.xMotion = ConfigurableJointMotion.Locked;
            _joint.yMotion = ConfigurableJointMotion.Locked;
            Frozen = _frozen;
            _joint.angularXMotion = ConfigurableJointMotion.Locked;
            _joint.angularYMotion = ConfigurableJointMotion.Locked;
            _joint.angularZMotion = ConfigurableJointMotion.Locked;
        }
    }
}
