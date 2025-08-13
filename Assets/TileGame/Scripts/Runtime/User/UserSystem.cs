using System;
using NUtilities.Save;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.User
{
    public class UserSystem : IInitializable
    {
        private readonly SaveService _saveService;
        
        public User User { get; private set; }
        
        public UserSystem(SaveService saveService)
        {
            _saveService = saveService;
        }
        
        public void Initialize()
        {
            // create user data
            User = new User();
            
            // load user data
            _saveService.Load(User, "user");
        }
    }
}