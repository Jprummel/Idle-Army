using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using DG.Tweening;
using Animancer;
using TMPro;
using StardustInteractive.Saving;

public class EnemyManager : ManagerBase, ISaveable
{
    [SerializeField] private EnemyManifest m_EnemyManifest;
    [TabGroup("Tabs", "UI"), SerializeField] private Button m_EnemyButton;
    [TabGroup("Tabs", "UI"), SerializeField] private Image m_EnemyImage;
    [TabGroup("Tabs", "UI"), SerializeField] private Image m_HealthBarFill;
    [TabGroup("Tabs", "UI"), SerializeField] private TextMeshProUGUI m_HealthText;
    [TabGroup("Tabs", "UI"), SerializeField] private TextMeshProUGUI m_EnemyName;

    [TabGroup("Tabs", "Animation"), SerializeField] private AnimancerComponent m_Animancer;
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
    private float m_EnemyMaxHealth;
    private float m_EnemyCurrentHealth;
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
        EnemyData newEnemy = GameManager.Instance.ProgressionManager.CurrentLevelData().GetRandomEnemy();
        m_CurrentEnemy = newEnemy;
        m_EnemyMaxHealth = CalculateMaxHealth();
        m_GoldReward = CalculateGoldToGive();
        m_EnemyCurrentHealth = m_EnemyMaxHealth;

        SetupVisuals();
    }

    public void SpawnBoss()
    {
        if (GameManager.Instance.ProgressionManager.CurrentLevelData().Boss == null)
        {
            //Level has no boss, spawn regular enemy and return
            SpawnNewEnemy();
            return;
        }
        BossData newBoss = GameManager.Instance.ProgressionManager.CurrentLevelData().Boss;
        m_CurrentEnemy = newBoss;
        m_EnemyCurrentHealth = CalculateMaxHealth();
        m_GoldReward = CalculateGoldToGive();
        UIEvents.EnemySpawned(m_CurrentEnemy);
    }

    public void SpawnSpecificEnemy(EnemyData enemy)
    {

    }

    public void Damage(int damage)
    {
        m_EnemyCurrentHealth -= damage;
        UpdateHealthBar();

        if (m_DamageSequence != null && !m_DamageSequence.IsPlaying())
        {
            m_DamageSequence.Play();
        }
        if (m_EnemyCurrentHealth <= 0)
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
        GameEvents.ProgressStage();

        if (GameManager.Instance.ProgressionManager.IsAtFinalStage)
        {
            if(GameManager.Instance.ProgressionManager.CurrentLevelData().Boss != null)
            {
                SpawnBoss();
                return;
            }
            else
            {
                SpawnNewEnemy();
                return;
            }
        }
        SpawnNewEnemy();
    }

    private void SetupVisuals()
    {
        m_EnemyImage.sprite = m_CurrentEnemy.EnemySprite;
        UpdateHealthBar();
        m_EnemyName.text = m_CurrentEnemy.Name;
        if (m_CurrentEnemy.IdleAnim != null)
        {
            m_Animancer.Play(m_CurrentEnemy.IdleAnim);
        }
    }

    private void UpdateHealthBar()
    {
        m_HealthBarFill.DOFillAmount(m_EnemyCurrentHealth / m_EnemyMaxHealth, 0.1f).SetEase(Ease.OutExpo);
        m_HealthText.SetText($"{m_EnemyCurrentHealth} HP");

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

    public void Save(string uniqueIdentifier, string saveFile)
    {
        ES3.Save($"Enemy_{uniqueIdentifier}",m_CurrentEnemy, saveFile);
        ES3.Save($"Enemy_MaxHealth_{uniqueIdentifier}", m_EnemyMaxHealth, saveFile);
        ES3.Save($"Enemy_CurrentHealth_{uniqueIdentifier}", m_EnemyCurrentHealth, saveFile);
        ES3.Save($"Enemy_GoldReward_{uniqueIdentifier}", m_GoldReward, saveFile);
    }

    public void Load(string uniqueIdentifier, string saveFile)
    {
        if (ES3.KeyExists($"Enemy_{uniqueIdentifier}",saveFile))
        {
            m_CurrentEnemy = ES3.Load<EnemyData>($"Enemy_{uniqueIdentifier}", saveFile);
        }

        if (ES3.KeyExists($"Enemy_MaxHealth_{uniqueIdentifier}", saveFile))
        {
            m_EnemyMaxHealth = ES3.Load<float>($"Enemy_MaxHealth_{uniqueIdentifier}", saveFile);
        }

        if (ES3.KeyExists($"Enemy_CurrentHealth_{uniqueIdentifier}", saveFile))
        {
            m_EnemyCurrentHealth = ES3.Load<float>($"Enemy_CurrentHealth_{uniqueIdentifier}", saveFile);
        }

        if (ES3.KeyExists($"Enemy_GoldReward_{uniqueIdentifier}", saveFile))
        {
            m_GoldReward = ES3.Load<int>($"Enemy_GoldReward_{uniqueIdentifier}", saveFile);
        }

        SetupVisuals();
    }

    public void ResetData(string uniqueIdentifier, string saveFile)
    {
        
    }
}