//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;

public class ScreenEvents : MonoBehaviour
{
    public UnityEventExtensions.IntInt OnScreenSizeChanged;


    int CurrentWidth, CurrentHeight;


    #region Behaviours
    void Awake()
    {
        UpdateScreenSize();
    }
    void Update()
    {
        if (CurrentWidth != Screen.width || CurrentHeight != Screen.height)
        {
            UpdateScreenSize();
            OnScreenSizeChanged.Invoke(CurrentWidth, CurrentHeight);
        }
    }
    #endregion


    void UpdateScreenSize()
    {
        CurrentWidth = Screen.width;
        CurrentHeight = Screen.height;
    }
}
