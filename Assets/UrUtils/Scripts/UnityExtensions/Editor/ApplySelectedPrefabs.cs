//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEditor;
using UnityEngine;


public class ApplySelectedPrefabs : EditorWindow
{
    public delegate void ApplyOrRevert(GameObject _goCurrentGo, Object _ObjPrefabParent, ReplacePrefabOptions _eReplaceOptions);
    [MenuItem("Tools/Apply all selected prefabs %#a")]
    static void ApplyPrefabs()
    {
        SearchPrefabConnections(ApplyToSelectedPrefabs);
    }

    [MenuItem("Tools/Revert all selected prefabs %#r")]
    static void ResetPrefabs()
    {
        SearchPrefabConnections(RevertToSelectedPrefabs);
    }

    //Look for connections
    static void SearchPrefabConnections(ApplyOrRevert _applyOrRevert)
    {
        GameObject[] tSelection = Selection.gameObjects;

        if (tSelection.Length > 0)
        {
            GameObject goPrefabRoot;
            //GameObject goParent;
            GameObject goCur;
            bool bTopHierarchyFound;
            int iCount = 0;
            PrefabType prefabType;
            bool bCanApply;
            //Iterate through all the selected gameobjects
            foreach (GameObject go in tSelection)
            {
                prefabType = PrefabUtility.GetPrefabType(go);
                //Is the selected gameobject a prefab?
                if (prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
                {
                    //Prefab Root;
                    goPrefabRoot = ((GameObject)PrefabUtility.GetPrefabParent(go)).transform.root.gameObject;
                    goCur = go;
                    bTopHierarchyFound = false;
                    bCanApply = true;
                    //We go up in the hierarchy to apply the root of the go to the prefab
                    while (goCur.transform.parent != null && !bTopHierarchyFound)
                    {
                        //Are we still in the same prefab?
                        Object prefabParent = PrefabUtility.GetPrefabParent(goCur.transform.parent.gameObject);
                        if (prefabParent != null && goPrefabRoot == (prefabParent as GameObject).transform.root.gameObject)
                        {
                            goCur = goCur.transform.parent.gameObject;
                        }
                        else
                        {
                            //The gameobject parent is another prefab, we stop here
                            bTopHierarchyFound = true;
                            if (goPrefabRoot != ((GameObject)PrefabUtility.GetPrefabParent(goCur)))
                            {
                                //Gameobject is part of another prefab
                                bCanApply = false;
                            }
                        }
                    }

                    if (_applyOrRevert != null && bCanApply)
                    {
                        iCount++;
                        _applyOrRevert(goCur, PrefabUtility.GetPrefabParent(goCur), ReplacePrefabOptions.ConnectToPrefab);
                    }
                }
            }
            Debug.Log(iCount + " prefab" + (iCount > 1 ? "s" : "") + " updated");
        }
    }

    //Apply      
    static void ApplyToSelectedPrefabs(GameObject _goCurrentGo, Object _ObjPrefabParent, ReplacePrefabOptions _eReplaceOptions)
    {
        PrefabUtility.ReplacePrefab(_goCurrentGo, _ObjPrefabParent, _eReplaceOptions);
    }

    //Revert
    static void RevertToSelectedPrefabs(GameObject _goCurrentGo, Object _ObjPrefabParent, ReplacePrefabOptions _eReplaceOptions)
    {
        PrefabUtility.ReconnectToLastPrefab(_goCurrentGo);
        PrefabUtility.RevertPrefabInstance(_goCurrentGo);
    }
}