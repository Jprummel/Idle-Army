using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopUI : TabContentContainer
{
    [SerializeField] private UpgradeManifest m_UpgradeManifest;
    [SerializeField] private UpgradeButton m_UpgradeButtonPrefab;
    [SerializeField] private Transform m_ButtonSpawnParent;

    [SerializeField] private CanvasGroup m_InfoBoxCanvasGroup;
    [SerializeField] private TextMeshProUGUI m_UpgradeName;
    [SerializeField] private TextMeshProUGUI m_UpgradeDescription;
    [SerializeField] private TextMeshProUGUI m_UpgradeCost;
    [SerializeField] private Button m_BuyButton;

    private List<UpgradeButton> m_UpgradeButtons = new List<UpgradeButton>();

    private UpgradeData m_SelectedUpgrade;

    private void Awake()
    {
        GameEvents.OnHerosChanged += PopulateUI;
        UIEvents.OnHerosChanged += PopulateUI;
        GameEvents.OnUpgradeUnlocked += PopulateUI;
        UIEvents.OnUpgradeUnlocked += PopulateUI;

        m_BuyButton.onClick.AddListener(BuyUpgrade);
    }

    private void OnDestroy()
    {
        GameEvents.OnHerosChanged -= PopulateUI;
        UIEvents.OnHerosChanged -= PopulateUI;
        GameEvents.OnUpgradeUnlocked -= PopulateUI;
        UIEvents.OnUpgradeUnlocked -= PopulateUI;

        m_BuyButton.onClick.RemoveListener(BuyUpgrade);
    }

    public override void Show()
    {
        base.Show();
        PopulateUI();
        SelectUpgrade(null);
    }

    private void SelectUpgrade(UpgradeData upgrade)
    {
        if(upgrade == null)
        {
            m_InfoBoxCanvasGroup.alpha = 0;
            return;
        }
        m_InfoBoxCanvasGroup.alpha = 1;
        m_SelectedUpgrade = upgrade;
        m_UpgradeName.SetText($"{upgrade.UpgradeName}");
        m_UpgradeDescription.SetText($"{upgrade.UpgradeDescription}");
        m_UpgradeCost.SetText($"{upgrade.Cost}");
    }

    void PopulateUI()
    {
        for (int i = 0; i < m_UpgradeButtons.Count; i++)
        {
            m_UpgradeButtons[i].gameObject.SetActive(false);
        }

        m_UpgradeButtons.Clear();

        for (int i = 0; i < GameManager.Instance.UpgradesManager.UnlockedUpgrades.Count; i++)
        {
            if(i < m_UpgradeButtons.Count)
            {
                m_UpgradeButtons[i].SetUpgradeData(GameManager.Instance.UpgradesManager.UnlockedUpgrades[i]);
                m_UpgradeButtons[i].gameObject.SetActive(true);
            }

            if(i >= m_UpgradeButtons.Count)
            {
                UpgradeButton newButton = Instantiate(m_UpgradeButtonPrefab, m_ButtonSpawnParent);
                newButton.SetUpgradeData(GameManager.Instance.UpgradesManager.UnlockedUpgrades[i]);
                newButton.OnSelectUpgrade += SelectUpgrade;
                m_UpgradeButtons.Add(newButton);
            }
        }
    }

    public void BuyUpgrade()
    {
        if (CanAfford())
        {
            GameManager.Instance.GoldManager.TakeGold(m_SelectedUpgrade.Cost);
            GameManager.Instance.UpgradesManager.ObtainUpgrade(m_SelectedUpgrade);

            GameEvents.UpgradeUnlocked();
            UIEvents.UpgradeUnlocked();
        }
        SelectUpgrade(null);
    }

    private bool CanAfford()
    {
        return GameManager.Instance.GoldManager.Gold >= m_SelectedUpgrade.Cost;
    }
}
