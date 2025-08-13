using TileGame.Game.Controller;
using TileGame.Game.Manager;
using TileGame.Game.State;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.Game
{
    public class GameScope : LifetimeScope
    {
        [Header("References")]
        public GameController gameController;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GSInit>(Lifetime.Transient).AsSelf();
            builder.Register<GSPlay>(Lifetime.Transient).AsSelf();
            builder.Register<GSPause>(Lifetime.Transient).AsSelf();
            builder.RegisterEntryPoint<GameManager>().WithParameter("gameController", gameController);
        }
    }
}