using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using StardustInteractive.Saving;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void Reset();
    public static Reset s_OnResetGame;


    //Managers
    [ShowInInspector, ReadOnly] private AutoclickerManager m_AutoClickManager;
    [ShowInInspector, ReadOnly] private EnemyManager m_EnemyManager;
    [ShowInInspector, ReadOnly] private GoldManager m_GoldManager;
    [ShowInInspector, ReadOnly] private ProgressionManager m_ProgressionManager;
    [ShowInInspector, ReadOnly] private SavingSystem m_SaveManager;

    public AutoclickerManager AutoClickManager => m_AutoClickManager;
    public EnemyManager EnemyManager => m_EnemyManager;
    public GoldManager Wallet => m_GoldManager;
    public ProgressionManager ProgressionManager => m_ProgressionManager;
    public SavingSystem SaveManager => m_SaveManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        GameEvents.GameManagerInitialized();
        InitializeManagers();
    }

    public void ResetGame()
    {
        s_OnResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void InitializeManagers()
    {
        m_AutoClickManager.Initialize();
        m_EnemyManager.Initialize();
        m_GoldManager.Initialize();
        m_ProgressionManager.Initialize();
    }

    private void DeInitializeManagers()
    {
        m_AutoClickManager.DeInitialize();
        m_EnemyManager.DeInitialize();
        m_GoldManager.DeInitialize();
        m_ProgressionManager.DeInitialize();
    }

    private void OnDestroy()
    {
        DeInitializeManagers();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_AutoClickManager == null)
        {
            if (FindObjectOfType<AutoclickerManager>() != null)
            {
                m_AutoClickManager = FindObjectOfType<AutoclickerManager>();
            }
        }

        if(m_GoldManager == null)
        {
            if(FindObjectOfType<GoldManager>() != null)
            {
                m_GoldManager = FindObjectOfType<GoldManager>();
            }
        }

        if(m_EnemyManager == null)
        {
            if(FindObjectOfType<EnemyManager>() != null)
            {
                m_EnemyManager = FindObjectOfType<EnemyManager>();
            }
        }

        if(m_ProgressionManager == null)
        {
            if (FindObjectOfType<ProgressionManager>() != null)
            {
                m_ProgressionManager = FindObjectOfType<ProgressionManager>();
            }
        }

        if(m_SaveManager == null)
        {
            if(FindObjectOfType<SavingSystem>() != null)
            {
                m_SaveManager = FindObjectOfType<SavingSystem>();
            }
        }
    }
#endif
}