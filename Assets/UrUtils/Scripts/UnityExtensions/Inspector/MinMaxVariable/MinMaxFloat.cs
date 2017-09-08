//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


[Serializable]
public class MinMaxFloat
{
    public float Min;
    public float Max;


    #region Constructors
    public MinMaxFloat() { }

    public MinMaxFloat(float min, float max)
    {
        Min = min;
        Max = max;

        Assert.IsTrue(Max >= Min, "Max should be >= than Min");
    }
    #endregion


    public float Random()
    {
        Assert.IsTrue(Max >= Min, "Max should be >= than Min");
        return Rand.Range(Min, Max + 1);
    }

    public float GetTByValue(float value, bool clamp = true)
    {
        float t = (value - Min) / (Max - Min);
        if (clamp)
            t = Mathf.Clamp01(t);
        return t;
    }

    public float GetValueByT(float t, bool clamp = true)
    {
        if (clamp)
            t = Mathf.Clamp01(t);
        return Min + (Max - Min) * t;
    }
}