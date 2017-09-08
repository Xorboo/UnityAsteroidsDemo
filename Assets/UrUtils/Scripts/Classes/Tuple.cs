//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class Tuple<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }

    public Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}
