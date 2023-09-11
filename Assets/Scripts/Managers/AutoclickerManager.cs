using Sirenix.OdinInspector;
using StardustInteractive.Saving;
using StardustInteractive.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AutoclickerManager : ManagerBase, ISaveable
{
    [ShowInInspector, ReadOnly] private Dictionary<AutoClickerData, int> m_AutoClickers = new Dictionary<AutoClickerData, int>();
    [SerializeField] private AutoClickerManifest m_autoClickerManifest;

    public Dictionary<AutoClickerData, int> AutoClickers => m_AutoClickers;

    private event Action<AutoClickerData> OnClickerAdded;

    private float m_TotalClickerDamage;

    public override void Initialize()
    {
        OnClickerAdded += DealDamage;
        base.Initialize();
    }

    public override void DeInitialize()
    {
        OnClickerAdded -= DealDamage;
        base.DeInitialize();
    }

    public void AddClicker(AutoClickerData autoClicker, int amount)
    {
        if (m_AutoClickers.ContainsKey(autoClicker))
        {
            m_AutoClickers[autoClicker] += amount;
        }
        else
        {
            m_AutoClickers.Add(autoClicker, amount);
        }
        TimerManager.AddTimer(autoClicker.AttackTimerID, autoClicker.AttackSpeed, () => { OnClickerAdded(autoClicker); }, repeats: -1, shouldReset: false);
    }

    private void CalculateDamage()
    {
        int damage = 0;
        foreach (AutoClickerData autoClicker in m_AutoClickers.Keys)
        {
            damage += Mathf.RoundToInt(m_autoClickerManifest.GetAutoClickerByType(autoClicker.AutoClickerType).Damage * m_AutoClickers[autoClicker]);
        }
        m_TotalClickerDamage = damage;
        Debug.Log($"Total damage : {m_TotalClickerDamage}");
    }

    private int CalculateDamage(AutoClickerData autoClicker)
    {
        int damage = 0;
        if (m_AutoClickers.ContainsKey(autoClicker))
        {
            damage = Mathf.RoundToInt(m_autoClickerManifest.GetAutoClickerByType(autoClicker.AutoClickerType).Damage * m_AutoClickers[autoClicker]);
        }
        return damage;
    }

    private void DealDamage(AutoClickerData autoClicker)
    {
        GameManager.Instance.EnemyManager.Damage(CalculateDamage(autoClicker));
    }

    public void Save(string uniqueIdentifier, string saveFile)
    {
        ES3.Save($"AutoClickers_{uniqueIdentifier}", m_AutoClickers, saveFile);
    }

    public void Load(string uniqueIdentifier, string saveFile)
    {
        if (ES3.KeyExists($"AutoClickers_{uniqueIdentifier}", saveFile))
        {
            m_AutoClickers = ES3.Load<Dictionary<AutoClickerData, int>>($"AutoClickers_{uniqueIdentifier}", saveFile);
        }

        foreach (AutoClickerData autoClicker in m_AutoClickers.Keys)
        {
            TimerManager.AddTimer(autoClicker.AttackTimerID, autoClicker.AttackSpeed, () => { OnClickerAdded(autoClicker); }, repeats: -1, shouldReset: false);
        }
        UIEvents.AutoClickersChanged();
    }

    public void ResetData(string uniqueIdentifier, string saveFile)
    {
        m_AutoClickers.Clear();
    }
}