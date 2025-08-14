using System.Collections.Generic;
using NUtilities.Popup;
using UnityEngine;
using UnityEngine.Serialization;

namespace NUtilities.Pool
{
    [CreateAssetMenu(menuName = "NUtilities/Create/Pool Data", fileName = "PoolSO")]
    public class PoolSO : ScriptableObject
    {
        [System.Serializable]
        public class PoolItem
        {
            public GameObject prefab;
            public int initialSize;
        }
        
        public int defaultSize = 10;
        public List<PoolItem> items;
    }
}