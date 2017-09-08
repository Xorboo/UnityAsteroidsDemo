//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Linq;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), true)]
public class ObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var methods = target.GetType().GetMethods().Where(p => p.GetParameters().Length == 0);
        foreach (var method in methods)
        {
            var ba = (InspectorButtonAttribute) Attribute.GetCustomAttribute(method, typeof(InspectorButtonAttribute));
            if (ba != null)
            {
                GUI.enabled = true;
                if (GUILayout.Button(ObjectNames.NicifyVariableName(method.Name)))
                    foreach (var t in targets)
                        method.Invoke(t, null);
                GUI.enabled = true;
            }
        }
    }
}