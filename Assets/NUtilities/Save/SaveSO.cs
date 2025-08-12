using UnityEngine;

namespace NUtilities.Save
{
    [CreateAssetMenu(fileName = "SaveSO", menuName = "NUtilities/Create/Save Data")]
    public class SaveSO : ScriptableObject
    {
        public enum Location
        {
            PlayerPrefs,
            PersistentDataPath,
        }

        public enum SaveType
        {
            Binary,
            Json,
        }

        public Location saveLocation = Location.PersistentDataPath;
        public SaveType saveType = SaveType.Json;
        public bool useEncryption = false;
        public string encryptionKey = "defaultKey";
        public string fileExtension = ".sav";
        public bool saveAllOnFocusLost = false;
    }
}
