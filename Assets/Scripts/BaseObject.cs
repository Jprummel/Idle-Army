using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : SerializedMonoBehaviour
{
    private void Awake()
    {
        //GameEvents.OnGameManagerInitialized += Initialize;
        Initialize();
    }

    //Always call the base at the end
    public virtual void Initialize()
    {
        PostInitialize();
    }

    public virtual void PostInitialize()
    {

    }

    //Always call the base at the end
    public virtual void DeInitialize()
    {

    }

    private void OnDestroy()
    {
        DeInitialize();
        GameEvents.OnGameManagerInitialized -= Initialize;
    }
}
