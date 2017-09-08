//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;

/// <summary>
/// Place an UI element to a world position
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class PlaceUIElementAtWorldPosition : MonoBehaviour
{
    [SerializeField]
    RectTransform Canvas = null;

    RectTransform RectTransform;

    /// <summary>
    /// Initiate
    /// </summary>
    void Awake()
    {
        // Get the rect transform
        RectTransform = GetComponent<RectTransform>();

        // Calculate the screen offset
        //UiOffset = new Vector2(Canvas.sizeDelta.x / 2f, Canvas.sizeDelta.y / 2f);
    }

    /// <summary>
    /// Move the UI element to the world position
    /// </summary>
    /// <param name="objectTransformPosition"></param>
    public void MoveToClickPoint(Vector3 objectTransformPosition)
    {
        // Get the position on the canvas
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
        Vector2 proportionalPosition = Vector2.Scale(new Vector2(ViewportPosition.x - 0.5f, ViewportPosition.y - 0.5f), Canvas.sizeDelta);

        // Set the position and remove the screen offset
        RectTransform.localPosition = proportionalPosition;
    }
}