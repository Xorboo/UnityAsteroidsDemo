//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using UnityEngine;
using Rand = UnityEngine.Random;

public static class TransformMotionExtensions
{
    public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Easer ease, Action finishDelegate = null, bool useDeltaTime = true)
    {
        float elapsed = 0;
        var start = transform.localPosition;
        var range = target - start;
        while (elapsed < duration)
        {
            float delta = useDeltaTime ? Time.deltaTime : MotionExtensions.FrameTime;
            elapsed = Mathf.MoveTowards(elapsed, duration, delta);
            transform.localPosition = start + range * ease(elapsed / duration);
            yield return null;
        }

        if (transform != null)
            transform.localPosition = target;

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Action finishDelegate = null, bool useDeltaTime = true)
    {
        return MoveTo(transform, target, duration, Ease.Linear, finishDelegate, useDeltaTime);
    }
    public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear, Action finishDelegate = null, bool useDeltaTime = true)
    {
        return MoveTo(transform, target, duration, Ease.FromType(ease), finishDelegate, useDeltaTime);
    }

    public static IEnumerator MoveToGlobal(this Transform transform, Vector3 target, float duration, Easer ease, Action finishDelegate = null, bool useDeltaTime = true)
    {
        float elapsed = 0;
        var start = transform.position;
        var range = target - start;
        while (elapsed < duration)
        {
            float delta = useDeltaTime ? Time.deltaTime : MotionExtensions.FrameTime;
            elapsed = Mathf.MoveTowards(elapsed, duration, delta);
            transform.position = start + range * ease(elapsed / duration);
            yield return null;
        }

        if (transform != null)
            transform.position = target;

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator MoveToGlobal(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear, Action finishDelegate = null, bool useDeltaTime = true)
    {
        return MoveToGlobal(transform, target, duration, Ease.FromType(ease), finishDelegate, useDeltaTime);
    }

    public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Easer easeX, Easer easeY, Easer easeZ, Action finishDelegate = null, bool useDeltaTime = true)
    {
        float elapsed = 0;
        var start = transform.localPosition;
        var range = target - start;
        while (elapsed < duration)
        {
            float delta = useDeltaTime ? Time.deltaTime : MotionExtensions.FrameTime;
            elapsed = Mathf.MoveTowards(elapsed, duration, delta);
            float t = elapsed / duration;
            transform.localPosition = start + new Vector3(range.x * easeX(t), range.y * easeY(t), range.y * easeZ(t));
            yield return 0;
        }

        if (transform != null)
            transform.localPosition = target;

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, EaseType easeX, EaseType easeY, EaseType easeZ, Action finishDelegate = null, bool useDeltaTime = true)
    {
        return MoveTo(transform, target, duration, Ease.FromType(easeX), Ease.FromType(easeY), Ease.FromType(easeZ), finishDelegate, useDeltaTime);
    }

    public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, Easer ease, Action finishDelegate = null, bool useDeltaTime = true)
    {
        var start = transform.localPosition;
        transform.localPosition = target;
        return MoveTo(transform, start, duration, ease, finishDelegate, useDeltaTime);
    }
    public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, Action finishDelegate = null, bool useDeltaTime = true)
    {
        return MoveFrom(transform, target, duration, Ease.Linear, finishDelegate, useDeltaTime);
    }
    public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, EaseType ease, Action finishDelegate = null, bool useDeltaTime = true)
    {
        return MoveFrom(transform, target, duration, Ease.FromType(ease), finishDelegate, useDeltaTime);
    }

    public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, Easer ease, Action finishDelegate = null)
    {
        float elapsed = 0;
        var start = transform.localScale;
        var range = target - start;
        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            transform.localScale = start + range * ease(elapsed / duration);
            yield return 0;
        }

        if (transform != null) // Should it be after delegate for some reason?
            transform.localScale = target;

        if (finishDelegate != null)
        {
            finishDelegate();
        }
    }
    public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, Action finishDelegate = null)
    {
        return ScaleTo(transform, target, duration, Ease.Linear, finishDelegate);
    }
    public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, EaseType ease, Action finishDelegate = null)
    {
        return ScaleTo(transform, target, duration, Ease.FromType(ease), finishDelegate);
    }

    public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, Easer ease)
    {
        var start = transform.localScale;
        transform.localScale = target;
        return ScaleTo(transform, start, duration, ease);
    }
    public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration)
    {
        return ScaleFrom(transform, target, duration, Ease.Linear);
    }
    public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, EaseType ease)
    {
        return ScaleFrom(transform, target, duration, Ease.FromType(ease));
    }

    public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, Easer ease)
    {
        float elapsed = 0;
        var start = transform.localRotation;
        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(start, target, ease(elapsed / duration));
            yield return 0;
        }
        transform.localRotation = target;
    }
    public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, EaseType ease = EaseType.Linear)
    {
        return RotateTo(transform, target, duration, Ease.FromType(ease));
    }

    public static IEnumerator RotateTo(this Transform transform, Vector3 target, float duration, Easer ease)
    {
        float elapsed = 0;
        var start = transform.rotation.eulerAngles;
        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(start, target, ease(elapsed / duration)));
            yield return 0;
        }
        transform.localRotation = Quaternion.Euler(target);
    }
    public static IEnumerator RotateTo(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear)
    {
        return RotateTo(transform, target, duration, Ease.FromType(ease));
    }

    public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, Easer ease)
    {
        var start = transform.localRotation;
        transform.localRotation = target;
        return RotateTo(transform, start, duration, ease);
    }
    public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration)
    {
        return RotateFrom(transform, target, duration, Ease.Linear);
    }
    public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, EaseType ease)
    {
        return RotateFrom(transform, target, duration, Ease.FromType(ease));
    }

    public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, Easer ease)
    {
        float elapsed = 0;
        var start = transform.localPosition;
        Vector3 position;
        float t;
        while (elapsed < duration)
        {
            elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
            t = ease(elapsed / duration);
            position.x = start.x * (1 - t) * (1 - t) + control.x * 2 * (1 - t) * t + target.x * t * t;
            position.y = start.y * (1 - t) * (1 - t) + control.y * 2 * (1 - t) * t + target.y * t * t;
            position.z = start.z * (1 - t) * (1 - t) + control.z * 2 * (1 - t) * t + target.z * t * t;
            transform.localPosition = position;
            yield return 0;
        }
        transform.localPosition = target;
    }
    public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration)
    {
        return CurveTo(transform, control, target, duration, Ease.Linear);
    }
    public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, EaseType ease)
    {
        return CurveTo(transform, control, target, duration, Ease.FromType(ease));
    }

    public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, Easer ease)
    {
        var target = transform.localPosition;
        transform.localPosition = start;
        return CurveTo(transform, control, target, duration, ease);
    }
    public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration)
    {
        return CurveFrom(transform, control, start, duration, Ease.Linear);
    }
    public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, EaseType ease)
    {
        return CurveFrom(transform, control, start, duration, Ease.FromType(ease));
    }

    public static IEnumerator Shake(this Transform transform, Vector3 amount, float duration)
    {
        var start = transform.localPosition;
        var shake = Vector3.zero;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            shake.Set(Rand.Range(-amount.x, amount.x), Rand.Range(-amount.y, amount.y), Rand.Range(-amount.z, amount.z));
            transform.localPosition = start + shake;
            yield return 0;
        }
        transform.localPosition = start;
    }
    public static IEnumerator Shake(this Transform transform, float amount, float duration)
    {
        return Shake(transform, new Vector3(amount, amount, amount), duration);
    }
}
