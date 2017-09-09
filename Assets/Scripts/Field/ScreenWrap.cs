using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


public class ScreenWrap : MonoBehaviour
{
    [SerializeField]
    float ExtraPadding = 0f;

    Rect Borders;
    IVector2 Resolution;


    #region Behaviours
    void Awake()
    {
        // TODO Have to check for resolution changes (device rotation) and recalculate borders when it happens
        UpdateBorders();
    }

    void Update()
    {
        CheckScreenUpdate();
        CheckWrapping();
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            UpdateBorders();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(Borders.xMin, Borders.yMin), new Vector3(Borders.xMax, Borders.yMin));
        Gizmos.DrawLine(new Vector3(Borders.xMin, Borders.yMax), new Vector3(Borders.xMax, Borders.yMax));
        Gizmos.DrawLine(new Vector3(Borders.xMin, Borders.yMin), new Vector3(Borders.xMin, Borders.yMax));
        Gizmos.DrawLine(new Vector3(Borders.xMax, Borders.yMin), new Vector3(Borders.xMax, Borders.yMax));
    }
    #endregion


    public void SetPadding(float padding)
    {
        Assert.IsTrue(padding > 0f, "Wrapping object padding should be > 0");

        ExtraPadding = padding;
        UpdateBorders();
    }


    void CheckWrapping()
    {
        bool isOutside = false;
        Vector3 position = transform.position;

        if (position.x <= Borders.xMin)
        {
            position.x = Borders.xMax;
            isOutside = true;
        }
        else if (position.x >= Borders.xMax)
        {
            position.x = Borders.xMin;
            isOutside = true;
        }


        if (position.y <= Borders.yMin)
        {
            position.y = Borders.yMax;
            isOutside = true;
        }
        else if (position.y >= Borders.yMax)
        {
            position.y = Borders.yMin;
            isOutside = true;
        }

        if (isOutside)
        {
            transform.position = position;
        }
    }

    void CheckScreenUpdate()
    {
        var res = Screen.currentResolution;
        if (Screen.width != Resolution.x || Screen.height != Resolution.y)
            UpdateBorders();
    }

    void UpdateBorders()
    {
        Resolution = new IVector2(Screen.width, Screen.height);

        Vector3 padding = new Vector3(ExtraPadding, ExtraPadding, 0f);

        var camera = Camera.main;
        Vector3 min = camera.ViewportToWorldPoint(Vector3.zero) - padding;
        Vector3 max = camera.ViewportToWorldPoint(Vector3.one) + padding;
        Borders = new Rect(min, max - min);
    }
}
