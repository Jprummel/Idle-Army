using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class UpgradeBuyButton : BuyButton
{
    [SerializeField] private UpgradeData m_UpgradeData;
    [SerializeField] private Image m_UpgradeIcon;

    public override void Awake()
    {
        base.Awake();
        //UIEvents.OnHerosChanged += RefreshUI;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        //UIEvents.OnHerosChanged -= RefreshUI;
    }

    private void Start()
    {
        SetupUI();
    }

    public override void OnBuy()
    {
        base.OnBuy();
        if (CanAfford())
        {
            GameManager.Instance.Wallet.TakeGold(m_UpgradeData.Cost);
            GameManager.Instance.UpgradesManager.UnlockUpgrade(m_UpgradeData);
            RefreshUI();
        }
    }

    private void SelectUpgrade()
    {

    }

    void SetupUI()
    {
        m_UpgradeIcon.sprite = m_UpgradeData.UpgradeIcon;
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
/*        //m_AutoClickerName.SetText($"{m_AutoClicker.Name}");
        m_Cost.SetText($"{CalculateCost()}");
        bool ownsThisType = GameManager.Instance.HeroManager.Heros.ContainsKey(m_HeroData);
        if (ownsThisType)
        {
            m_Level.SetText($"Lvl.{GameManager.Instance.HeroManager.Heros[m_HeroData]}");
        }
        else
        {
            m_Level.SetText("Not unlocked");
        }*/
    }

    private bool CanAfford()
    {
        return GameManager.Instance.Wallet.Gold >= m_UpgradeData.Cost;
    }
}
