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
            // add event listener for tile selection
            Container.GameController.grid.OnTileSelectedEvent = OnTileSelected;
        }
        
        private void OnTileSelected(Tile tile)
        {
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

        public override void Exit()
        {
            // remove event listener for tile selection
            Container.GameController.grid.OnTileSelectedEvent = null;
        }
    }
}