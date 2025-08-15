using System.Collections.Generic;
using UnityEngine;

namespace NUtilities.FSM
{
    public class StateMachine<T>
        where T : class
    {
        private readonly Dictionary<string, State<T>> _states;
        private State<T> _currentState;
        private TransitionState _currentTransition;

        public T Owner { get; private set; }

        public StateMachine(T owner)
        {
            Owner = owner;
            _states = new Dictionary<string, State<T>>();
            _currentState = null;
        }

        public string CurrentState => _currentState?.Name;

        public void AddState(State<T> state)
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
            state.StateMachine = this;
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

    /**
     *  GameObjectStateMachine
     *  A specialized state machine for GameObjects.
     *  It inherits from the generic StateMachine class and uses GameObject as the owner type
     */
    public class GameObjectStateMachine : StateMachine<GameObject>
    {
        public GameObjectStateMachine(GameObject owner)
            : base(owner) { }
    }
}
