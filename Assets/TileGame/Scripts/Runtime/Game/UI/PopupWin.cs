using System;
using NUtilities.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace TileGame.Game.UI
{
    public class PopupWin : MonoBehaviour
    {
        public Button quitButton;
        public Button nextButton;

        void Start()
        {
            quitButton.onClick.AddListener(QuitGame);
            nextButton.onClick.AddListener(NextLevel);
        }

        private void QuitGame()
        {
            Hide(new PopupWinResponse() { next = false });
        }

        private void NextLevel()
        {
            Hide(new PopupWinResponse() { next = true });
        }

        private void Hide(PopupWinResponse response)
        {
            GetComponent<Popup>().Hide(response);
        }

        public class PopupWinResponse
        {
            public bool next;
        }
    }
}
