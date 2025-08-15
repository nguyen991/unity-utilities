using System;
using NUtilities.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace TileGame.Game.UI
{
    public class PopupLose : MonoBehaviour
    {
        public Button retryButton;
        public Button quitButton;
        public Button moreButton;

        void Start()
        {
            retryButton.onClick.AddListener(OnRetry);
            quitButton.onClick.AddListener(QuitGame);
            moreButton.onClick.AddListener(MoreChance);
        }

        private void QuitGame()
        {
            Hide(new PopupLoseResponse() { quit = true });
        }

        private void MoreChance()
        {
            Hide(new PopupLoseResponse() { more = true });
        }

        private void OnRetry()
        {
            Hide(new PopupLoseResponse() { retry = true });
        }

        private void Hide(PopupLoseResponse response)
        {
            GetComponent<Popup>().Hide(response);
        }

        public class PopupLoseResponse
        {
            public bool retry;
            public bool quit;
            public bool more;
        }
    }
}
