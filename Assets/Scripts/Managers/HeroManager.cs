using Sirenix.OdinInspector;
using StardustInteractive.Saving;
using StardustInteractive.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : ManagerBase, ISaveable
{
    [ShowInInspector, ReadOnly] private Dictionary<HeroData, int> m_Heros = new Dictionary<HeroData, int>();
    [SerializeField] private HeroManifest m_HeroManifest;

    public Dictionary<HeroData, int> Heros => m_Heros;

    private event Action<HeroData> OnHeroAdded;

    private float m_TotalHeroDamage;

    public override void Initialize()
    {
        OnHeroAdded += DealDamage;
        base.Initialize();
    }

    public override void DeInitialize()
    {
        OnHeroAdded -= DealDamage;
        base.DeInitialize();
    }

    public void AddHero(HeroData autoClicker, int amount)
    {
        if (m_Heros.ContainsKey(autoClicker))
        {
            m_Heros[autoClicker] += amount;
        }
        else
        {
            m_Heros.Add(autoClicker, amount);
        }
        TimerManager.AddTimer(autoClicker.AttackTimerID, autoClicker.AttackSpeed, () => { OnHeroAdded(autoClicker); }, repeats: -1, shouldReset: false);
    }

    private void CalculateDamage()
    {
        int damage = 0;
        foreach (HeroData autoClicker in m_Heros.Keys)
        {
            damage += Mathf.RoundToInt(m_HeroManifest.GetHero(autoClicker.Hero).Damage * m_Heros[autoClicker]);
        }
        m_TotalHeroDamage = damage;
        Debug.Log($"Total damage : {m_TotalHeroDamage}");
    }

    private int CalculateDamage(HeroData hero)
    {
        int damage = 0;
        if (m_Heros.ContainsKey(hero))
        {
            damage = Mathf.RoundToInt(m_HeroManifest.GetHero(hero.Hero).Damage * m_Heros[hero] * (1 + GameManager.Instance.UpgradesManager.GetTotalUpgradeBonus(hero.Hero, UpgradeTypes.Damage)));
        }
        return damage;
    }

    private void DealDamage(HeroData autoClicker)
    {
        GameManager.Instance.EnemyManager.Damage(CalculateDamage(autoClicker));
    }

    public void Save(string uniqueIdentifier, string saveFile)
    {
        ES3.Save($"AutoClickers_{uniqueIdentifier}", m_Heros, saveFile);
    }

    public void Load(string uniqueIdentifier, string saveFile)
    {
        if (ES3.KeyExists($"AutoClickers_{uniqueIdentifier}", saveFile))
        {
            m_Heros = ES3.Load<Dictionary<HeroData, int>>($"AutoClickers_{uniqueIdentifier}", saveFile);
        }

        foreach (HeroData autoClicker in m_Heros.Keys)
        {
            TimerManager.AddTimer(autoClicker.AttackTimerID, autoClicker.AttackSpeed, () => { OnHeroAdded(autoClicker); }, repeats: -1, shouldReset: false);
        }
        UIEvents.HerosChanged();
        GameEvents.HerosChanged();
    }

    public void ResetData(string uniqueIdentifier, string saveFile)
    {
        m_Heros.Clear();
    }
}