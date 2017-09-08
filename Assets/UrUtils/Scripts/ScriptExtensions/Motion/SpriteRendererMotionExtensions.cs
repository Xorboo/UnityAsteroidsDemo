//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using UnityEngine;
using System.Collections;


public static class SpriteRendererMotionExtensions
{
    public static IEnumerator OpacityTo(this SpriteRenderer renderer, float value, float duration, Easer ease, Action finishDelegate = null)
    {
        float elapsed = 0;
        var start = renderer.color.a;
        var range = value - start;
        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, start + range * ease(elapsed / duration));
            yield return 0;
        }
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, value);

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator OpacityTo(this SpriteRenderer renderer, float value, float duration, EaseType ease = EaseType.Linear, Action finishDelegate = null)
    {
        return OpacityTo(renderer, value, duration, Ease.FromType(ease), finishDelegate);
    }


    public static IEnumerator ColorTo(this SpriteRenderer renderer, Color color, float duration, Easer ease, Action finishDelegate = null)
    {
        float elapsed = 0;
        var start = renderer.color;
        var range = color - start;
        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            renderer.color = start + range * ease(elapsed / duration);
            yield return 0;
        }
        renderer.color = color;

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator ColorTo(this SpriteRenderer renderer, Color color, float duration, EaseType ease = EaseType.Linear, Action finishDelegate = null)
    {
        return ColorTo(renderer, color, duration, Ease.FromType(ease), finishDelegate);
    }
}
