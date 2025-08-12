using UnityEngine;

namespace NUtilities.FSM
{
    public class State
    {
        public string Name { get; private set; }
        private TransitionState _transition;
        
        public State(string name)
        {
            Name = name;
        }

        public virtual void Enter(Object context)
        {
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void Exit()
        {
        }

        protected void SetTransition(string to, Object context)
        {
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
        public Object Context;
        
        public TransitionState(string state, Object context = null)
        {
            State = state;
            Context = context;
        }
    }
}