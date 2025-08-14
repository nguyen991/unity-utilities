using UnityEngine;
using UnityEngine.Pool;

namespace NUtilities.Pool
{
    public class PoolObject : MonoBehaviour
    {
        public IObjectPool<GameObject> pool;
        
        public void Release()
        {
            pool.Release(gameObject);
        }
    }
}