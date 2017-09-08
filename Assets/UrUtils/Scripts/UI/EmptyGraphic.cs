//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Empty graphic for the button.
/// Use this to change the clickable area of the button without changing size of the image.
/// Set "Target Graphics" to your real image object.
/// </summary>
public class EmptyGraphic : Graphic
{
    public override void SetMaterialDirty() { return; }
    public override void SetVerticesDirty() { return; }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Debug.LogWarning("OnPopulateMesh(VertexHelper vh)");
        vh.Clear();
    }
}