using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action OnGameManagerInitialized;
    public static event Action<int> OnGoldUpdated;
    public static event Action OnProgressStage;
    public static event Action OnProgressLevel;

    public static void GameManagerInitialized()
    {
        OnGameManagerInitialized?.Invoke();
    }

    public static void GoldUpdated(int goldChange)
    {
        OnGoldUpdated?.Invoke(goldChange);
    }

    public static void ProgressStage()
    {
        OnProgressStage?.Invoke();
    }

    public static void ProgressLevel()
    {
        OnProgressLevel?.Invoke();
    }
}
