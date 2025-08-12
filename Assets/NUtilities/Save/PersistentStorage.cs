using UnityEngine;

namespace NUtilities.Save
{
    public class PersistentStorage : IFileStorage
    {
        public bool Delete(string fileName)
        {
            if (!Exists(fileName))
                return false;

            var path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            try
            {
                System.IO.File.Delete(path);
                Debug.Log($"File deleted: {path}");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to delete file {path}: {ex.Message}");
                return false;
            }
        }

        public bool Exists(string fileName)
        {
            var path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            return System.IO.File.Exists(path);
        }

        public string Get(string fileName)
        {
            var path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllText(path);
            }
            Debug.LogWarning($"File not found: {path}");
            return string.Empty;
        }

        public void Save(string data, string fileName)
        {
            var path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            System.IO.File.WriteAllText(path, data);
            Debug.Log($"Data saved to {path}");
        }
    }
}
