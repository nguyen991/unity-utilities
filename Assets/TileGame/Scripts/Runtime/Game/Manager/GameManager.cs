using System;
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
    public class GameManager : ITickable, IAsyncStartable, IDisposable
    {
        private readonly GameFSM _stateMachine;
        private readonly LoadingSystem _loadingSystem;
        private readonly GameController _gameController;
        
        private StartGameArgs _startGameArgs;
        private GameModel _gameModel;
        
        public GameModel GameModel => _gameModel;
        public GameController GameController => _gameController;
        
        public GameManager(IObjectResolver resolver, LoadingSystem loadingSystem, GameController gameController, StartGameArgs startGameArgs, LevelSystem levelSystem)
        {
            _loadingSystem = loadingSystem;
            _gameController = gameController;
            _startGameArgs = startGameArgs;
            _gameModel = new GameModel();
            
            // create state machine
            _stateMachine = new GameFSM(this);
            
            // register states
            _stateMachine.AddState(resolver.Resolve<GSInit>());
            _stateMachine.AddState(resolver.Resolve<GSPlay>());
            _stateMachine.AddState(resolver.Resolve<GSPause>());
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            // wait for the next frame to ensure all initializations are complete
            await UniTask.NextFrame(cancellation);
            
            // start state machine
            _stateMachine.ChangeState(GameConst.State.Init, _startGameArgs);
            
            // hide loading screen
            _loadingSystem.Hide();
        }
        
        public void Tick()
        {
            _stateMachine.Update(Time.deltaTime);
        }

        public void Dispose()
        {
            _stateMachine.Destroy();
        }
    }
}