//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine.Events;


public static class UnityEventExtensions
{
    [Serializable] public class Empty : UnityEvent { }

    [Serializable] public class Int : UnityEvent<int> { }
    [Serializable] public class Bool : UnityEvent<bool> { }
    [Serializable] public class Float : UnityEvent<float> { }
    [Serializable] public class String : UnityEvent<string> { }
    
    [Serializable] public class Vector2 : UnityEvent<UnityEngine.Vector2> { }
    [Serializable] public class Vector3 : UnityEvent<UnityEngine.Vector3> { }
    [Serializable] public class GameObject : UnityEvent<UnityEngine.GameObject> { }
    [Serializable] public class Collider2D : UnityEvent<UnityEngine.Collider2D> { }
    [Serializable] public class Collider2DCollider2D : UnityEvent<UnityEngine.Collider2D, UnityEngine.Collider2D> { }

    [Serializable] public class IntInt : UnityEvent<int, int> { }
}
