using Cysharp.Threading.Tasks;
using NUtilities.Helper;
using TileGame.Game.Controller;
using TileGame.Level;
using UnityEngine;

namespace TileGame.Game.State
{
    public class GSInit : NUtilities.FSM.State
    {
        private readonly LevelSystem _levelSystem;
        private readonly GridManager _gridManager;
        
        public GSInit(LevelSystem levelSystem, GridManager gridManager) : base(GameConst.State.Init)
        {
            _levelSystem = levelSystem;
            _gridManager = gridManager;
        }

        public override async UniTask EnterAsync(object context)
        {
            if (context is not GSInitContext initContext)
            {
                Log.E("GSInitContext is null. Cannot initialize game state.");
                return;
            }
            
            // load the level
            var level = await _levelSystem.LoadLevel(initContext.level);
            _gridManager.Init(level);
            
            // change to next state
            SetTransition(GameConst.State.Play);
        }
        
        /**
         *  Context for the GSInit state.
         *  Contains information about the level to be initialized.
         *  This can be extended to include more initialization parameters as needed.
         */
        public class GSInitContext
        {
            public int level;
        }
    }
}