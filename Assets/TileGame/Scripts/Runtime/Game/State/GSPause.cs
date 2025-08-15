using Cysharp.Threading.Tasks;
using NUtilities.Popup;
using UnityEngine;

namespace TileGame.Game.State
{
    public class GSPause : GameFSMState
    {
        private readonly PopupSystem _popupSystem;

        public GSPause(PopupSystem popup)
            : base(GameConst.State.Pause)
        {
            _popupSystem = popup;
        }

        public override void Enter(object context)
        {
            // pause the game
            Time.timeScale = 0;

            // open pause menu UI
            _popupSystem
                .ShowAsync("pause")
                .ContinueWith(_ =>
                {
                    // exit state
                    SetTransition(GameConst.State.Play);
                });
        }

        public override void Exit()
        {
            // resume the game
            Time.timeScale = 1;
        }
    }
}
