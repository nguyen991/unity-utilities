using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUtilities.Helper.Addressable;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TileGame.Load
{
    public class GameLoader : MonoBehaviour
    {
        public List<string> downloadLabels;
        
        public TextMeshProUGUI progressText;
        
        public string nextSceneName = "Lobby";
        
        private async UniTaskVoid Start()
        {
            //TODO: load other plugins here

            // Download all assets
            await Downloader.Download(downloadLabels, (current, total) =>
            {
                if (progressText != null)
                {
                    var progress = (float)current / total;
                    progressText.text = $"Downloading: {progress:P0}%";
                }
            });
            
            // Replace the scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}