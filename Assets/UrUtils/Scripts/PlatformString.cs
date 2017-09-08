//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;


[Serializable]
public class PlatformString
{
#pragma warning disable 414
    [SerializeField, Tooltip("Will be returned if not empty")]
    string Override = "";

    [Header("Standalone")]
    [SerializeField]
    string Windows = "";
    [SerializeField]
    string MacOS = "";
    [SerializeField]
    string Linux = "";

    [Header("Mobile")]
    [SerializeField]
    string IOS = "";
    [SerializeField]
    string Android = "";

    [Header("Other")]
    [SerializeField]
    string WebGL = "";
    [SerializeField, Space(5), Tooltip("Returned for unknown platform")]
    string Default = "";
#pragma warning restore 414


    #region Constructors
    public PlatformString() { }
    public PlatformString(string defaultValue)
    {
        Default = defaultValue;
    }
    #endregion


    public string Value
    {
        get
        {
            if (!String.IsNullOrEmpty(Override))
                return Override;

#if UNITY_STANDALONE_WIN
            return Windows;
#elif UNITY_STANDALONE_OSX
            return MacOS;
#elif UNITY_STANDALONE_LINUX
            return Linux;
#elif UNITY_IOS
            return IOS;
#elif UNITY_ANDROID
            return Android;
#elif UNITY_WEBGL
            return WebGL;
#else
            return Default;
#endif
        }
    }


    public static implicit operator string(PlatformString platformString)
    {
        return platformString.Value;
    }
}
