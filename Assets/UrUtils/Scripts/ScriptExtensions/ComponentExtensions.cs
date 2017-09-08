//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using System;
using System.Reflection;


public static class ComponentExtensions
{
    public static T GetCopyOf<T>(this Component destination, T original) where T : Component
    {
        Type type = destination.GetType();
        return destination.GetCopyOf(original, type);
    }

    public static T GetCopyOf<T>(this Component destination, T original, Type type) where T : Component
    {
        if (type != original.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        var pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(destination, pinfo.GetValue(original, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(destination, finfo.GetValue(original));
        }
        return destination as T;
    }

    public static T CopyTo<T>(this Component original, GameObject destination) where T : Component
    {
        Type type = original.GetType();
        Component copiedComponent = destination.AddComponent(type);
        return copiedComponent.GetCopyOf(original, type) as T;
    }
}
