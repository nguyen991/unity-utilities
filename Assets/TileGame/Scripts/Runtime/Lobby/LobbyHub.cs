using System;
using Cysharp.Threading.Tasks;
using NUtilities.Loading;
using TileGame.Game.Model;
using TileGame.Level;
using TileGame.User;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TileGame.Lobby
{
    public class LobbyHub : MonoBehaviour
    {
        public Button startButton;
        
        private LoadingSystem _loadingSystem;
        private UserSystem _userSystem;

        [Inject]
        public void Inject(LoadingSystem loadingSystem, UserSystem userSystem)
        {
            _loadingSystem = loadingSystem;
            _userSystem = userSystem;
        }

        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            _loadingSystem.ReplaceScene("Game", new StartGameArgs() { level = 1 }).Forget();
        }
    }
}
