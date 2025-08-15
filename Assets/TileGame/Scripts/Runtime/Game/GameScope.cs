using TileGame.Game.Controller;
using TileGame.Game.Manager;
using TileGame.Game.State;
using TileGame.Game.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.Game
{
    public class GameScope : LifetimeScope
    {
        [Header("References")]
        public GameController gameController;
        public GameHUD gameHUD;

        protected override void Configure(IContainerBuilder builder)
        {
            builder
                .RegisterEntryPoint<GameManager>()
                .WithParameter("gameController", gameController)
                .AsSelf();

            builder.Register<BoosterManager>(Lifetime.Scoped).AsSelf();
            builder.RegisterComponentInNewPrefab(gameHUD, Lifetime.Scoped);

            builder.Register<GSInit>(Lifetime.Transient).AsSelf();
            builder.Register<GSPlay>(Lifetime.Transient).AsSelf();
            builder.Register<GSPause>(Lifetime.Transient).AsSelf();
            builder.Register<GSEnd>(Lifetime.Transient).AsSelf();
        }
    }
}
