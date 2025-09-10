using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Tempor
{
    /// <summary>
    /// Handles character's virtual movement with smooth locomotion
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The walk speed of the character")]
        private float _defaultWalkSpeed = 1.5f;

        public LocomotionSphere LocomotionSphere { get; private set; }
        private Transform _mainCameraTransform;
        private Vector2 _moveDirection;

        public float WalkSpeed { get; set; }

        public bool IsRunning { get; private set; }

        /// <summary>
        /// The product of this and the walk speed is the run speed
        /// </summary>
        public float RunSpeedMultiplier { get; set; } = 2f;

        private void Awake()
        {
            _mainCameraTransform = Camera.main?.transform;
            LocomotionSphere = GetComponentInChildren<LocomotionSphere>();
            
            //_inputReader.MoveEvent += OnMove;
            //_inputReader.RunEvent += OnToggleRun;

            ResetWalkSpeed();
        }
        
        private void OnMove(Vector2 direction) => _moveDirection = direction;
        private void OnToggleRun() => IsRunning = !IsRunning;

        private void FixedUpdate() => Move();

        private void Move()
        {
            if (_moveDirection.magnitude < 0.1f)
                IsRunning = false;

            var currentSpeed = WalkSpeed;
            if (IsRunning)
                currentSpeed *= RunSpeedMultiplier;

            var headYaw = Quaternion.LookRotation(Vector3.Cross(_mainCameraTransform.right, Vector3.up));
            var targetLinearVelocity = headYaw * new Vector3(_moveDirection.x, 0, _moveDirection.y) * currentSpeed;
            LocomotionSphere.RollFromLinearVelocity(targetLinearVelocity);
        }

        /// <summary>
        /// Resets walk speed to avatar's walk speed
        /// </summary>
        public void ResetWalkSpeed() => WalkSpeed = _defaultWalkSpeed;
    }
}
