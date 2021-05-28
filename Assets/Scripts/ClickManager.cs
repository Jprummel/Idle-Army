using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

public class ClickManager : SerializedMonoBehaviour
{
    #region Properties
    [SerializeField,HideInInspector]
    private ClickerState state = new ClickerState();

    [TabGroup("Dictionaries","AutoClickers")][OdinSerialize] private Dictionary<string, List<AutoClicker>> m_AutoClickers
    {
        get { return this.state.AutoClickers; }
        set { this.state.AutoClickers = value; }
    }

    [TabGroup("Dictionaries","ShopUI")][OdinSerialize] private Dictionary<string, List<TextMeshProUGUI>> m_ClickerShopTexts = new Dictionary<string, List<TextMeshProUGUI>>();
    #endregion

    private void Awake()
    {
        GameManager.s_OnResetGame += ResetClickers;
        LoadState();
    }

    private void Update()
    {
        foreach (var clicker in m_AutoClickers)
        {
            for (int i = 0; i < clicker.Value.Count; i++)
            {
                clicker.Value[i].Attack();
            }
        }
    }
    #region SHOP CODE
    public void BuyUnit(AutoClicker unit)
    {
        //Check for / set the cost
        int cost = unit.NextUnitCost;
        if (m_AutoClickers.ContainsKey(unit.Name) && ClickersExist(m_AutoClickers[unit.Name]))
        {
            cost = LastClickerInList(unit.Name).NextUnitCost;
        }

        if (GameManager.s_Instance.IsPurchasePossible(cost)) //If player has enough gold buy the unit
        {
            GameManager.s_Instance.TakeGold(cost);
            AutoClicker newUnit = ScriptableObject.CreateInstance<AutoClicker>();
            newUnit.Init(unit.Name, unit.Damage, unit.AttackCooldown);
            if (!m_AutoClickers.ContainsKey(unit.Name)) //Creates a list for this type of autoclicker if there was none in the dictionary yet   
            {
                m_AutoClickers.Add(unit.Name, new List<AutoClicker>()); 
            }
            m_AutoClickers[unit.Name].Add(newUnit);            
            newUnit.UnitCost = cost;
            newUnit.NextUnitCost = CalculateNewCost(LastClickerInList(unit.Name).UnitCost, unit.CostMultiplier);
            UpdateUI(unit.Name);
            DataManager.Save(FileNameConfig.CLICKERDATA, this.state); //Save the state of clickers after buying a new one
        }
    }

    int CalculateNewCost(int unitCost, float costMultiplier)
    {
        return unitCost = Mathf.RoundToInt(Mathf.Pow(unitCost, costMultiplier));
    }

    /// <summary>
    /// Check if a list that holds a certain type of Autoclickers exists
    /// </summary>
    /// <param name="clickerList"></param>
    /// <returns></returns>
    private bool ClickersExist(List<AutoClicker> clickerList)
    {
        bool clickersExist = false;
        if(clickerList.Count > 0)
        {
            clickersExist = true;
        }
        return clickersExist;
    }

    private AutoClicker LastClickerInList(string unitName)
    {
        return m_AutoClickers[unitName][m_AutoClickers[unitName].Count-1];
    }
    #endregion

    void UpdateUI(string clickerName)
    {
        
        m_ClickerShopTexts[clickerName][0].text = $"{clickerName}\n {LastClickerInList(clickerName).NextUnitCost} Gold";
        m_ClickerShopTexts[clickerName][1].text = $"x {m_AutoClickers[clickerName].Count}";
    }

    #region Data
    public void LoadState()
    {
        state = DataManager.Load<ClickerState>(FileNameConfig.CLICKERDATA, state);
        m_AutoClickers = state.AutoClickers;
        foreach (var clickers in m_AutoClickers)
        {
            if (clickers.Value.Count > 0)
            {
                UpdateUI(clickers.Value[0].Name);
            }
        }
    }

    void ResetClickers()
    {
        foreach (var clickers in m_AutoClickers)
        {
            if (clickers.Value.Count > 0) // Resets the UI and NextUnitCost
            {
                LastClickerInList(clickers.Key).NextUnitCost = clickers.Value[0].UnitCost;
                m_ClickerShopTexts[clickers.Key][0].text = $"{clickers.Key}\n {LastClickerInList(clickers.Key).NextUnitCost} Gold";
                m_ClickerShopTexts[clickers.Key][1].text = "x 0";
            }
            clickers.Value.Clear();
        }
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
    }
}