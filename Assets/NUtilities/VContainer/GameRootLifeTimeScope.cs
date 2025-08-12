using System;
using NUtilities.Loading;
using NUtilities.Popup;
using NUtilities.Save;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NUtilities.VContainer
{
    public class GameRootLifeTimeScope : LifetimeScope
    {
        [Header("Scene Loading")]
        public GameObject loadingViewPrefab;

        [Header("Popup")]
        public PopupSO popup;

        [Header("Save System")]
        public SaveSO saveSO;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register entries services
            builder.UseEntryPoints(
                Lifetime.Singleton,
                entryPoints =>
                {
                    // save system
                    entryPoints.Add<SaveService>().WithParameter("config", saveSO).AsSelf();

                    // popup service
                    entryPoints.Add<PopupService>().WithParameter("config", popup).AsSelf();

                    // loading service
                    entryPoints
                        .Add<LoadingService>()
                        .WithParameter("prefab", loadingViewPrefab)
                        .AsSelf();
                }
            );
        }
    }
}
