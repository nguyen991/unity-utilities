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
    private LoadingSystem _loadingSystem;

    [Inject]
    public void Inject(SceneContext sceneContext, LoadingSystem loadingSystem)
    {
        _loadingSystem = loadingSystem;
        Debug.Log(
            $"SceneContext: Level = {sceneContext.level}, GameMode = {sceneContext.gameMode}"
        );
        _loadingSystem.Hide();
    }

    public void ReplaceScene()
    {
        _loadingSystem.ReplaceScene("S1").Forget();
    }
}
