using System;
using System.Collections.Generic;
using UnityEngine;

namespace NUtilities.FSM
{
    public class ActionState<T> : State<T> where T : class
    {
        private readonly List<Action<float>> _actions;
        
        public ActionState(string name) : base(name)
        {
            _actions = new List<Action<float>>();
        }

        public void AddAction(Action<float> action)
        {
            if (action == null)
            {
                Debug.LogError("Cannot add a null action to the ActionState.");
                return;
            }
            _actions.Add(action);
        }
        
        public override void Update(float deltaTime)
        {
            foreach (var action in _actions)
            {
                action.Invoke(deltaTime);
            }
        }
    }
}