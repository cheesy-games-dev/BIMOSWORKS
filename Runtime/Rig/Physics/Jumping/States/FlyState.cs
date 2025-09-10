using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Tempor
{
    /// <summary>
    /// Airborne state
    /// </summary>
    public class FlyState : JumpState
    {
        private bool _isFalling;
        private float _airTime;
        private readonly float _minAirTime = 0.01f;

        protected override void Enter()
        {
            _airTime = 0f;
            _isFalling = false;
            Jumping.PhysicsRig.Joints.Pelvis.massScale = 2f;
        }

        protected override void Update()
        {
            if (_airTime >= _minAirTime && Jumping.LocomotionSphere.IsGrounded)
                StateMachine.ChangeState<StandState>();

            _airTime += Time.fixedDeltaTime;

            var sign = _isFalling ? -1f : 1f;
            var newLegHeight = Crouching.TargetLegHeight - Crouching.StandingLegHeight * Time.fixedDeltaTime * sign * 4f;
            if (newLegHeight > (Crouching.StandingLegHeight - Crouching.CrawlingLegHeight) / 2f)
                Crouching.TargetLegHeight = newLegHeight;

            if (Jumping.PhysicsRig.Rigidbodies.LocomotionSphere.linearVelocity.y < 0f && !_isFalling)
            {
                _isFalling = true;
                //float height = Jumping.PhysicsRig.Rigidbodies.LocomotionSphere.position.y - StateMachine.Jumping.PhysicsRig.Colliders.LocomotionSphere.radius;
                //Debug.LogError("Height reached: " + height);
            }
        }

        protected override void Exit()
        {
            Crouching.TargetLegHeight = Crouching.StandingLegHeight;
            Jumping.PhysicsRig.Joints.Pelvis.massScale = 1f;
        }
    }
}