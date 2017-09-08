//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public static class ScrollRectMotionExtensions
{
    public static IEnumerator ScrollTo(this ScrollRect scrollRect, Vector2 target, float duration, Easer ease, Action finishDelegate = null)
    {
        float elapsed = 0;
        var start = scrollRect.normalizedPosition;
        var range = target - start;
        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            scrollRect.normalizedPosition = start + range * ease(elapsed / duration);
            yield return 0;
        }
        scrollRect.normalizedPosition = target;

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator ScrollTo(this ScrollRect scrollRect, Vector2 target, float duration, Action finishDelegate = null)
    {
        return ScrollTo(scrollRect, target, duration, Ease.Linear, finishDelegate);
    }
    public static IEnumerator ScrollTo(this ScrollRect scrollRect, Vector2 target, float duration, EaseType ease = EaseType.Linear, Action finishDelegate = null)
    {
        return ScrollTo(scrollRect, target, duration, Ease.FromType(ease), finishDelegate);
    }
}
