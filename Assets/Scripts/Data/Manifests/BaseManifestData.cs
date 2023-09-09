using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManifestData : SerializedScriptableObject
{
    [Button]
    public virtual void GatherData()
    {

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        GatherData();
    }
#endif
}
