using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUtilities.Helper.Addressable;
using NUtilities.Loading;
using NUtilities.Popup;
using NUtilities.Save;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

public class SOneScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _downloadText;

    private PopupService _popupService;
    private LoadingService _loadingService;
    private SaveService _saveSystem;

    [Inject]
    public void Inject(
        PopupService popupService,
        LoadingService loadingService,
        SaveService saveSystem
    )
    {
        _popupService = popupService;
        _loadingService = loadingService;
        _loadingService.Hide();
        _saveSystem = saveSystem;
    }

    public void Click()
    {
        _popupService.Show("base");
    }

    public void ReplaceScene()
    {
        _loadingService
            .ReplaceScene("S2", new SceneContext() { level = 10, gameMode = 2 })
            .Forget();
    }

    private async UniTaskVoid Start()
    {
        // save data
        // var data = new SaveData() { level = 10, gameMode = 20 };
        // _saveSystem.Save(data, "test");

        // load data
        var data = new SaveData();
        _saveSystem.RegisterModel(data, "test");
        _saveSystem.LoadAll();

        Debug.Log(
            $"Loaded Data: Level = {data.level}, Score = {data.score}, GameMode = {data.gameMode}"
        );

        data.level += 12;

        // download assets
        await Downloader.Download(
            new List<string> { "SD" },
            (downloadedBytes, totalBytes) =>
            {
                _downloadText.text = $"Downloading : {downloadedBytes}/{totalBytes} bytes";
            }
        );

        // load background sprite
        var sprite = await Addressables.LoadAssetAsync<Sprite>("background");
        var bkgr = new GameObject("Background");
        bkgr.transform.localScale = Vector3.one * 0.35f;
        var spriteRenderer = bkgr.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }

    void OnApplicationFocus(bool focus) { }
}

public class SaveData
{
    public int level;
    public int score;
    public int gameMode;
}
