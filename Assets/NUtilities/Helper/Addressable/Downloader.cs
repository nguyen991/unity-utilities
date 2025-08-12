using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NUtilities.Helper.Addressable
{
    public class Downloader
    {
        public static async UniTask Download(
            IList<string> labels,
            Action<long, long> onProgress = null
        )
        {
            // get download size
            var downloadSize = await Addressables.GetDownloadSizeAsync(labels);
            if (downloadSize == 0)
            {
                Debug.Log("No download required, all assets are already downloaded.");
                return;
            }

            var op = Addressables.DownloadDependenciesAsync(labels, Addressables.MergeMode.Union);
            while (!op.IsDone)
            {
                // report progress
                long downloadedBytes = op.GetDownloadStatus().DownloadedBytes;
                onProgress?.Invoke(downloadedBytes, downloadSize);
                await UniTask.Yield();
            }

            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Download completed successfully");
            }
            else
            {
                Debug.LogError($"Download failed: {op.OperationException?.Message}");
            }

            // release the operation handle
            Addressables.Release(op);
        }
    }
}
