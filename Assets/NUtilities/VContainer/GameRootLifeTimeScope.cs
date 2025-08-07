using System;
using NUtilities.Popup;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NUtilities.VContainer
{
    public class GameRootLifeTimeScope : LifetimeScope
    {
        public PopupSO popup;
        public bool preloadPopups = true;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(popup);
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<PopupService>().AsSelf();
            });
        }
    }
}