using System;
using UnityEngine;

namespace KadenZombie8.BIMOS.Core.StateMachine
{
    public abstract class BaseState<T> : ScriptableObject, IState<T> where T : IStateMachine<T>
    {
        protected T StateMachine;

        public event Action OnEnter;
        public event Action OnUpdate;
        public event Action OnExit;

        public void ConnectTo(T stateMachine) => StateMachine = stateMachine;

        public virtual void OnAwake() { }
        public virtual void OnStart() { }

        public void EnterState()
        {
            Enter();
            OnEnter?.Invoke();
        }

        protected virtual void Enter() { }

        public void UpdateState()
        {
            Update();
            OnUpdate?.Invoke();
        }

        protected virtual void Update() { }

        public void ExitState()
        {
            Exit();
            OnExit?.Invoke();
        }

        protected virtual void Exit() { }
    }
}
