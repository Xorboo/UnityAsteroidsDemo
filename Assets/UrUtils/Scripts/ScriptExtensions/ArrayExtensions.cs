//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;


public static class ArrayExtensions
{
    public static T Last<T>(this T[] array)
    {
        int n = array.Length;
        if (n > 0)
            return array[n - 1];
        return default(T);
    }

    public static T First<T>(this T[] array)
    {
        int n = array.Length;
        if (n > 0)
            return array[0];
        return default(T);
    }

    public static bool Contains<T>(this T[] array, T element)
    {
        return Array.IndexOf(array, element) != -1;
    }
}
