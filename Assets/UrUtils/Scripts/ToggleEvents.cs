//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleEvents : MonoBehaviour
{
    public UnityEvent OnToggleWasOn;
    public UnityEvent OnToggleWasOff;

    Toggle Toggle;


    void Awake()
    {
        Toggle = GetComponent<Toggle>();
        Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    void Start()
    {
        OnValueChanged(Toggle.isOn);
    }

    void OnDestroy()
    {
        Toggle.onValueChanged.RemoveListener(OnValueChanged);
    }

    void OnValueChanged(bool value)
    {
        if (value)
            OnToggleWasOn.Invoke();
        else
            OnToggleWasOff.Invoke();
    }

    public void SetToggle(bool value)
    {
        if (value != Toggle.isOn)
        {
            Toggle.isOn = value;
        }
    }
}