using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUtilities.Loading;
using TileGame.Game.Controller;
using TileGame.Game.Model;
using TileGame.Game.State;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.Game.Manager
{
    public class GameManager : ITickable, IAsyncStartable, IDisposable
    {
        private readonly LoadingSystem _loadingSystem;
        private readonly GameController _gameController;
        private readonly IObjectResolver _resolver;

        private GameFSM _stateMachine;
        private StartGameArgs _startGameArgs;
        private GameModel _gameModel;

        public GameModel GameModel => _gameModel;
        public GameController GameController => _gameController;

        public GameManager(
            IObjectResolver resolver,
            LoadingSystem loadingSystem,
            GameController gameController,
            StartGameArgs startGameArgs
        )
        {
            _resolver = resolver;
            _loadingSystem = loadingSystem;
            _gameController = gameController;
            _startGameArgs = startGameArgs;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            // wait for the next frame to ensure all initializations are complete
            await UniTask.NextFrame(cancellation);

            // create game model
            _gameModel = new GameModel();

            // create state machine
            _stateMachine = new GameFSM(this);

            // register states
            _stateMachine.AddState(_resolver.Resolve<GSInit>());
            _stateMachine.AddState(_resolver.Resolve<GSPlay>());
            _stateMachine.AddState(_resolver.Resolve<GSPause>());
            _stateMachine.AddState(_resolver.Resolve<GSEnd>());

            // start state machine
            _stateMachine.ChangeState(GameConst.State.Init, _startGameArgs);

            // hide loading screen
            _loadingSystem.Hide();
        }

        public void Tick()
        {
            _stateMachine?.Update(Time.deltaTime);
        }

        public void Dispose()
        {
            _stateMachine.Destroy();
        }
    }
}
