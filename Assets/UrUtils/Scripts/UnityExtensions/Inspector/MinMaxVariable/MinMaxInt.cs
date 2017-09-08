//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;
using Rand = UnityEngine.Random;


[Serializable]
public class MinMaxInt
{
    public int Min;
    public int Max;


    #region Constructors
    public MinMaxInt() { }

    public MinMaxInt(int min, int max)
    {
        Min = min;
        Max = max;
    }
#endregion


    public int Random()
    {
        return Rand.Range(Min, Max + 1);
    }

    public int Delta()
    {
        return Max - Min;
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
