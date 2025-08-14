using System.Collections.Generic;
using NUtilities.Popup;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;

namespace NUtilities.Pool
{
    public class PoolSystem : IInitializable
    {
        private readonly PoolSO _config;
        private Transform _parent;
        private Dictionary<string, IObjectPool<GameObject>> _pools;
        
        public PoolSystem(PoolSO config)
        {
            _config = config;
            _pools = new Dictionary<string, IObjectPool<GameObject>>();
        }
        
        public void Initialize()
        {
            // create pool parent
            var go = new GameObject("PoolParent");
            Object.DontDestroyOnLoad(go);
            _parent = go.transform;
            
            // reload pools
            _config.items.ForEach(item => CreatePool(item.prefab, item.initialSize > 0 ? item.initialSize : _config.defaultSize));
        }

        private ObjectPool<GameObject> CreatePool(GameObject prefab, int initialSize)
        {
            if (_pools.ContainsKey(prefab.name))
            {
                Debug.LogWarningFormat("Pool for {0} already exists.", prefab.name);
                return null;
            }

            // create pool for the prefab
            var pool = new ObjectPool<GameObject>(
                () =>
                {
                    var go = Object.Instantiate(prefab, _parent);
                    go.AddComponent<PoolObject>();
                    return go;
                },
                go => go.SetActive(true),
                go =>
                {
                    go.SetActive(false);
                    go.transform.SetParent(_parent);
                },
                Object.Destroy,
                true,
                initialSize
            );

            Debug.Log("Created pool for " + prefab.name + " with initial size " + initialSize);
            _pools[prefab.name] = pool;
            return pool;
        }

        public GameObject Get(string name, Transform parent = null, Vector3? position = null,
            Quaternion? rotation = null)
        {
            if (!_pools.TryGetValue(name, out var pool))
            {
                Debug.LogErrorFormat("Pool for {0} does not exist.", name);
                return null;
            }

            var instance = pool.Get();
            instance.GetComponent<PoolObject>().pool = pool;
            instance.transform.SetParent(parent ?? _parent, false);
            if (position.HasValue)
            {
                instance.transform.position = position.Value;
            }
            if (rotation.HasValue)
            {
                instance.transform.rotation = rotation.Value;
            }
            return instance;
        }

        public GameObject Get(GameObject prefab, Transform parent = null, Vector3? position = null, Quaternion? rotation = null)
        {
            if (prefab == null)
            {
                Debug.LogError("Cannot get a null prefab from the pool.");
                return null;
            }

            // Create the pool if it doesn't exist
            if (!_pools.ContainsKey(prefab.name))
            {
                CreatePool(prefab, _config.defaultSize);
            }

            return Get(prefab.name, parent, position, rotation);
        }

        public void Release(GameObject instance)
        {
            if (instance == null)
            {
                Debug.LogError("Cannot release a null instance to the pool.");
                return;
            }
            instance.GetComponent<PoolObject>()?.Release();
        }
    }
}