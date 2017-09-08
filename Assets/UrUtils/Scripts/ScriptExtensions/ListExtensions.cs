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


public static class ListExtensions
{
    public static bool Empty<T>(this List<T> list)
    {
        return list == null || 0 == list.Count;
    }

    public static T Random<T>(this List<T> list)
    {
        if (list.Empty())
            return default(T);
        return list[Rand.Range(0, list.Count)];
    }

    public static T First<T>(this List<T> list)
    {
        if (list.Empty())
            return default(T);
        return list[0];
    }

    public static T Last<T>(this List<T> list)
    {
        if (list.Empty())
            return default(T);
        return list[list.Count - 1];
    }

    public static bool RemoveFirst<T>(this List<T> list)
    {
        if (list.Empty())
            return false;
        list.RemoveAt(0);
        return true;
    }

    public static bool RemoveLast<T>(this List<T> list)
    {
        if (list.Empty())
            return false;
        list.RemoveAt(list.Count - 1);
        return true;
    }

    public static T Pop<T>(this List<T> list)
    {
        if (list.Empty())
            return default(T);

        T lastElement = list.Last();
        list.RemoveLast();
        return lastElement;
    }

    public static int N<T>(this List<T> list)
    {
        return list != null ? list.Count : 0;
    }
}
