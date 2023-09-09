using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using DG.Tweening;

public class EnemyManager : ManagerBase
{
    [SerializeField] private EnemyManifest m_EnemyManifest;
    [TabGroup("Tabs", "UI"), SerializeField] private Button m_EnemyButton;
    [TabGroup("Tabs", "UI"), SerializeField] private Image m_EnemyImage;
    [TabGroup("Tabs", "UI"), SerializeField] private Image m_HealthBarFill;

    [TabGroup("Tabs", "Animation"), SerializeField] private float m_ShakeDuration;
    [TabGroup("Tabs", "Animation"), SerializeField] private float m_ShakeStrength;
    [TabGroup("Tabs", "Animation"), SerializeField] private int m_ShakeVibrato;
    [TabGroup("Tabs", "Animation"), SerializeField] private float m_ShakeRandomness;
    [TabGroup("Tabs", "Animation"), SerializeField] private Color32 m_DamagedColor;
    [TabGroup("Tabs", "Animation"), SerializeField] private float m_ColorSwapDuration;
    private Color32 m_DefaultColor;
    private Sequence m_DamageSequence => DamageAnimation();

    //EnemyData
    private EnemyData m_CurrentEnemy;
    private float m_MaxHealth;
    private float m_EnemyCurrentHP;
    private int m_GoldReward;

    private Vector3 m_originalPosition;

    public override void Initialize()
    {
        base.Initialize();
        m_EnemyButton.onClick.AddListener(ClickDamage);
        SpawnNewEnemy();
        m_originalPosition = m_EnemyImage.transform.localPosition;
        m_DefaultColor = m_EnemyImage.color;
    }


    public void SpawnNewEnemy()
    {
        EnemyData newEnemy = m_EnemyManifest.GetRandomEnemy();
        m_CurrentEnemy = newEnemy;
        m_MaxHealth = CalculateMaxHealth();
        m_GoldReward = CalculateGoldToGive();
        m_EnemyCurrentHP = m_MaxHealth;

        SetupVisuals();
    }

    public void SpawnBoss()
    {
        BossData newBoss = m_EnemyManifest.GetBossByLevel(GameManager.Instance.ProgressionManager.CurrentLevel);
        m_CurrentEnemy = newBoss;
        m_EnemyCurrentHP = CalculateMaxHealth();
        m_GoldReward = CalculateGoldToGive();
        UIEvents.EnemySpawned(m_CurrentEnemy);
    }

    public void SpawnSpecificEnemy(EnemyData enemy)
    {

    }

    public void Damage(int damage)
    {
        m_EnemyCurrentHP -= damage;
        UpdateHealthBar();

        if (m_DamageSequence != null && !m_DamageSequence.IsPlaying())
        {
            m_DamageSequence.Play();
        }
        if (m_EnemyCurrentHP <= 0)
        {
            if (m_DamageSequence.IsPlaying())
            {
                m_DamageSequence.Kill();
            }

            OnEnemyKilled();
        }
    }

    private void ClickDamage()
    {
        Damage(1);
    }

    private Sequence DamageAnimation()
    {
        Sequence damageSequence = DOTween.Sequence();
        damageSequence.Append(m_EnemyImage.transform.DOShakePosition(m_ShakeDuration, m_ShakeStrength, m_ShakeVibrato, m_ShakeRandomness)).OnComplete(() => m_EnemyImage.transform.localPosition = m_originalPosition);
        damageSequence.Join(m_EnemyImage.DOColor(m_DamagedColor, m_ColorSwapDuration)).OnComplete(() => m_EnemyImage.DOColor(m_DefaultColor, m_ColorSwapDuration));
        return damageSequence;
    }

    public void OnEnemyKilled()
    {
        GameManager.Instance.Wallet.AddGold(m_GoldReward);
        SpawnNewEnemy();
        GameEvents.ProgressStage();
    }

    private void SetupVisuals()
    {
        m_EnemyImage.sprite = m_CurrentEnemy.EnemySprite;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        m_HealthBarFill.fillAmount = m_EnemyCurrentHP / m_MaxHealth;
    }

    int CalculateGoldToGive()
    {
        return m_CurrentEnemy.BaseGoldReward + Mathf.RoundToInt((Mathf.Pow(GameManager.Instance.ProgressionManager.CurrentLevel + 1, 1.25f)));
    }

    float CalculateMaxHealth()
    {
        return m_CurrentEnemy.BaseMaxHP + Mathf.RoundToInt((Mathf.Pow(GameManager.Instance.ProgressionManager.CurrentLevel + 1, 2.25f)));
    }

    public override void DeInitialize()
    {
        m_EnemyButton.onClick.RemoveListener(ClickDamage);
        base.DeInitialize();
    }
}