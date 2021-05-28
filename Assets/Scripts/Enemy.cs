using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int m_EnemyID;
    [SerializeField] private float m_MaxHP;
    [SerializeField,HideInInspector]private float m_CurrentHP;
    [SerializeField] private int m_GoldToGive;
    [SerializeField] private Image m_HealthBarFill;
    [SerializeField] private Animation m_DamageAnim;

    private void Start()
    {
        if (GameManager.s_Instance.GetCurrentLevel() != 1)
        {
            CalculateMaxHealth();
            CalculateGoldToGive();
        }
        m_CurrentHP = m_MaxHP;
    }

    public int GetEnemyID()
    {
        return m_EnemyID;
    }

    public void Damage(int damage)
    {
        m_CurrentHP -= damage;
        m_HealthBarFill.fillAmount = m_CurrentHP / m_MaxHP;
        m_DamageAnim.Stop();
        m_DamageAnim.Play();
        if(m_CurrentHP <= 0)
        {
            Defeated();
        }
    }

    public void Defeated()
    {
        GameManager.s_Instance.AddGold(m_GoldToGive);
        EnemyManager.s_Instance.DefeatEnemy(this.gameObject);
        GameManager.s_Instance.NextStage();
    }

    int CalculateGoldToGive()
    {
        return m_GoldToGive = m_GoldToGive + Mathf.RoundToInt((Mathf.Pow(GameManager.s_Instance.GetCurrentLevel(), 1.25f)));
    }

    float CalculateMaxHealth()
    {
        return m_MaxHP = m_MaxHP + Mathf.RoundToInt((Mathf.Pow(GameManager.s_Instance.GetCurrentLevel(), 2.25f)));
    }
}