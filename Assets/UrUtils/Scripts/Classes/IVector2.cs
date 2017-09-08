//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;


[Serializable]
public struct IVector2
{
    public int x, y;

    public IVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }

    public static IVector2 operator +(IVector2 a, IVector2 b) { return new IVector2(a.x + b.x, a.y + b.y); }
    public static IVector2 operator -(IVector2 a) { return new IVector2(-a.x, -a.y); }
    public static IVector2 operator -(IVector2 a, IVector2 b) { return new IVector2(a.x - b.x, a.y - b.y); }
    public static IVector2 operator *(int d, IVector2 a) { return new IVector2(a.x * d, a.y * d); }
    public static IVector2 operator *(IVector2 a, int d) { return new IVector2(a.x * d, a.y * d); }
    public static IVector2 operator /(IVector2 a, int d) { return new IVector2(a.x / d, a.y / d); }
    public static bool operator ==(IVector2 lhs, IVector2 rhs) { return lhs.x == rhs.x && lhs.y == rhs.y; }
    public static bool operator !=(IVector2 lhs, IVector2 rhs) { return lhs.x != rhs.x || lhs.y != rhs.y; }

    public override bool Equals(object obj)
    {
        var other = obj as IVector2?;
        //not sure if this is right..
        if (other == null) return false;
        if (!other.HasValue) return false;
        return this == other;
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }

    public static implicit operator Vector2(IVector2 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static implicit operator IVector2(Vector2 v)
    {
        return new IVector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }
}