using System.Linq;
using Cysharp.Threading.Tasks;
using LitMotion;
using NUtilities.Loading;
using NUtilities.Popup;
using TileGame.Game.Controller;
using TileGame.Game.Manager;
using TileGame.Game.UI;
using UnityEngine;

namespace TileGame.Game.State
{
    public class GSPlay : GameFSMState
    {
        private readonly PopupSystem _popupSystem;
        private readonly LoadingSystem _loadingSystem;
        private readonly GameHUD _gameHUD;
        private readonly BoosterManager _boosterManager;

        public GSPlay(
            PopupSystem popupSystem,
            LoadingSystem loadingSystem,
            GameHUD gameHUD,
            BoosterManager boosterManager
        )
            : base(GameConst.State.Play)
        {
            _popupSystem = popupSystem;
            _loadingSystem = loadingSystem;
            _gameHUD = gameHUD;
            _boosterManager = boosterManager;
        }

        public override void Enter(object context)
        {
            // add event listener for tile selection
            Container.GameController.grid.OnTileSelectedEvent = OnTileSelected;
            Container.GameController.hold.onArrangeComplete = () => ArrangeCompleted().Forget();
            _gameHUD.backButton.onClick.AddListener(QuitGame);
            _gameHUD.retryButton.onClick.AddListener(Retry);
            _gameHUD.onBoosterButtonClicked.AddListener(UseBooster);
        }

        private void UseBooster(GameConst.BoosterType boosterType)
        {
            //TODO: check booster remain uses

            // active booster
            _boosterManager.UseBooster(boosterType);
        }

        private void OnTileSelected(Tile tile)
        {
            // check is full
            if (Container.GameModel.IsHoldingFull())
            {
                return;
            }

            // check if the tile is on top of the grid
            if (!Container.GameModel.IsOnTop(tile))
            {
                tile.Shake();
                return;
            }

            // move the tile to container
            Container.GameModel.AddSelectTile(tile);
            Container.GameController.hold.AddTile(tile);
        }

        private async UniTaskVoid ArrangeCompleted()
        {
            // check if the holding tiles are full
            if (Container.GameModel.IsHoldingFull())
            {
                ShowLose();
                return;
            }

            // check is completed game
            if (Container.GameModel.IsCompleted())
            {
                SetTransition(GameConst.State.End);
                return;
            }

            // get remove tiles
            var removeTiles = Container.GameModel.holdingTiles.Where(t =>
                t.State == Tile.TileState.Removing
            );
            if (removeTiles.Count() == 0)
            {
                return;
            }

            // wait for all tiles to be removed
            await UniTask.WhenAll(
                removeTiles.Select(async t =>
                {
                    await t.Release();
                })
            );
            Container.GameModel.RemoveHoldingTiles(removeTiles.ToList());

            // rearrange remaining tiles
            Container.GameController.hold.ArrangeHolingTiles();
        }

        private void ShowLose()
        {
            _popupSystem
                .ShowAsync(GameConst.Popup.Lose)
                .ContinueWith(context =>
                {
                    var response = context as PopupLose.PopupLoseResponse;
                    if (response.retry)
                    {
                        Retry();
                    }
                    else if (response.quit)
                    {
                        QuitGame();
                    }
                    else if (response.more)
                    {
                        MoreChance();
                    }
                });
        }

        private void MoreChance()
        {
            // return last 3 holding tile
            var tiles = Container.GameModel.holdingTiles.TakeLast(3).ToList();
            Container.GameModel.RemoveHoldingTiles(tiles);
            Container.GameModel.tiles.AddRange(tiles);
            Container.GameController.grid.ArrangeTiles();
            Container.GameModel.UpdateOverlayTiles();
        }

        private void QuitGame()
        {
            _loadingSystem.ReplaceScene(GameConst.Scene.Lobby).Forget();
        }

        private void Retry()
        {
            SetTransition(GameConst.State.Init);
        }

        public override void Exit()
        {
            // remove event listener for tile selection
            Container.GameController.grid.OnTileSelectedEvent = null;
            Container.GameController.hold.onArrangeComplete = null;
            _gameHUD.backButton.onClick.RemoveListener(QuitGame);
            _gameHUD.retryButton.onClick.RemoveListener(Retry);
            _gameHUD.onBoosterButtonClicked.RemoveListener(UseBooster);
        }
    }
}
