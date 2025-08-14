using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NUtilities.Save
{
    public class SaveSystemBehaviour : MonoBehaviour
    {
        public UnityAction<bool> onApplicationFovus;

        void OnApplicationFocus(bool focus)
        {
            onApplicationFovus?.Invoke(focus);
        }
    }
}
