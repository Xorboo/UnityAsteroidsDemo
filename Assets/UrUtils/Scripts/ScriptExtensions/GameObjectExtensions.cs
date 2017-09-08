//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using System.Collections.Generic;


public static class GameObjectExtensions
{
    public static bool HasRigidbody(this GameObject gameObject)
    {
        return (gameObject.GetComponent<Rigidbody>() != null);
    }

    public static bool HasAnimation(this GameObject gameObject)
    {
        return (gameObject.GetComponent<UnityEngine.Animation>() != null);
    }

    public static List<T> FindComponents<T>(bool includeActive = true, bool includeInactive = false) where T : Component
    {
#if UNITY_EDITOR
        var allObjects = Resources.FindObjectsOfTypeAll<T>();
        var result = new List<T>(allObjects.Length);
        foreach (var obj in allObjects)
        {
            var objType = UnityEditor.PrefabUtility.GetPrefabType(obj);
            if (objType != UnityEditor.PrefabType.Prefab && objType != UnityEditor.PrefabType.ModelPrefab)
            {
                bool isActive = obj.gameObject.activeSelf;
                if (isActive && includeActive || !isActive && includeInactive)
                {
                    result.Add(obj);
                }
            }
        }
        return result;
#else
        Debug.LogError("FindComponents<T>() availible only in editor mode!");
        return new List<T>();
#endif
    }

    public static void SetActiveWithTest(this GameObject gameObject, bool active)
    {
        if (gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }
    }


    #region Object Pooling
    public static GameObject Spawn(this GameObject prefab)
    {
        return prefab.Spawn(prefab.transform.localPosition, prefab.transform.localRotation, null);
    }

    public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return prefab.Spawn(position, rotation, null);
    }

    public static GameObject Spawn(this GameObject prefab, Transform parent, bool worldPosition = false)
    {
        return prefab.Spawn(prefab.transform.localPosition, prefab.transform.localRotation, parent, worldPosition);
    }

    public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool worldPosition = true)
    {
        return ObjectPool.Spawn(prefab, position, rotation, parent, worldPosition);
    }

    public static void Recycle(this GameObject replica)
    {
        ObjectPool.Recycle(replica);
    }

    public static T Spawn<T>(this T prefab) where T: Component
    {
        return prefab.Spawn(prefab.transform.localPosition, prefab.transform.localRotation, null);
    }

    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        return prefab.Spawn(position, rotation, null);
    }

    public static T Spawn<T>(this T prefab, Transform parent, bool worldPosition = false) where T : Component
    {
        return prefab.Spawn(prefab.transform.localPosition, prefab.transform.localRotation, parent, worldPosition);
    }

    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation, Transform parent, bool worldPosition = false) where T : Component
    {
        return ObjectPool.Spawn(prefab.gameObject, position, rotation, parent, worldPosition).GetComponent<T>();
    }

    public static void Recycle<T>(T obj) where T : Component
    {
        obj.gameObject.Recycle();
    }
    #endregion
}
