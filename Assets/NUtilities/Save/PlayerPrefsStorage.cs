using UnityEngine;

namespace NUtilities.Save
{
    public class PlayerPrefsStorage : IFileStorage
    {
        public bool Delete(string fileName)
        {
            if (!Exists(fileName))
                return false;
            PlayerPrefs.DeleteKey(fileName);
            return true;
        }

        public bool Exists(string fileName)
        {
            return PlayerPrefs.HasKey(fileName);
        }

        public string Get(string fileName)
        {
            return PlayerPrefs.GetString(fileName, string.Empty);
        }

        public void Save(string data, string fileName)
        {
            PlayerPrefs.SetString(fileName, data);
        }
    }
}
