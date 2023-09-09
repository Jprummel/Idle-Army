using TMPro;
using UnityEngine;

public class BuyButtonAutoClicker : BuyButton
{
    [SerializeField] private AutoClicker m_AutoClicker;

    [SerializeField] private TextMeshProUGUI m_AutoClickerName;
    [SerializeField] private TextMeshProUGUI m_Cost;
    [SerializeField] private TextMeshProUGUI m_Owned;

    private void Awake()
    {
        UIEvents.OnAutoClickersChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        UIEvents.OnAutoClickersChanged -= RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
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

    public override void RefreshUI()
    {
        base.RefreshUI();
        m_AutoClickerName.SetText($"{m_AutoClicker.Name}");
        m_Cost.SetText( $"{CalculateCost()}" );
        bool ownsThisType = GameManager.Instance.AutoClickManager.AutoClickers.ContainsKey(m_AutoClicker);
        if (ownsThisType)
        {
            m_Owned.SetText($"{GameManager.Instance.AutoClickManager.AutoClickers[m_AutoClicker]}");
        }
        else
        {
            m_Owned.SetText("0");
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