using UnityEngine;


public static class ApplicationExtensions
{
    public static RuntimePlatform Platform
    {
        get
        {
#if UNITY_ANDROID
            return RuntimePlatform.Android;
#elif UNITY_IOS
            return RuntimePlatform.IPhonePlayer;
#elif UNITY_STANDALONE_OSX
            return RuntimePlatform.OSXPlayer;
#elif UNITY_STANDALONE_WIN
            return RuntimePlatform.WindowsPlayer;
#elif UNITY_STANDALONE_LINUX
            return RuntimePlatform.LinuxPlayer;
#elif UNITY_WEBGL
            return RuntimePlatform.WebGLPlayer;
#else
            Debug.LogWarningFormat("ApplicationExtensions: Unknown RuntimePlatform: {0}", Application.platform);
            return RuntimePlatform.WindowsPlayer;
#endif
        }
    }
}
