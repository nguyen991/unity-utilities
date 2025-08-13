using TileGame.Game.Controller;
using UnityEngine;

namespace TileGame.Game.State
{
    public class GSPlay : GameFSMState
    {
        public GSPlay() : base(GameConst.State.Play)
        {
        }

        public override void Enter(object context)
        {
            Debug.Log("GSPlay Enter");
            
            // add event listener for tile selection
            StateMachine.Owner.gridController.OnTileSelectedEvent = OnTileSelected;
        }
        
        private void OnTileSelected(Tile tile)
        {
            Debug.Log("OnTileSelected" + tile.gameObject.name);
        }

        public override void Exit()
        {
            // remove event listener for tile selection
            StateMachine.Owner.gridController.OnTileSelectedEvent = null;
        }
    }
}