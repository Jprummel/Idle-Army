using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static event Action<int> OnGoldUpdated;
    public static event Action OnHerosChanged;
    public static event Action<EnemyData> OnEnemySpawned;
    public static event Action OnProgressStage;
    public static event Action OnProgressLevel;

    public static void GoldUpdated(int goldValue)
    {
        OnGoldUpdated?.Invoke(goldValue);
    }

    public static void EnemySpawned(EnemyData enemyData)
    {
        OnEnemySpawned?.Invoke(enemyData);
    }

    public static void ProgressStage()
    {
        OnProgressStage?.Invoke();
    }
    public static void ProgressLevel()
    {
        OnProgressLevel?.Invoke();
    }

    public static void AutoClickersChanged()
    {
        OnHerosChanged?.Invoke();
    }

}
