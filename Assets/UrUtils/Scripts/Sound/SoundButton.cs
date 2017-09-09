//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;
using UnityEngine.UI;


public class SoundButton : MonoBehaviour
{
    public static Action<SoundButton> OnButtonClicked = delegate { };


    #region Behaviours
    void Awake()
    {
        var button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(ButtonClicked);
    }
    #endregion


    public void ButtonClicked()
    {
        OnButtonClicked(this);
    }
}
