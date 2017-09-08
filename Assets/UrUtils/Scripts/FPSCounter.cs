//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    float TextUpdateRate = 0.5f;

    Text Text;
    float deltaTime = 0.0f;

    float NextUpdateTime = 0f;


    #region Behaviours
    void Awake()
    {
        Text = GetComponent<Text>();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        int fps = Mathf.FloorToInt(1.0f / deltaTime);

        NextUpdateTime -= Time.deltaTime;
        if (NextUpdateTime <= 0f)
        {
            NextUpdateTime = TextUpdateRate;
            Text.text = fps.ToString();
        }
    }
    #endregion
}
