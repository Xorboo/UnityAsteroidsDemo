//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ImageMotionExtensions
{
    public static IEnumerator FillTo(this Image image, float value, float duration, Easer ease, Action finishDelegate = null, bool waitOneFrame = true)
    {
        if (waitOneFrame)
            yield return 0;

        float elapsed = 0;
        var start = image.fillAmount;
        var range = value - start;

        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            image.fillAmount = start + range * ease(elapsed / duration);
            yield return 0;
        }
        image.fillAmount = value;

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator FillTo(this Image image, float value, float duration, EaseType ease = EaseType.Linear, Action finishDelegate = null, bool waitOneFrame = true)
    {
        return FillTo(image, value, duration, Ease.FromType(ease), finishDelegate, waitOneFrame);
    }
}
