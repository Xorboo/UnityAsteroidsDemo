﻿//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using UnityEngine.Events;

public class ObjectFromPool : MonoBehaviour
{
    public UnityEventExtensions.GameObject OnRestart;
    public UnityEventExtensions.GameObject OnRecycle;
}
