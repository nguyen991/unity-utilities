using System;
using TileGame.Level;
using TileGame.User;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TileGame.Game
{
    public class TileGameLifeScope : MonoBehaviour
    {
        public void Configure(IContainerBuilder builder)
        {
            builder.UseEntryPoints(Lifetime.Singleton, entry =>
            {
                entry.Add<LevelSystem>().AsSelf();
                entry.Add<UserSystem>().AsSelf();
            });
        }
    }
}
