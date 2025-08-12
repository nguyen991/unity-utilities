using UnityEngine;
using VContainer;

namespace NUtilities.Save
{
    public class SaveSystemBehaviour : MonoBehaviour
    {
        private SaveService _saveSystem;

        [Inject]
        public void Inject(SaveService saveSystem)
        {
            _saveSystem = saveSystem;
        }

        void OnApplicationFocus(bool focus)
        {
            _saveSystem.OnApplicationFocus(focus);
        }
    }
}
