using System.Threading;
using Cysharp.Threading.Tasks;
using NUtilities.Loading;
using TileGame.Game.Controller;
using TileGame.Game.Model;
using TileGame.Game.State;
using TileGame.Level;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.Game.Manager
{
    public class GameManager : ITickable, IAsyncStartable
    {
        private readonly GameFSM _stateMachine;
        private readonly LoadingService _loadingService;
        private readonly GameController _gameController;
        
        private StartGameModel _startGameArgs;
        
        public GameManager(IObjectResolver resolver, LoadingService loadingService, GameController gameController, StartGameModel startGameArgs, LevelSystem levelSystem)
        {
            _loadingService = loadingService;
            _gameController = gameController;
            _startGameArgs = startGameArgs;
            
            // create state machine
            _stateMachine = new GameFSM(_gameController);
            
            // register states
            _stateMachine.AddState(resolver.Resolve<GSInit>());
            _stateMachine.AddState(resolver.Resolve<GSPlay>());
            _stateMachine.AddState(resolver.Resolve<GSPause>());
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            // wait for the next frame to ensure all initializations are complete
            await UniTask.NextFrame(cancellation);
            
            // hide loading screen
            _loadingService.Hide();
            
            // start state machine
            _stateMachine.ChangeState(GameConst.State.Init, _startGameArgs);
        }
        
        public void Tick()
        {
            _stateMachine.Update(Time.deltaTime);
        }
    }
}