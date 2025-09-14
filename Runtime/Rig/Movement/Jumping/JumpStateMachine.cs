using KadenZombie8.BIMOS.Core.StateMachine;

namespace KadenZombie8.BIMOS.Rig.Movement
{
    public class JumpStateMachine : BaseStateMachine<JumpStateMachine>
    {
        public Jumping Jumping { get; private set; }

        protected override void OnAwake() => Jumping = GetComponent<Jumping>();
    }
}
