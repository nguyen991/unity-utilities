using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace NUtilities.Loading
{
    public class LoadingService : IInitializable
    {
        private readonly GameObject _prefab;
        private LoadingView _view;

        public LoadingService(GameObject prefab)
        {
            _prefab = prefab;
        }

        public void Initialize()
        {
            // Instantiate the loading view prefab
            _view = Object.Instantiate(_prefab).GetComponent<LoadingView>();
            if (_view == null)
            {
                Debug.LogError("LoadingView component not found on the prefab.");
                return;
            }
            Object.DontDestroyOnLoad(_view.gameObject);
        }

        public async UniTask ReplaceScene(string sceneName, params object[] contexts)
        {
            using (
                LifetimeScope.Enqueue(builder =>
                {
                    foreach (var context in contexts)
                    {
                        builder.RegisterInstance(context).AsSelf();
                    }
                })
            )
            {
                // Initialize the loading view
                _view.StartLoading();

                // Load the scene asynchronously
                var operation = SceneManager.LoadSceneAsync(sceneName);
                operation.allowSceneActivation = false;

                // Wait for the scene to be fully loaded
                while (!operation.isDone)
                {
                    _view.OnProgress(operation.progress);

                    // Scene is loaded, now we can activate it
                    if (operation.progress >= 0.9f)
                    {
                        operation.allowSceneActivation = true;
                    }
                    await UniTask.Yield();
                }

                // Scene is now active, complete the loading view
                _view.OnComplete().Forget();
            }
        }

        public void HideLoadingView()
        {
            _view.allowHide = true;
        }
    }
}
