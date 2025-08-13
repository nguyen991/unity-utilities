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
        
        private LoadingService _loadingService;
        private UserSystem _userSystem;

        [Inject]
        public void Inject(LoadingService loadingService, UserSystem userSystem)
        {
            _loadingService = loadingService;
            _userSystem = userSystem;
        }

        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            _loadingService.ReplaceScene("Game", new StartGameModel() { level = 1 }).Forget();
        }
    }
}
