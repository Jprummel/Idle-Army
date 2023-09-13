using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Manifest", menuName = "Idle Army/Manifests/Enemy Manifest", order = 0)]
public class EnemyManifest : BaseManifestData
{
    [SerializeField] private List<EnemyData> m_AllEnemies = new List<EnemyData>();
    [SerializeField] private List<BossData> m_AllBosses = new List<BossData>();

    public List<EnemyData> AllEnemies => m_AllEnemies;


    public EnemyData GetRandomEnemy()
    {
        int randomIndex = Random.Range(0,m_AllEnemies.Count);
        return m_AllEnemies[randomIndex];
    }

    public BossData GetBossByLevel(int currentLevel)
    {
        int bossIndex = (currentLevel - 1 ) % m_AllBosses.Count;
        return m_AllBosses[bossIndex];
    }

#if UNITY_EDITOR
    public override void GatherData()
    {
        base.GatherData();
        m_AllEnemies.Clear();
        m_AllBosses.Clear();
        foreach (var enemy in AssetDatabaseUtils.FindAssetsByType<EnemyData>())
        {
            m_AllEnemies.Add(enemy);
        }

        foreach (var boss in AssetDatabaseUtils.FindAssetsByType<BossData>())
        {
            m_AllBosses.Add(boss);
        }
    }
#endif
}
