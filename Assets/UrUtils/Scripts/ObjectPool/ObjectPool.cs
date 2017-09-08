//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : MonoBehaviour
{
    static ObjectPool Instance;
    static bool IsQuit = false;

    [SerializeField]
    bool AllowNewPoolLists = false;
    [SerializeField]
    GameObject[] InitialPrefabs = null;


    Dictionary<GameObject, List<GameObject>> PooledObjects = new Dictionary<GameObject, List<GameObject>>();
    Dictionary<GameObject, GameObject> SpawnedObjects = new Dictionary<GameObject, GameObject>();
    //Dictionary<GameObject, GameObject> ToRecycle = new Dictionary<GameObject, GameObject>();
    //Coroutine RecycleCoroutine;
    int Counter = 0;


    #region Behaviours
    void Awake()
    {
        Instance = GetComponent<ObjectPool>();

        foreach (var prefab in InitialPrefabs)
            PooledObjects.Add(prefab, new List<GameObject>());
    }

    void OnApplicationQuit()
    {
        IsQuit = true;
    }
    #endregion


    #region Non-static
    GameObject GetFromPool(GameObject prefab)
    {
        List<GameObject> list;
        if (!PooledObjects.TryGetValue(prefab, out list))
        {
            if (!AllowNewPoolLists)
                return null;

            list = new List<GameObject>();
            PooledObjects.Add(prefab, list);
        }

        GameObject replica = null;
        for (int i = list.Count; 0 < i && replica == null; --i)
        {
            replica = list[0];
            list.RemoveAt(0);
        }

        if (replica == null)
        {
            replica = Instantiate(prefab);
            replica.name = string.Format("'{0}' #{1}", prefab.name, ++Counter);
        }
        else
            replica.SetActive(prefab.activeSelf);

        SpawnedObjects.Add(replica, prefab);
        return replica;
    }

    void ReturnToPool(GameObject replica)
    {
        //if (ToRecycle.ContainsKey(replica))
        //    return; // Already marked to recycle - just ignore it

        GameObject prefab;
        if (SpawnedObjects.TryGetValue(replica, out prefab))
        {
            SpawnedObjects.Remove(replica);


            var objectFromPool = replica.GetComponent<ObjectFromPool>();
            if (objectFromPool != null)
                objectFromPool.OnRecycle.Invoke(replica);

            PooledObjects[prefab].Add(replica);
            replica.transform.SetParent(transform, false);
            replica.SetActive(false);

            /*ToRecycle.Add(replica, prefab);
            if (RecycleCoroutine == null)
                RecycleCoroutine = StartCoroutine(Recycle());*/
        }
        else
        {
            DestroyObject(replica);
        }
    }

    /*IEnumerator Recycle()
    {
        yield return new WaitForEndOfFrame();

        foreach (var element in ToRecycle)
        {
            var replica = element.Key;
            var prefab = element.Value;

            var objectFromPool = replica.GetComponent<ObjectFromPool>();
            if (objectFromPool != null)
                objectFromPool.OnRecycle.Invoke(replica);

            PooledObjects[prefab].Add(replica);
            replica.transform.SetParent(transform, false);
            replica.SetActive(false);
        }
        ToRecycle.Clear();
        RecycleCoroutine = null;
    }*/
    #endregion Non-static


    #region Static
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool worldPosition)
    {
        GameObject replica;
        if (Instance != null)
        {
            replica = Instance.GetFromPool(prefab);
            if (replica != null)
            {
                replica.transform.parent = parent;
                if (worldPosition)
                    replica.transform.SetPositionAndRotation(position, rotation);
                else
                {
                    replica.transform.localPosition = position;
                    replica.transform.localRotation = rotation;
                }
            }
            else
                replica = InstantiatePrefab(prefab, position, rotation, parent, worldPosition);
        }
        else
            replica = InstantiatePrefab(prefab, position, rotation, parent, worldPosition);

        var objectFromPool = replica.GetComponent<ObjectFromPool>();
        if (objectFromPool != null)
            objectFromPool.OnRestart.Invoke(replica);

        return replica;
    }

    static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool worldPosition)
    {
        var replica = Instantiate(prefab, position, rotation, parent);
        if (!worldPosition)
        {
            replica.transform.localPosition = position;
            replica.transform.localRotation = rotation;
        }
        return replica;
    }

    public static void Recycle(GameObject replica)
    {
        if (IsQuit)
            return;

        if (Instance != null)
            Instance.ReturnToPool(replica);
        else
            DestroyObject(replica);
    }

    static void DestroyObject(GameObject replica)
    {
        var objectFromPool = replica.GetComponent<ObjectFromPool>();
        if (objectFromPool != null)
            objectFromPool.OnRecycle.Invoke(replica);
        Destroy(replica);
    }
    #endregion Static
}