using System;
using NUtilities.Loading;
using NUtilities.Popup;
using NUtilities.Save;
using UnityEngine;
using UnityEngine.Events;
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
        
        [Header("Other Builder Configurations")]
        public UnityEvent<IContainerBuilder>[] buildEvents;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register entries services
            builder.UseEntryPoints(
                Lifetime.Singleton,
                entry =>
                {
                    // save system
                    entry.Add<SaveService>().WithParameter("config", saveSO).AsSelf();

                    // popup service
                    entry.Add<PopupService>().WithParameter("config", popup).AsSelf();

                    // loading service
                    entry
                        .Add<LoadingService>()
                        .WithParameter("prefab", loadingViewPrefab)
                        .AsSelf();
                }
            );

            // Register other scopes
            foreach (var builFunc in buildEvents)
            {
                builFunc?.Invoke(builder);
            }
        }
    }
}
