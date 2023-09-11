using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Idle Army/Level Data", order = 0)]
public class LevelData : GameData
{
    [SerializeField] private string m_LevelName;
    [SerializeField] private Sprite m_Background;
    [SerializeField] private List<EnemyData> m_Enemies = new List<EnemyData>();
    [SerializeField] private BossData m_Boss;

    public string LevelName => m_LevelName;
    public Sprite Background => m_Background;
    public List<EnemyData> Enemies => m_Enemies;
    public BossData Boss => m_Boss;

    public EnemyData GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, m_Enemies.Count);
        return m_Enemies[randomIndex];
    }

}
