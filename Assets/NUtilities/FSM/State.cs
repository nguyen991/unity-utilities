using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NUtilities.FSM
{
    public class State<T> where T : class
    {
        public string Name { get; private set; }
        public StateMachine<T> StateMachine { get; set; }
        
        private TransitionState _transition;
        
        public State(string name)
        {
            Name = name;
        }

        public virtual void Enter(object context)
        {
        }
        
        public virtual UniTask EnterAsync(object context)
        {
            return UniTask.CompletedTask;
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void Exit()
        {
        }

        protected void SetTransition(string to, object context = null, bool overrideExisting = false)
        {
            if (string.IsNullOrEmpty(to))
            {
                Debug.LogError("Transition state cannot be null or empty.");
                return;
            }
            if (_transition != null && !overrideExisting)
            {
                Debug.LogWarning($"Transition already set to {_transition.State}. Overriding with new transition to {to}.");
                return;
            }
            _transition = new TransitionState(to, context);
        }

        public TransitionState GetTransition()
        {
            var transition = _transition;
            _transition = null; // Clear transition after getting it
            return transition;
        }
    }

    public class TransitionState
    {
        public string State;
        public object Context;
        
        public TransitionState(string state, object context = null)
        {
            State = state;
            Context = context;
        }
    }
    
    public class GameObjectState : State<GameObject>
    {
        protected GameObjectState(string name) : base(name)
        {
        }
    }
}