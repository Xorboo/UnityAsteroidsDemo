//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;


public static class VectorExtensions
{
	public static Vector3 Clone(this Vector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static Vector3 FromValue(float value)
    {
        return new Vector3(value, value, value);
    }

    public static void Set(ref Vector3 vector, float x, float y, float z)
    {
        vector.x = x;
        vector.y = y;
        vector.z = z;
    }

    public static void Add(ref Vector3 vector, float x, float y, float z)
    {
        vector.x += x;
        vector.y += y;
        vector.z += z;
    }

    public static Vector2 XY(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 YZ(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    public static float Distance2DXZ(Vector3 a, Vector3 b)
    {
        return new Vector2(a.x - b.x, a.z - b.z).magnitude;
    }

    public static float SqrDistance2DXZ(Vector3 a, Vector3 b)
    {
        return new Vector2(a.x - b.x, a.z - b.z).sqrMagnitude;
    }
    

    // Returns angle in -180..180
    public static float SignedAngle(Vector2 a, Vector2 b)
    {
        float angle = Vector2.Angle(a, b);
        Vector3 cross = Vector3.Cross(a, b);
        float sign = cross.z == 0 ? 1 : -Mathf.Sign(cross.z);
        return angle * sign;
    }

    // Returns angle in 0..360
    public static float CircleAngle(Vector2 a, Vector2 b)
    {
        float angle = SignedAngle(a, b);
        if (angle < 0)
            angle += 360f;
        return angle;
    }
}
