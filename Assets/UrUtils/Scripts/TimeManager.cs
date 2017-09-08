//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;


public class TimeManager
{
    static int PauseCount = 0;

    public static float TimeFactor
    {
        get { return IsPaused ? 0f : _TimeFactor; }
        set { _TimeFactor = value; }
    }
    static float _TimeFactor = 1f;


    public static float DeltaTime
    {
        get { return PauseCount == 0 ? Time.deltaTime * TimeFactor : 0f; }
    }
    public static float FixedDeltaTime
    {
        get { return PauseCount == 0 ? Time.fixedDeltaTime * TimeFactor : 0f; }
    }


    #region Pausing
    public static bool IsPaused
    {
        get { return PauseCount > 0; }
    }

    public static void Pause()
    {
        PauseCount++;
        //Debug.LogError("PAUSE: " + PauseCount);
    }

    public static void Resume()
    {
        PauseCount--;
        //Debug.LogError("RESUME: " + PauseCount);

        if (PauseCount < 0)
        {
            Debug.LogError("Extra unpause, something is wrong?");
            PauseCount = 0;
        }

    }
    #endregion
}
