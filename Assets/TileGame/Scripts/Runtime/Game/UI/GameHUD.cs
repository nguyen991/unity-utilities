using System.Collections.Generic;
using TileGame.Game.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TileGame.Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        public Button backButton;
        public Button retryButton;
        public List<Button> boosterButtons;

        public UnityEvent<GameConst.BoosterType> onBoosterButtonClicked;

        void Start()
        {
            // add listeners to booster buttons
            for (var i = 0; i < boosterButtons.Count; i++)
            {
                var boosterType = (GameConst.BoosterType)i;
                boosterButtons[i]
                    .onClick.AddListener(() =>
                    {
                        onBoosterButtonClicked.Invoke(boosterType);
                    });
            }
        }
    }
}
