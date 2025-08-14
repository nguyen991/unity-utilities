using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NUtilities.Save
{
    public class SaveSystem : IInitializable
    {
        private readonly IFileStorage _fileStorage;
        private readonly ISaveType _saveType;
        private readonly SaveSO _config;
        private readonly IObjectResolver _resolver;

        private Dictionary<string, object> _registeredModels;

        public SaveSystem(SaveSO config, IObjectResolver resolver)
        {
            _resolver = resolver;
            _registeredModels = new Dictionary<string, object>();
            _config = config;

            // Initialize file storage location
            switch (_config.saveLocation)
            {
                case SaveSO.Location.PlayerPrefs:
                    _fileStorage = new PlayerPrefsStorage();
                    break;
                case SaveSO.Location.PersistentDataPath:
                    _fileStorage = new PersistentStorage();
                    break;
            }

            // Initialize save type
            switch (_config.saveType)
            {
                case SaveSO.SaveType.Json:
                    _saveType = new SaveTypeJson();
                    break;
                case SaveSO.SaveType.Binary:
                    _saveType = new SaveTypeBinary();
                    break;
            }
        }

        public void Initialize()
        {
            var go = new GameObject("SaveSystem");
            var behaviour = go.AddComponent<SaveSystemBehaviour>();
            behaviour.onApplicationFovus = OnApplicationFocus;
            Object.DontDestroyOnLoad(go);
        }

        public void RegisterModel(object model, string fileName, string version = "")
        {
            if (model == null)
            {
                Debug.LogError("Model cannot be null");
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("File name cannot be null or empty");
                return;
            }

            // Append version if provided
            fileName += version;

            // Prefix the file name with the configured prefix and extension
            if (!fileName.EndsWith(_config.fileExtension))
            {
                fileName += _config.fileExtension;
            }

            // Check if the model is already registered
            if (_registeredModels.ContainsKey(fileName))
            {
                Debug.LogWarning($"Model with file name {fileName} is already registered.");
                return;
            }
            _registeredModels[fileName] = model;
        }

        public void Save(object data, string fileName, string version = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("File name cannot be null or empty");
                return;
            }
            if (data == null)
            {
                Debug.LogError("Data cannot be null");
                return;
            }

            // Prefix the file name with the configured prefix and extension
            if (!fileName.EndsWith(_config.fileExtension))
            {
                fileName += version + _config.fileExtension;
            }
            Debug.Log($"Saving data to {fileName}");

            // Serialize the data
            var content = _saveType.Serialize(
                data,
                _config.useEncryption ? _config.encryptionKey : null
            );
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"Failed to serialize data for {fileName}");
                return;
            }
            Debug.Log($"Saving data to {fileName}: {content}");

            // Save the serialized data to the file storage
            _fileStorage.Save(content, fileName);
        }

        public void SaveAll()
        {
            foreach (var model in _registeredModels)
            {
                Save(model.Value, model.Key);
            }
        }

        public void Load(object model, string fileName, string version = "")
        {
            if (model == null)
            {
                Debug.LogError("Model cannot be null");
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("File name cannot be null or empty");
                return;
            }

            // Prefix the file name with the configured prefix and extension
            if (!fileName.EndsWith(_config.fileExtension))
            {
                fileName += version + _config.fileExtension;
            }
            Debug.Log($"Loading data from {fileName}");

            // Get the content from file storage
            var content = _fileStorage.Get(fileName);
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogWarning($"No data found for {fileName}");
                return;
            }

            // Deserialize the model
            _saveType.Deserialize(
                content,
                model,
                _config.useEncryption ? _config.encryptionKey : null
            );
        }

        public void LoadAll()
        {
            foreach (var model in _registeredModels)
            {
                Load(model.Value, model.Key);
            }
        }

        public bool Delete(string fileName, string version = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("File name cannot be null or empty");
                return false;
            }

            // Prefix the file name with the configured prefix and extension
            if (!fileName.EndsWith(_config.fileExtension))
            {
                fileName += version + _config.fileExtension;
            }
            return _fileStorage.Delete(fileName);
        }

        public bool Exists(string fileName, string version = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("File name cannot be null or empty");
                return false;
            }
            // Prefix the file name with the configured prefix and extension
            if (!fileName.EndsWith(_config.fileExtension))
            {
                fileName += version + _config.fileExtension;
            }
            return _fileStorage.Exists(fileName);
        }

        private void OnApplicationFocus(bool value)
        {
            if (!value && _config.saveAllOnFocusLost)
            {
                SaveAll();
            }
        }
    }
}
