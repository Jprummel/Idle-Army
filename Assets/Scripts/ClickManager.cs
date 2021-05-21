using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;

[Serializable]
public class ClickManager : MonoBehaviour
{
    #region Properties
    [SerializeField,HideInInspector]
    private ClickerState state = new ClickerState();

    //Auto clickers
    [ShowInInspector] public Dictionary<string, List<AutoClicker>> m_AutoClickers = new Dictionary<string, List<AutoClicker>>();

    [ShowInInspector]private List<AutoClicker> m_Villagers
    {
        get { return this.state.Villagers; }
        set { this.state.Villagers = value; }
    }
    [ShowInInspector] private List<AutoClicker> m_Squires
    {
        get { return this.state.Squires; }
        set { this.state.Squires = value; }
    }
    [ShowInInspector] private List<AutoClicker> m_Rangers
    {
        get { return this.state.Rangers; }
        set { this.state.Rangers = value; }
    }
    [ShowInInspector] private List<AutoClicker> m_Knights
    {
        get { return this.state.Knights; }
        set { this.state.Knights = value; }
    }

    //Auto Clicker Costs
    [TabGroup("Unit Costs")] [ShowInInspector] private int m_VillagerCost
    {
        get { return this.state.VillagerCost; }
        set { this.state.VillagerCost = value; }
    }
    [TabGroup("Unit Costs")] [ShowInInspector] private int m_SquireCost
    {
        get { return this.state.SquireCost; }
        set { this.state.SquireCost = value; }
    }
    [TabGroup("Unit Costs")] [ShowInInspector] private int m_RangerCost
    {
        get { return this.state.RangerCost; }
        set { this.state.RangerCost = value; }
    }
    [TabGroup("Unit Costs")] [ShowInInspector] private int m_KnightCost
    {
        get { return this.state.KnightCost; }
        set { this.state.KnightCost = value; }
    }

    //Auto Clicker UI
    [TabGroup("Unit Amount UI")] [SerializeField] private TextMeshProUGUI m_VillagerAmountText;
    [TabGroup("Unit Amount UI")] [SerializeField] private TextMeshProUGUI m_SquireAmountText;
    [TabGroup("Unit Amount UI")] [SerializeField] private TextMeshProUGUI m_RangerAmountText;
    [TabGroup("Unit Amount UI")] [SerializeField] private TextMeshProUGUI m_KnightAmountText;
    [TabGroup("Unit Cost UI")] [SerializeField] private TextMeshProUGUI m_VillagerCostText;
    [TabGroup("Unit Cost UI")] [SerializeField] private TextMeshProUGUI m_SquireCostText;
    [TabGroup("Unit Cost UI")] [SerializeField] private TextMeshProUGUI m_RangerCostText;
    [TabGroup("Unit Cost UI")] [SerializeField] private TextMeshProUGUI m_KnightCostText;
    #endregion

    private void Awake()
    {
        GameManager.s_OnResetGame += ResetClickers;
        LoadState();
    }

    private void Update()
    {        
        for (int i = 0; i < m_Villagers.Count; i++)
        {
            m_Villagers[i].Attack();
        }
        for (int i = 0; i < m_Squires.Count; i++)
        {
            m_Squires[i].Attack();
        }
        for (int i = 0; i < m_Rangers.Count; i++)
        {
            m_Villagers[i].Attack();
        }
        for (int i = 0; i < m_Knights.Count; i++)
        {
            m_Villagers[i].Attack();
        }
    }
    #region SHOP CODE

    private void BuyUnit<T>(T unit, List<T> listToAddTo, int unitCost)
    {
        if (GameManager.s_Instance.IsPurchasePossible(unitCost))
        {
            GameManager.s_Instance.TakeGold(unitCost);
            listToAddTo.Add(unit);
            DataManager.Save(FileNameConfig.CLICKERDATA, this.state); //Save the state of clickers after buying a new one
        }
    }


    public void OnBuyVillager()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_VillagerCost))
        {
            BuyUnit(new Villager(), m_Villagers, m_VillagerCost);
            m_VillagerCost = CalculateNewCost(m_VillagerCost, 1.15f);
            UpdateUI(m_VillagerAmountText, m_Villagers.Count, m_VillagerCostText, "Villager", m_VillagerCost);
        }
    }

    public void OnBuySquire()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_SquireCost))
        {
            BuyUnit(new Squire(), m_Squires, m_SquireCost);
            m_SquireCost = CalculateNewCost(m_SquireCost, 1.13f);
            UpdateUI(m_SquireAmountText, m_Squires.Count, m_SquireCostText, "Squire", m_SquireCost);
        }
    }

    public void OnBuyRanger()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_RangerCost))
        {
            BuyUnit(new Ranger(), m_Rangers, m_RangerCost);
            m_RangerCost = CalculateNewCost(m_RangerCost, 1.13f);
            UpdateUI(m_RangerAmountText, m_Rangers.Count, m_RangerCostText, "Ranger", m_RangerCost);
        }
    }

    public void OnBuyKnight()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_KnightCost))
        {
            BuyUnit(new Knight(), m_Knights, m_KnightCost);
            m_KnightCost = CalculateNewCost(m_KnightCost, 1.13f);
            UpdateUI(m_KnightAmountText, m_Knights.Count, m_KnightCostText, "Knight", m_KnightCost);
        }
    }
    #endregion

    void UpdateUI(TextMeshProUGUI unitAmountText,int unitAmount, TextMeshProUGUI unitCostText,string unitName, int unitCost)
    {
        unitAmountText.text = $"x {unitAmount}";
        unitCostText.text = $"{unitName}\n {unitCost} Gold";
    }

    int CalculateNewCost(int unitCost, float costMultiplier)
    {        
       return unitCost = Mathf.RoundToInt(Mathf.Pow(unitCost, costMultiplier));  
    }

    #region Data
    public void LoadState()
    {
        state = DataManager.Load<ClickerState>(FileNameConfig.CLICKERDATA, state);
        m_Villagers = state.Villagers;
        m_VillagerCost = state.VillagerCost;
        m_Squires = state.Squires;
        m_SquireCost = state.SquireCost;
        m_Rangers = state.Rangers;
        m_RangerCost = state.RangerCost;
        m_Knights = state.Knights;
        m_KnightCost = state.KnightCost;

        UpdateUI(m_VillagerAmountText, m_Villagers.Count, m_VillagerCostText, "Villager", m_VillagerCost);
        UpdateUI(m_SquireAmountText, m_Squires.Count, m_SquireCostText, "Squire", m_SquireCost);
        UpdateUI(m_RangerAmountText, m_Rangers.Count, m_RangerCostText, "Ranger", m_RangerCost);
        UpdateUI(m_KnightAmountText, m_Knights.Count, m_KnightCostText, "Knight", m_KnightCost);
    }

    void ResetClickers()
    {        
        m_Villagers.Clear();
        m_Squires.Clear();
        m_Rangers.Clear();
        m_Knights.Clear();

        m_VillagerCost = 10;
        m_SquireCost = 100;
        m_RangerCost = 200;
        m_KnightCost = 500;

        UpdateUI(m_VillagerAmountText, m_Villagers.Count, m_VillagerCostText, "Villager", m_VillagerCost);
        UpdateUI(m_SquireAmountText, m_Squires.Count, m_SquireCostText, "Squire", m_SquireCost);
        UpdateUI(m_RangerAmountText, m_Rangers.Count, m_RangerCostText, "Ranger", m_RangerCost);
        UpdateUI(m_KnightAmountText, m_Knights.Count, m_KnightCostText, "Knight", m_KnightCost);
        DataManager.Save(FileNameConfig.CLICKERDATA, this.state);
    }
    #endregion

    private void OnDestroy()
    {
        GameManager.s_OnResetGame -= ResetClickers;
    }

    [Serializable]
    public class ClickerState
    {
        public Dictionary<string, List<AutoClicker>> AutoClickers;
        public List<AutoClicker> Villagers;
        public List<AutoClicker> Squires;
        public List<AutoClicker> Rangers;
        public List<AutoClicker> Knights;

        public int VillagerCost;
        public int SquireCost;
        public int RangerCost;
        public int KnightCost;
    }
}