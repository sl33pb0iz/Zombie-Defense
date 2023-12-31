using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class ReconnectPrefabByName : MonoBehaviour
{
    //public List<GameObject> prefabList = new List<GameObject>();

    //[Button]
    //public void ReLink(Transform target)
    //{
    //    if(target == null)
    //    {
    //        target = transform;
    //    }
    //    var replaced = false;
    //    string targetname = target.name.Split(' ')[0];
    //    foreach (GameObject prefab in prefabList)
    //    {
    //        if (!PrefabUtility.IsPartOfAnyPrefab(target) && targetname.Equals(prefab.name))
    //        {
    //            var newTarget = PrefabUtility.InstantiatePrefab(prefab, target.parent) as GameObject;
    //            newTarget.transform.SetPositionAndRotation(target.position, target.rotation);
    //            newTarget.transform.localScale = target.localScale;
    //            newTarget.transform.SetSiblingIndex(target.GetSiblingIndex());
    //            target.SetAsLastSibling();
    //            if(target.childCount == 0)
    //            {
    //                DestroyImmediate(target.gameObject);
    //            }
    //            replaced = true;
    //            break;
    //        }
    //    }
    //    if (replaced) return;
    //    var childCount = target.childCount;
    //    for (int i = 0; i < childCount; i++)
    //    {
    //        ReLink(target.GetChild(i));
    //    }
    //}
}
