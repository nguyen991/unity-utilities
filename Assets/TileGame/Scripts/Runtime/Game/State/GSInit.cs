using Cysharp.Threading.Tasks;
using NUtilities.Helper;
using TileGame.Game.Model;
using TileGame.Level;

namespace TileGame.Game.State
{
    public class GSInit : GameFSMState
    {
        private readonly LevelSystem _levelSystem;
        
        public GSInit(LevelSystem levelSystem) : base(GameConst.State.Init)
        {
            _levelSystem = levelSystem;
        }

        public override async UniTask EnterAsync(object context)
        {
            // get the start game arguments
            if (context is not StartGameModel args)
            {
                args = new StartGameModel() { level = 1 };
                Log.D("No start game arguments provided, defaulting to level 1.");
            }
            
            // load the level
            var level = await _levelSystem.LoadLevel(args.level);
            StateMachine.Owner.gridController.Init(level);
            
            // change to next state
            SetTransition(GameConst.State.Play);
        }
    }
}