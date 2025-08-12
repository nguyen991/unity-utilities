using TileGame.Game.Controller;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.Game
{
    public class GameScope : LifetimeScope
    {
        public GridManager gridManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(gridManager).As<GridManager>();
        }
    }
}