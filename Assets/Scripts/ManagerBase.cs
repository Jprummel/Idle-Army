using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase : SerializedMonoBehaviour
{
    public virtual void Initialize()
    {

    }

    public virtual void PostInitialize()
    {

    }

    public virtual void DeInitialize()
    {

    }

    private void OnDestroy()
    {
        DeInitialize();
    }
}
