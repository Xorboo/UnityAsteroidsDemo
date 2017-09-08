//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;


public static class StringExtensions
{
    const string TimeFormatString = "{0:D2}:{1:D2}.{2:D2}";

    public static string TimeToString(float seconds)
    {
        var milliseconds = Mathf.FloorToInt(1000.0f * seconds);
        return TimeToString(milliseconds);
    }

    public static string TimeToString(int milliseconds)
    {
        var seconds = milliseconds / 1000;
        milliseconds %= 1000;
        milliseconds /= 10;
        var min = seconds / 60;
        seconds %= 60;
        return string.Format(TimeFormatString, min, seconds, milliseconds);
    }
}