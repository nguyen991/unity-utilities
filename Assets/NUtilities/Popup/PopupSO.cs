using System.Collections.Generic;
using UnityEngine;

namespace NUtilities.Popup
{
    [CreateAssetMenu(fileName = "PopupSO", menuName = "NUtilities/Create/Popup Data")]
    public class PopupSO : ScriptableObject
    {
        [System.Serializable]
        public class PopupConfiguration
        {
            public string name;
            public GameObject prefab;
            public bool preload;
        }

        public GameObject canvasGroup;
        public List<PopupConfiguration> popups;
    }
}
