using Cysharp.Threading.Tasks;
using NUtilities.Pool;
using TileGame.Game.Model;
using TileGame.Level;

namespace TileGame.Game.State
{
    public class GSInit : GameFSMState
    {
        private readonly LevelSystem _levelSystem;
        private readonly PoolSystem _poolSystem;

        public GSInit(LevelSystem levelSystem, PoolSystem poolSystem)
            : base(GameConst.State.Init)
        {
            _levelSystem = levelSystem;
            _poolSystem = poolSystem;
        }

        public override async UniTask EnterAsync(object context)
        {
            // get the start game arguments
            if (context is not StartGameArgs args)
            {
                args = new StartGameArgs() { level = Container.GameModel.level };
            }

            // clear previous tiles
            Container.GameModel.tiles.ForEach(tile => _poolSystem.Release(tile.gameObject));
            Container.GameModel.tiles.Clear();
            Container.GameModel.holdingTiles.ForEach(tile => _poolSystem.Release(tile.gameObject));
            Container.GameModel.holdingTiles.Clear();
            Container.GameModel.isExpandHolding = false;

            // load the level
            Container.GameModel.level = args.level;
            var level = await _levelSystem.LoadLevel(args.level);
            Container.GameController.grid.Init(level);
            Container.GameModel.UpdateOverlayTiles();

            // change to next state
            SetTransition(GameConst.State.Play);
        }
    }
}
