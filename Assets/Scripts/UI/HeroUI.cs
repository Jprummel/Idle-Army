using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : BuyButton
{
    [TabGroup("Tabs", "Data")][SerializeField] private HeroData m_HeroData;

    [TabGroup("Tabs", "UI References")][SerializeField] private Image m_HeroImage;
    [TabGroup("Tabs", "UI References")][SerializeField] private TextMeshProUGUI m_HeroName;
    [TabGroup("Tabs", "UI References")][SerializeField] private TextMeshProUGUI m_Cost;
    [TabGroup("Tabs", "UI References")][SerializeField] private TextMeshProUGUI m_Level;

    [TabGroup("Tabs", "UI References")][SerializeField] private CanvasGroup m_StatsCanvasGroup;
    [TabGroup("Tabs", "UI References")] [SerializeField] private TextMeshProUGUI m_DamageText;

    public override void Awake()
    {
        base.Awake();
        UIEvents.OnHerosChanged += RefreshUI;
        UIEvents.OnUpgradeUnlocked += RefreshUI;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        UIEvents.OnHerosChanged -= RefreshUI;
        UIEvents.OnUpgradeUnlocked -= RefreshUI;
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
            GameManager.Instance.GoldManager.TakeGold(CalculateCost());
            GameManager.Instance.HeroManager.AddHero(m_HeroData, 1);
            GameEvents.HerosChanged();
            UIEvents.HerosChanged();
            RefreshUI();
        }
    }

    void SetupUI()
    {
        m_HeroName.SetText($"{m_HeroData.Name}");
        m_HeroImage.sprite = m_HeroData.Sprite;
        RefreshUI();
    }

    public void Spawn(HeroData heroData)
    {
        m_HeroData = heroData;
        SetupUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        //m_AutoClickerName.SetText($"{m_AutoClicker.Name}");
        m_Cost.SetText( $"{CalculateCost()}" );
        bool ownsThisType = GameManager.Instance.HeroManager.Heros.ContainsKey(m_HeroData);
        if (ownsThisType)
        {
            m_Level.SetText($"Lvl.{GameManager.Instance.HeroManager.Heros[m_HeroData]}");
            m_StatsCanvasGroup.alpha = 1;
            m_DamageText.SetText($"{GameManager.Instance.HeroManager.CalculateDamage(m_HeroData)}");
        }
        else
        {
            m_StatsCanvasGroup.alpha = 0;
            m_Level.SetText("Not unlocked");
        }
    }

    private int CalculateCost()
    {
        if (GameManager.Instance.HeroManager.Heros.ContainsKey(m_HeroData))
        {
            return Mathf.RoundToInt(m_HeroData.BaseCost * Mathf.Pow(m_HeroData.CostMultiplier, GameManager.Instance.HeroManager.Heros[m_HeroData]));
        }
        else
        {
            return m_HeroData.BaseCost;
        }
    }

    private bool CanAfford()
    {
        return GameManager.Instance.GoldManager.Gold >= CalculateCost();
    }
}