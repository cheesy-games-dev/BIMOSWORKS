namespace KadenZombie8.BIMOS.Rig.Tempor
{
    /// <summary>
    /// State where the player is not engaged with jumping
    /// </summary>
    public class StandState : JumpState
    {
        protected override void Enter()
        {
            Jumping.OnAnticipate += AnticipateJump;
            Jumping.PhysicsRig.Joints.Pelvis.massScale = 0.1f;
        }

        protected override void Exit()
        {
            Jumping.OnAnticipate -= AnticipateJump;
            Jumping.PhysicsRig.Joints.Pelvis.massScale = 1f;
        }

        private void AnticipateJump() => StateMachine.ChangeState<CompressState>();
    }
}
