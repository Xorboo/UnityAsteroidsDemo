//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class ImageAnimator : AnimatorBase
{
    Image Image
    {
        get
        {
            if (!_Image)
                _Image = GetComponent<Image>();
            return _Image;
        }
    }
    Image _Image = null;


    protected override void SetSprite(Sprite sprite)
    {
        Image.sprite = sprite;
    }
}
