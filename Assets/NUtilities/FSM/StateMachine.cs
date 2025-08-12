using System.Collections.Generic;
using UnityEngine;

namespace NUtilities.FSM
{
    public class StateMachine
    {
        private readonly Dictionary<string, State> _states;
        private State _currentState;
        private TransitionState _currentTransition;
        
        public StateMachine()
        {
            _states = new Dictionary<string, State>();
            _currentState = null;
        }
        
        public string CurrentState => _currentState?.Name;

        public void AddState(State state)
        {
            if (state == null)
            {
                Debug.LogError("Cannot add a null state to the state machine.");
                return;
            }
            
            if (!_states.TryAdd(state.Name, state))
            {
                Debug.LogWarning($"State {state.Name} already exists in the state machine.");
            }
        }

        public void ChangeState(TransitionState transition)
        {
            ChangeState(transition.State, transition.Context);
        }

        public void ChangeState(string name, object context = null)
        {
            if (!_states.TryGetValue(name, out var newState))
            {
                Debug.LogError($"State {name} does not exist in the state machine.");
                return;
            }
            
            if (newState == _currentState)
            {
                Debug.LogWarning($"Already in state: {name}");
                return;
            }

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter(context);
            _currentState.EnterAsync(context);
        }
        
        public void Update(float deltaTime)
        {
            // Update the current state if it exists
            _currentState?.Update(deltaTime);
            
            // Check for transitions
            _currentTransition = _currentState?.GetTransition();
            if (_currentTransition != null)
            {
                ChangeState(_currentTransition);
            }
        }

        public void Destroy()
        {
            _currentState?.Exit();
        }
    }
}
