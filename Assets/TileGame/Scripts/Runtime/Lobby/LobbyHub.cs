using System;
using Cysharp.Threading.Tasks;
using NUtilities.Loading;
using TileGame.Game;
using TileGame.Game.Model;
using TileGame.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TileGame.Lobby
{
    public class LobbyHub : MonoBehaviour
    {
        public Button startButton;
        public TMP_InputField levelInputField;

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
            _loadingSystem.Hide();
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            var level = 1;
            if (int.TryParse(levelInputField.text, out var parsedLevel))
            {
                level = parsedLevel;
            }
            _loadingSystem
                .ReplaceScene(GameConst.Scene.Game, new StartGameArgs() { level = level })
                .Forget();
        }
    }
}
