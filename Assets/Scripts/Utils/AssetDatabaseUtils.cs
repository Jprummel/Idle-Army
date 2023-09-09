using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetDatabaseUtils : MonoBehaviour
{
    public static IEnumerable<T> FindAssetsByType<T>() where T : Object
    {
        var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var t in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(t);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                yield return asset;
            }
        }
    }
}
