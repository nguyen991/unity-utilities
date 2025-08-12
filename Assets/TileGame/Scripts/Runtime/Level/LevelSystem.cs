using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TileGame.Level
{
    public class LevelSystem
    {
        public async UniTask<LevelData> LoadLevel(int level)
        {
            var textAsset = await Addressables.LoadAssetAsync<TextAsset>($"Levels/Level_{level}");
            return JsonConvert.DeserializeObject<LevelData>(textAsset.text);
        }
    }
}