//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FixedCameraSize : MonoBehaviour
{
    [SerializeField, Tooltip("Screen width in units")]
    float _Size = 5f;

    public float Size
    {
        get { return _Size; }
        set
        {
            if (_Size == value)
                return;
            _Size = value;
            UpdateCameraSize();
        }
    }

    [SerializeField, Tooltip("Camera type")]
    Type _EdgeType = Type.Height;
    public Type EdgeType
    {
        get { return _EdgeType; }
        set
        {
            if (_EdgeType == value)
                return;
            _EdgeType = value;
            UpdateCameraSize();
        }
    }

    public enum Type
    {
        Width, Height, LongestEdge, ShortestEdge
    };

    Camera Camera;


    #region Behaviours
    void Awake()
    {
        Camera = GetComponent<Camera>();
        UpdateCameraSize();
    }
    #endregion


    void UpdateCameraSize()
    {
        if (Camera == null)
            return;

        switch (EdgeType)
        {
            case Type.Width:
                SetWidth();
                break;

            case Type.Height:
                SetHeight();
                break;

            case Type.LongestEdge:
                if (Camera.aspect > 1)
                    SetWidth();
                else
                    SetHeight();
                break;

            case Type.ShortestEdge:
                if (Camera.aspect > 1)
                    SetHeight();
                else
                    SetWidth();
                break;

            default:
                Debug.LogErrorFormat("Unsupported camera size type: {0}", EdgeType);
                SetHeight();
                break;
        }
    }

    void SetHeight()
    {
        Camera.orthographicSize = Size / 2f;
    }

    void SetWidth()
    {
        Camera.orthographicSize = _Size / Camera.aspect;
    }

    public void ScreenSizeChanged(int width, int height)
    {
        UpdateCameraSize();
    }

#if UNITY_EDITOR
    [InspectorButton]
    public void ForceUpdate()
    {
        UpdateCameraSize();
    }
#endif
}