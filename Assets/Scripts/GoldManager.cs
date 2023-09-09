using Sirenix.OdinInspector;
using StardustInteractive.Saving;
using System;
using UnityEngine;

public class GoldManager : ManagerBase, ISaveable
{

    [SerializeField] private int m_StartingGold;

    [ShowInInspector,ReadOnly]private int m_Gold;

    public int Gold => m_Gold;

    public override void Initialize()
    {
        m_Gold = m_StartingGold;
        base.Initialize();
    }

    public override void PostInitialize()
    {
        base.PostInitialize();
        GameEvents.GoldUpdated(m_Gold);
    }

    public void UpdateGoldBalance(int amount)
    {
        m_Gold += amount;
        GameEvents.GoldUpdated(m_Gold);
        UIEvents.GoldUpdated(m_Gold);
    }

    public void AddGold(int amount)
    {
        m_Gold += amount;
        GameEvents.GoldUpdated(m_Gold);
        UIEvents.GoldUpdated(m_Gold);
    }

    public void TakeGold(int amount)
    {
        m_Gold -= amount;
        GameEvents.GoldUpdated(m_Gold);
        UIEvents.GoldUpdated(m_Gold);
    }

    public void Save(string uniqueIdentifier, string saveFile)
    {
        ES3.Save($"Gold_{uniqueIdentifier}", m_Gold, saveFile);
    }

    public void Load(string uniqueIdentifier, string saveFile)
    {
        if (ES3.KeyExists($"Gold_{uniqueIdentifier}", saveFile))
        {
            m_Gold = ES3.Load<int>($"Gold_{uniqueIdentifier}",saveFile);
        }
        GameEvents.GoldUpdated(m_Gold);
        UIEvents.GoldUpdated(m_Gold);
    }

    public void ResetData(string uniqueIdentifier, string saveFile)
    {
        m_Gold = m_StartingGold;
    }
}
