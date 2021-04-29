using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager s_Instance;

    private int m_VillagerBaseDamage = 1;
    private float m_VillagerDamageModifier = 1;
    private int m_SquireBaseDamage = 7;
    private float m_SquireDamageModifier = 1;
    private int m_RangerBaseDamage = 5;
    private float m_RangerDamageModifier = 1;
    private int m_KnightBaseDamage = 15;
    private float m_KnightDamageModifier = 1;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    int CalculateDamage(int baseDamage, float damageModifier)
    {
        return Mathf.RoundToInt(baseDamage * damageModifier);
    }

    public int GetVillagerDamage()
    {
        return CalculateDamage(m_VillagerBaseDamage, m_VillagerDamageModifier);
    }

    public int GetSquireDamage()
    {
        return CalculateDamage(m_SquireBaseDamage, m_SquireDamageModifier);
    }

    public int GetRangerDamage()
    {
        return CalculateDamage(m_RangerBaseDamage, m_RangerDamageModifier);
    }

    public int GetKnightDamage()
    {
        return CalculateDamage(m_KnightBaseDamage, m_KnightDamageModifier);
    }
}
