using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUtilities.Helper.Addressable;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace TileGame.Loader
{
    public class GameLoader : MonoBehaviour
    {
        public List<string> downloadLabels;
        
        public TextMeshProUGUI progressText;
        
        public string nextSceneName = "Lobby";

        private async UniTaskVoid Start()
        {
            // Download all assets
            if (downloadLabels.Count > 0)
            {
                await Downloader.Download(downloadLabels, (current, total) =>
                {
                    if (progressText != null)
                    {
                        var progress = (float)current / total;
                        progressText.text = $"Downloading: {progress:P0}%";
                    }
                });
            }
            
            //TODO: load other plugins here
            
            // next frame
            await UniTask.NextFrame();

            // Replace the scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}