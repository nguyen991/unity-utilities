using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace TileGame.Level
{
    public class LevelSystem : IInitializable
    {
        public void Initialize()
        {
        }
        
        public async UniTask<LevelData> LoadLevel(int level)
        {
            var textAsset = await Addressables.LoadAssetAsync<TextAsset>($"Levels/Level_{level}.json");
            return JsonConvert.DeserializeObject<LevelData>(textAsset.text);
        }
    }
}