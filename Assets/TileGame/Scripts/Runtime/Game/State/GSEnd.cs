using Cysharp.Threading.Tasks;
using NUtilities.Loading;
using NUtilities.Popup;
using TileGame.Game.Model;
using TileGame.Game.UI;
using UnityEngine;

namespace TileGame.Game.State
{
    public class GSEnd : GameFSMState
    {
        private readonly PopupSystem _popupSystem;
        private readonly LoadingSystem _loadingSystem;

        public GSEnd(PopupSystem popupSystem, LoadingSystem loadingSystem)
            : base(GameConst.State.End)
        {
            _popupSystem = popupSystem;
            _loadingSystem = loadingSystem;
        }

        public override void Enter(object context)
        {
            _popupSystem
                .ShowAsync(GameConst.Popup.End)
                .ContinueWith(context =>
                {
                    var response = context as PopupWin.PopupWinResponse;
                    if (response.next)
                    {
                        // load next game
                        SetTransition(
                            GameConst.State.Init,
                            new StartGameArgs() { level = Container.GameModel.level + 1 }
                        );
                    }
                    else
                    {
                        // back to lobby
                        _loadingSystem.ReplaceScene(GameConst.Scene.Lobby).Forget();
                    }
                });
        }
    }
}
