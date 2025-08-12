using Cysharp.Threading.Tasks;
using NUtilities.Loading;
using UnityEngine;
using VContainer;

public struct SceneContext
{
    public int level;
    public int gameMode;
}

public class STwoScript : MonoBehaviour
{
    private LoadingService _loadingService;

    [Inject]
    public void Inject(SceneContext sceneContext, LoadingService loadingService)
    {
        _loadingService = loadingService;
        Debug.Log(
            $"SceneContext: Level = {sceneContext.level}, GameMode = {sceneContext.gameMode}"
        );
        _loadingService.HideLoadingView();
    }

    public void ReplaceScene()
    {
        _loadingService.ReplaceScene("S1").Forget();
    }
}
