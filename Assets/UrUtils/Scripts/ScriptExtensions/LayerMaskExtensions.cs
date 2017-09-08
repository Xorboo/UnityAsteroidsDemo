//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;


public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask lm, int layer)
    {
        return lm == (lm | (1 << layer));
    }
}
