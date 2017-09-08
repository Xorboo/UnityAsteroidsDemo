//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;


// Selects a random index from a array of items with their chances
public class ChanceChooser<T>
{
    int Count = 0;
    int MaxCount = 0;

    T[] Elements;
    float[] Chances;

    public float TotalChance { get; private set; }

    public ChanceChooser(int maxCount)
    {
        MaxCount = maxCount;
        TotalChance = 0;
        InitArrays();
    }

    public ChanceChooser(ChancePair<T>[] chances)
    {
        MaxCount = chances.Length;
        InitArrays();
        foreach (var chance in chances)
        {
            AddChance(chance.Element, chance.Chance);
        }
    }

    void InitArrays()
    {
        Elements = new T[MaxCount];
        Chances = new float[MaxCount];
    }

    public void AddChance(T item, float probability)
    {
        if (Count >= MaxCount)
        {
            Debug.LogError("Can't add another chance, count over " + MaxCount);
            return;
        }

        TotalChance += probability;

        Elements[Count] = item;
        Chances[Count] = TotalChance;
        Count++;
    }

    public T GetRandom()
    {
        float r = Rand.Range(0f, TotalChance);
        for (int i = 0; i < Count; i++)
        {
            if (Chances[i] >= r)
            {
                return Elements[i];
            }
        }

        Debug.LogError("Couldn't get random element, you mad bro? " + Count);
        return default(T);
    }
}

// Inherit from this class with required type to be able to set the values from Unity Editor, then initialize ChanceChooser with the array of this items
public class ChancePair<T>
{
    public T Element;
    public float Chance;
}