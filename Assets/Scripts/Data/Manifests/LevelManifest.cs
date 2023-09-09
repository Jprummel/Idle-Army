using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Manifest", menuName = "Idle Army/Manifests/Level Manifest", order = 0)]
public class LevelManifest : BaseManifestData
{
    [SerializeField] private List<LevelData> m_AllLevels = new List<LevelData>();

    public List<LevelData> AllLevels => m_AllLevels;

    public LevelData GetLevelData(int currentLevel)
    {
        int levelIndex = (currentLevel - 1) % m_AllLevels.Count;
        return m_AllLevels[levelIndex];
    }

    public override void GatherData()
    {
        base.GatherData();
        //m_AllLevels.Clear();
        foreach (LevelData level in AssetDatabaseUtils.FindAssetsByType<LevelData>())
        {
            if (!m_AllLevels.Contains(level))
            {
                m_AllLevels.Add(level);
            }
        }
    }
}
