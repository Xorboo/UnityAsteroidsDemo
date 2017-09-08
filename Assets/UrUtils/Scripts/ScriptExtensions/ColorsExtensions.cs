//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using System.Collections;

public static class ColorsExtensions
{
    public static Color FromColor(Color color)
    {
        return new Color(color.r, color.g, color.b, color.a);
    }

    public static Color FromColor(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Color ChangeAlpha(this Color color, float alpha)
    {
        return FromColor(color, alpha);
    }

    public static Color Copy(this Color color)
    {
        return FromColor(color);
    }

    public static string ToHexString(this Color color, bool addHashTag = false)
    {
        Color32 col = color;
        string hex = (addHashTag ? "#" : "") + col.r.ToString("X2") + col.g.ToString("X2") + col.b.ToString("X2");
        return hex;
    }

    public static Color HexToColor(string hex)
    {
        if (string.IsNullOrEmpty(hex))
            return Color.white;
        if (hex[0] == '#')
            hex = hex.Substring(1);

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = hex.Length == 8 ? byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;
        return new Color32(r, g, b, a);
    }
}