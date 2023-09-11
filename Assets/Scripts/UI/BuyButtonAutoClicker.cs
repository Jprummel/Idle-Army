using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonAutoClicker : BuyButton
{
    [SerializeField] private AutoClickerData m_AutoClicker;

    [SerializeField] private Image m_AutoClickerImage;
    [SerializeField] private TextMeshProUGUI m_AutoClickerName;
    [SerializeField] private TextMeshProUGUI m_Cost;
    [SerializeField] private TextMeshProUGUI m_Level;

    public override void Awake()
    {
        base.Awake();
        UIEvents.OnAutoClickersChanged += RefreshUI;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        UIEvents.OnAutoClickersChanged -= RefreshUI;
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
            GameManager.Instance.AutoClickManager.AddClicker(m_AutoClicker, 1);
            RefreshUI();
        }
    }

    void SetupUI()
    {
        m_AutoClickerName.SetText($"{m_AutoClicker.Name}");
        m_AutoClickerImage.sprite = m_AutoClicker.AutoClickerSprite;
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        //m_AutoClickerName.SetText($"{m_AutoClicker.Name}");
        m_Cost.SetText( $"{CalculateCost()}" );
        bool ownsThisType = GameManager.Instance.AutoClickManager.AutoClickers.ContainsKey(m_AutoClicker);
        if (ownsThisType)
        {
            m_Level.SetText($"Lvl.{GameManager.Instance.AutoClickManager.AutoClickers[m_AutoClicker]}");
        }
        else
        {
            m_Level.SetText("Not unlocked");
        }
    }

    private int CalculateCost()
    {
        if (GameManager.Instance.AutoClickManager.AutoClickers.ContainsKey(m_AutoClicker))
        {
            return Mathf.RoundToInt(m_AutoClicker.BaseCost * Mathf.Pow(m_AutoClicker.CostMultiplier, GameManager.Instance.AutoClickManager.AutoClickers[m_AutoClicker]));
        }
        else
        {
            return m_AutoClicker.BaseCost;
        }
    }

    private bool CanAfford()
    {
        return GameManager.Instance.Wallet.Gold >= CalculateCost();
    }
}