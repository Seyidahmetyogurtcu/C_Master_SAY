using UnityEngine;

namespace Count_Master_SAY.Control
{
    public static class Vibrator
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaObject unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaObject unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif

        /// <summary>
        /// Vibrates 100 miliseconds default
        /// </summary>
        /// <param name="milliseconds"></param>
        public static void Vibrate(long milliseconds = 100)
        {
            if (IsAndroid())
            {
                vibrator.Call("vibrate", milliseconds);
                
            }
            else
            {
                Handheld.Vibrate();
            }
        }


        public static void Cancel()
        {
            if (IsAndroid())
            {
                vibrator.Call("cancel");
            }
        }

        public static bool IsAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}

