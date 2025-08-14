using System;
using NUtilities.Loading;
using NUtilities.Pool;
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
        
        [Header("Pool System")]
        public PoolSO poolSO;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register entries services
            builder.UseEntryPoints(
                Lifetime.Singleton,
                entry =>
                {
                    // save system
                    entry.Add<SaveSystem>().WithParameter("config", saveSO).AsSelf();

                    // popup service
                    entry.Add<PopupSystem>().WithParameter("config", popup).AsSelf();

                    // loading service
                    entry
                        .Add<LoadingSystem>()
                        .WithParameter("prefab", loadingViewPrefab)
                        .AsSelf();
                    
                    // pool system
                    entry.Add<PoolSystem>().WithParameter("config", poolSO).AsSelf();
                }
            );
        }
    }
}
