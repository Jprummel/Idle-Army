using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Manifest", menuName = "Idle Army/Manifests/Upgrade Manifest", order = 0)]
public class UpgradeManifest : BaseManifestData
{
    [SerializeField] private List<UpgradeData> m_AllUpgrades;
    
    public List<UpgradeData> AllUpgrades => m_AllUpgrades;

    public override void GatherData()
    {
        base.GatherData();
        m_AllUpgrades.Clear();
        foreach (var upgrade in AssetDatabaseUtils.FindAssetsByType<UpgradeData>())
        {
            m_AllUpgrades.Add(upgrade);
        }
    }
}
