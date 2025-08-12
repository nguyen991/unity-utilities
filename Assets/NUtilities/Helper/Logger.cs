using UnityEngine;

namespace NUtilities.Helper
{
    public static class Log
    {
        public static bool Enable = true;

        public static void D(Object context, string message, params object[] args)
        {
            if (!Enable)
                return;
            Debug.LogFormat(context, message, args);
        }

        public static void D(string message, params object[] args)
        {
            if (!Enable)
                return;
            Debug.LogFormat(message, args);
        }

        public static void E(Object context, string message, params object[] args)
        {
            if (!Enable)
                return;
            Debug.LogErrorFormat(context, message, args);
        }

        public static void E(string message, params object[] args)
        {
            if (!Enable)
                return;
            Debug.LogErrorFormat(message, args);
        }

        public static void W(Object context, string message, params object[] args)
        {
            if (!Enable)
                return;
            Debug.LogWarningFormat(context, message, args);
        }

        public static void W(string message, params object[] args)
        {
            if (!Enable)
                return;
            Debug.LogWarningFormat(message, args);
        }
    }
}
