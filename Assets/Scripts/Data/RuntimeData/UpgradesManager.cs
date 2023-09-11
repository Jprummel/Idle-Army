using Sirenix.OdinInspector;
using StardustInteractive.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : ManagerBase, ISaveable
{
    private Dictionary<Heros, List<UpgradeData>> m_UnlockedUpgrades = new Dictionary<Heros, List<UpgradeData>>();

    public Dictionary<Heros, List <UpgradeData>> UnlockedUpgrades => m_UnlockedUpgrades;

    public void UnlockUpgrade(UpgradeData upgradeToUnlock)
    {
        if (m_UnlockedUpgrades.ContainsKey(upgradeToUnlock.Hero))
        {
            m_UnlockedUpgrades[upgradeToUnlock.Hero].Add(upgradeToUnlock);
        }
        else
        {
            m_UnlockedUpgrades.Add(upgradeToUnlock.Hero, new List<UpgradeData> {  upgradeToUnlock });
        }
    }

    public void LockUpgrade(Heros hero, UpgradeData upgradeToLock)
    {

    }

    public void ResetAllUpgrades()
    {
        m_UnlockedUpgrades.Clear();
    }

    public float GetTotalUpgradeBonus(Heros hero, UpgradeTypes upgradeType)
    {
        float totalBonus = 0;
        if (m_UnlockedUpgrades.ContainsKey(hero))
        {
            foreach (UpgradeData upgrade in m_UnlockedUpgrades[hero])
            {
                if(upgrade.UpgradeType == upgradeType)
                {
                    totalBonus += upgrade.Multiplier;
                }
            }
        }
        return totalBonus;
    }

    public bool IsUpgradeUnlocked(UpgradeData upgrade)
    {
        bool isUnlocked = false;
        foreach (Heros autoClicker in m_UnlockedUpgrades.Keys)
        {
            foreach (UpgradeData upgradeData in m_UnlockedUpgrades[autoClicker])
            {
                if(upgrade == upgradeData)
                {
                    isUnlocked = true;
                }
            }
        }
        return isUnlocked;
    }

    public void Save(string uniqueIdentifier, string saveFile)
    {
        ES3.Save($"UnlockedUpgrades_{uniqueIdentifier}", m_UnlockedUpgrades, saveFile);
    }

    public void Load(string uniqueIdentifier, string saveFile)
    {
        if (ES3.KeyExists($"UnlockedUpgrades_{uniqueIdentifier}", saveFile))
        {
            m_UnlockedUpgrades = ES3.Load<Dictionary<Heros, List<UpgradeData>>>($"UnlockedUpgrades_{uniqueIdentifier}", saveFile);
        }
    }

    public void ResetData(string uniqueIdentifier, string saveFile)
    {
        ResetAllUpgrades();
    }
}
