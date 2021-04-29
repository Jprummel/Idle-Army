using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickManager : MonoBehaviour
{
    //Auto clickers
    public List<float> m_VillagerAttackLastTime = new List<float>();
    public List<float> m_SquireAttackLastTime = new List<float>();
    public List<float> m_RangerAttackLastTime = new List<float>();
    public List<float> m_KnightAttackLastTime = new List<float>();

    //Auto Clicker Costs
    [SerializeField] private int m_VillagerCost;
    [SerializeField] private int m_SquireCost;
    [SerializeField] private int m_RangerCost;
    [SerializeField] private int m_KnightCost;

    //Auto Clicker UI
    [SerializeField] private TextMeshProUGUI m_VillagerAmountText;
    [SerializeField] private TextMeshProUGUI m_SquireAmountText;
    [SerializeField] private TextMeshProUGUI m_RangerAmountText;
    [SerializeField] private TextMeshProUGUI m_KnightAmountText;
    [SerializeField] private TextMeshProUGUI m_VillagerCostText;
    [SerializeField] private TextMeshProUGUI m_SquireCostText;
    [SerializeField] private TextMeshProUGUI m_RangerCostText;
    [SerializeField] private TextMeshProUGUI m_KnightCostText;


    private void Update()
    {
        //Villager Attack
        for (int i = 0; i < m_VillagerAttackLastTime.Count; i++)
        {
            if (Time.time - m_VillagerAttackLastTime[i] >= 1.0f)
            {
                m_VillagerAttackLastTime[i] = Time.time; //Sets last time of click to current time
                EnemyManager.s_Instance.CurrentEnemy.Damage(DamageManager.s_Instance.GetVillagerDamage());
            }
        }

        //Squire Attack
        for (int i = 0; i < m_SquireAttackLastTime.Count; i++)
        {
            if (Time.time - m_SquireAttackLastTime[i] >= 1.0f)
            {
                m_SquireAttackLastTime[i] = Time.time; //Sets last time of click to current time
                EnemyManager.s_Instance.CurrentEnemy.Damage(DamageManager.s_Instance.GetSquireDamage());
            }
        }

        //Ranger Attack
        for (int i = 0; i < m_RangerAttackLastTime.Count; i++)
        {
            if (Time.time - m_RangerAttackLastTime[i] >= 0.5f)
            {
                m_RangerAttackLastTime[i] = Time.time; //Sets last time of click to current time
                EnemyManager.s_Instance.CurrentEnemy.Damage(DamageManager.s_Instance.GetRangerDamage());
            }
        }

        //Knight Attack
        for (int i = 0; i < m_KnightAttackLastTime.Count; i++)
        {
            if (Time.time - m_KnightAttackLastTime[i] >= 1.0f)
            {
                m_KnightAttackLastTime[i] = Time.time; //Sets last time of click to current time
                EnemyManager.s_Instance.CurrentEnemy.Damage(DamageManager.s_Instance.GetKnightDamage());
            }
        }
    }

    public void OnBuyVillager()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_VillagerCost))
        {
            GameManager.s_Instance.TakeGold(m_VillagerCost);
            m_VillagerAttackLastTime.Add(Time.time);
            m_VillagerAmountText.text = $"x {m_VillagerAttackLastTime.Count}";
            m_VillagerCost = CalculateNewCost(m_VillagerCost, 1.15f);
            m_VillagerCostText.text = $"Villager\n{m_VillagerCost} Gold";
        }
    }

    public void OnBuySquire()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_SquireCost))
        {
            GameManager.s_Instance.TakeGold(m_SquireCost);
            m_SquireAttackLastTime.Add(Time.time);
            m_SquireAmountText.text = $"x {m_SquireAttackLastTime.Count}";
            m_SquireCost = CalculateNewCost(m_SquireCost, 1.13f);
            m_SquireCostText.text = $"Squire\n{m_SquireCost} Gold";
        }
    }

    public void OnBuyRanger()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_RangerCost))
        {
            GameManager.s_Instance.TakeGold(m_RangerCost);
            m_RangerAttackLastTime.Add(Time.time);
            m_RangerAmountText.text = $"x {m_RangerAttackLastTime.Count}";
            m_RangerCost = CalculateNewCost(m_RangerCost, 1.13f);
            m_RangerCostText.text = $"Ranger\n{m_RangerCost} Gold";
        }
    }

    public void OnBuyKnight()
    {
        if (GameManager.s_Instance.IsPurchasePossible(m_KnightCost))
        {
            GameManager.s_Instance.TakeGold(m_KnightCost);
            m_KnightAttackLastTime.Add(Time.time);
            m_KnightAmountText.text = $"x {m_KnightAttackLastTime.Count}";
            m_KnightCost = CalculateNewCost(m_KnightCost, 1.13f);
            m_KnightCostText.text = $"Knight\n{m_KnightCost} Gold";
        }
    }

    int CalculateNewCost(int unitCost, float costMultiplier)
    {
        
       return Mathf.RoundToInt(Mathf.Pow(unitCost, costMultiplier));
       
    }
}
