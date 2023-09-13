using Sirenix.OdinInspector;
using StardustInteractive.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : ManagerBase, ISaveable
{
    [SerializeField] private HeroManifest m_HeroManifest;
    [SerializeField] private UpgradeManifest m_UpgradeManifest;
    [ShowInInspector,ReadOnly]private List<UpgradeData> m_LockedUpgrades =  new List<UpgradeData>();
    [ShowInInspector, ReadOnly] private List<UpgradeData> m_UnlockedUpgrades = new List<UpgradeData>();
    [ShowInInspector, ReadOnly] private Dictionary<Heros, List<UpgradeData>> m_OwnedUpgrades = new Dictionary<Heros, List<UpgradeData>>();

    public List <UpgradeData> UnlockedUpgrades => m_UnlockedUpgrades;
    public Dictionary<Heros, List<UpgradeData>> OwnedUpgrades => m_OwnedUpgrades;
    void Awake()
    {
        m_LockedUpgrades = m_UpgradeManifest.AllUpgrades;
        GameEvents.OnHerosChanged += CheckForUpgradeUnlock;
    }

    private void CheckForUpgradeUnlock()
    {
        for (int i = 0; i < m_LockedUpgrades.Count; i++)
        {
            if (AreUnlockRequirementsMet(m_LockedUpgrades[i]))
            {
                UnlockUpgrade(m_LockedUpgrades[i]);
            }
        }
    }

    private bool AreUnlockRequirementsMet(UpgradeData upgrade)
    {
        HeroData hero = null;
        foreach (UnlockCondition condition in upgrade.UnlockRequirements)
        {
            hero = m_HeroManifest.GetHero(condition.Hero);
            if (!GameManager.Instance.HeroManager.Heros.ContainsKey(hero))
            {
                return false;
            }

            if (GameManager.Instance.HeroManager.Heros[hero] < condition.RequiredLevel)
            {
                return false;
            }
            
        }

        return true;
    }

    public void UnlockUpgrade(UpgradeData upgradeToUnlock)
    {
        if (m_UnlockedUpgrades.Contains(upgradeToUnlock))
        {
            return;
        }
        m_UnlockedUpgrades.Add(upgradeToUnlock);
        m_LockedUpgrades.Remove(upgradeToUnlock);
    }

    public void ObtainUpgrade(UpgradeData upgradeToUnlock)
    {
        if (m_OwnedUpgrades.ContainsKey(upgradeToUnlock.Hero))
        {
            m_OwnedUpgrades[upgradeToUnlock.Hero].Add(upgradeToUnlock);
        }
        else
        {
            m_OwnedUpgrades.Add(upgradeToUnlock.Hero, new List<UpgradeData> { upgradeToUnlock });
        }
        m_UnlockedUpgrades.Remove(upgradeToUnlock);
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
        if (m_OwnedUpgrades.ContainsKey(hero))
        {
            foreach (UpgradeData upgrade in m_OwnedUpgrades[hero])
            {
                if(upgrade.UpgradeType == upgradeType)
                {
                    totalBonus += upgrade.Multiplier;
                }
            }
        }
        return totalBonus;
    }

    public void Save(string uniqueIdentifier, string saveFile)
    {
        ES3.Save($"UnlockedUpgrades_{uniqueIdentifier}", m_UnlockedUpgrades, saveFile);
        ES3.Save($"OwnedUpgrades_{uniqueIdentifier}", m_OwnedUpgrades, saveFile);
    }

    public void Load(string uniqueIdentifier, string saveFile)
    {
        m_LockedUpgrades = m_UpgradeManifest.AllUpgrades;

        if (ES3.KeyExists($"UnlockedUpgrades_{uniqueIdentifier}", saveFile))
        {
            m_UnlockedUpgrades = ES3.Load<List<UpgradeData>>($"UnlockedUpgrades_{uniqueIdentifier}", saveFile);
        }

        if (ES3.KeyExists($"OwnedUpgrades_{uniqueIdentifier}", saveFile))
        {
            m_OwnedUpgrades = ES3.Load<Dictionary<Heros,List<UpgradeData>>>($"OwnedUpgrades_{uniqueIdentifier}", saveFile);
        }

        GameEvents.UpgradeUnlocked();
        UIEvents.UpgradeUnlocked();
    }

    public void ResetData(string uniqueIdentifier, string saveFile)
    {
        ResetAllUpgrades();
    }
}
