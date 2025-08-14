using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NUtilities.Popup
{
    public class PopupSystem : IInitializable
    {
        private readonly PopupSO _config;
        private readonly Dictionary<string, Popup> _popups;
        private readonly IObjectResolver _container;
        private Transform _popupParent;

        public PopupSystem(PopupSO config, IObjectResolver container)
        {
            _config = config;
            _container = container;
            _popups = new Dictionary<string, Popup>();
        }

        public void Initialize()
        {
            // create popup parent
            if (_config.canvasGroup != null)
            {
                _popupParent = Object.Instantiate(_config.canvasGroup).transform;
                Object.DontDestroyOnLoad(_popupParent.gameObject);
            }

            // reload popups
            Reload();
        }

        public void Reload()
        {
            foreach (var popupConfig in _config.popups)
            {
                if (!popupConfig.preload)
                {
                    continue;
                }

                if (_popups.ContainsKey(popupConfig.name))
                {
                    Debug.LogWarningFormat("Popup {0} already exists.", popupConfig.name);
                    continue;
                }

                // instantiate the popup prefab
                CreatePopup(popupConfig);
            }
        }

        private Popup CreatePopup(PopupSO.PopupConfiguration config)
        {
            if (_popups.ContainsKey(config.name))
            {
                Debug.LogWarningFormat("Popup {0} already exists.", config.name);
                return _popups[config.name];
            }

            // instantiate the popup prefab
            var popup = _container.Instantiate(config.prefab).GetComponent<Popup>();
            _popups.Add(config.name, popup);

            // Set the parent of the popup
            if (_popupParent != null)
            {
                popup.transform.SetParent(_popupParent, false);
            }

            return popup;
        }

        public void Show(string popupType, object dataInput = null)
        {
            // Check if the popup type exists in the dictionary
            if (!_popups.TryGetValue(popupType, out var popup))
            {
                var popupConfig = _config.popups.Find(p => p.name == popupType);
                if (popupConfig == null)
                {
                    Debug.LogWarningFormat("Popup {0} not found.", popupType);
                    return;
                }
                popup = CreatePopup(popupConfig);
            }

            // Show the popup with the provided data input
            popup.Show(dataInput);
        }

        public UniTask<object> ShowAsync(string popupType, object dataInput = null)
        {
            // Check if the popup type exists in the dictionary
            if (!_popups.TryGetValue(popupType, out var popup))
            {
                var popupConfig = _config.popups.Find(p => p.name == popupType);
                if (popupConfig == null)
                {
                    Debug.LogWarningFormat("Popup {0} not found.", popupType);
                    return UniTask.FromResult<object>(null);
                }
                popup = CreatePopup(popupConfig);
            }

            // Create a new UniTaskCompletionSource to handle the async operation
            var source = new UniTaskCompletionSource<object>();
            popup.Show(dataInput, source);
            return source.Task;
        }

        public void Hide(string popupType, object result = null)
        {
            if (!_popups.TryGetValue(popupType, out var popup))
                return;
            popup.Hide(result);
        }
    }
}
