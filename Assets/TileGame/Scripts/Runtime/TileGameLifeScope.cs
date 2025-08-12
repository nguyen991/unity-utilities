using System;
using NUtilities.Popup;
using TileGame.Level;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.Game
{
    public class TileGameLifeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // register level system
            builder.Register<LevelSystem>(Lifetime.Singleton).AsSelf();
        }
    }
}
