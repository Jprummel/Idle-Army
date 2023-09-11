using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroBuyButton : BuyButton
{
    [SerializeField] private HeroData m_HeroData;

    [SerializeField] private Image m_HeroImage;
    [SerializeField] private TextMeshProUGUI m_HeroName;
    [SerializeField] private TextMeshProUGUI m_Cost;
    [SerializeField] private TextMeshProUGUI m_Level;

    public override void Awake()
    {
        base.Awake();
        UIEvents.OnHerosChanged += RefreshUI;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        UIEvents.OnHerosChanged -= RefreshUI;
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
            GameManager.Instance.Wallet.TakeGold(CalculateCost());
            GameManager.Instance.HeroManager.AddHero(m_HeroData, 1);
            GameEvents.HerosChanged();
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
        }
        else
        {
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
        return GameManager.Instance.Wallet.Gold >= CalculateCost();
    }
}