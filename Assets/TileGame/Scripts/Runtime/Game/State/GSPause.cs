using UnityEngine;

namespace TileGame.Game.State
{
    public class GSPause : GameFSMState
    {
        public GSPause(string name) : base(GameConst.State.Pause)
        {
        }
    }
}