using UnityEngine;

namespace NUtilities.Helper.Device
{
    public class Device
    {
        public static float GetScreenWidth()
        {
            return Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        }

        public static float GetScreenHeight()
        {
            return Camera.main.orthographicSize * 2f;
        }
    }
}
