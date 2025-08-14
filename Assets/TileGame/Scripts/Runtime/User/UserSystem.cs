using System;
using NUtilities.Save;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.User
{
    public class UserSystem : IInitializable
    {
        private readonly SaveSystem _saveSystem;
        
        public User User { get; private set; }
        
        public UserSystem(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }
        
        public void Initialize()
        {
            // create user data
            User = new User();
            
            // load user data
            _saveSystem.Load(User, "user");
        }
    }
}