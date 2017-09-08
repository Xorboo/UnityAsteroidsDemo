//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using System;
using System.Collections;


public class ScaleJumper : MonoBehaviour
{
    [SerializeField] float Duration = 0.2f;
    float HalfDuration;

    [SerializeField] float MaxScale = 1.2f;
    Vector3 MaxScaleVector;
    Vector3 DefaultScale = Vector3.one;

    [SerializeField] EaseType UpEase = EaseType.Linear;
    [SerializeField] EaseType DownEase = EaseType.Linear;
    Easer UpEaser;
    Easer DownEaser;

    IEnumerator Routine = null;
    Action FinishAction = null;


    public void DoScale(Action finishAction = null)
    {
        Clear();

        FinishAction = finishAction;
        Routine = ScaleRoutine(finishAction);
        StartCoroutine(Routine);
    }


    void Awake() 
    {
        HalfDuration = Duration / 2f;

        MaxScaleVector = VectorExtensions.FromValue(MaxScale);
        DefaultScale = transform.localScale;

        UpEaser = Ease.FromType(UpEase);
        DownEaser = Ease.FromType(DownEase);
    }

    void OnEnable()
    {
        Clear();
    }

    void OnDisable()
    {
        Clear();
    }

    void Clear()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
            transform.localScale = DefaultScale;

            if (FinishAction != null)
                FinishAction();
        }
    }

    IEnumerator ScaleRoutine(Action finishAction)
    {
        float elapsed = 0;

        var start = transform.localScale;
        var range = MaxScaleVector - start;
        while (elapsed < HalfDuration)
        {
            elapsed = Mathf.MoveTowards(elapsed, HalfDuration, Time.deltaTime);
            transform.localScale = start + range * UpEaser(elapsed / HalfDuration);
            yield return 0;
        }

        elapsed = 0;

        start = transform.localScale;
        range = DefaultScale - start;
        while (elapsed < HalfDuration)
        {
            elapsed = Mathf.MoveTowards(elapsed, HalfDuration, Time.deltaTime);
            transform.localScale = start + range * DownEaser(elapsed / HalfDuration);
            yield return 0;
        }
        
        transform.localScale = DefaultScale;

        if (finishAction != null)
            finishAction();
    }
}
