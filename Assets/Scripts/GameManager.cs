using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;

    //Player stats
    private int m_Level = 1;
    private int m_Stage = 1;
    private int m_MaxStage = 5;
    private int m_Gold;
    
    //UI
    [SerializeField] private TextMeshProUGUI m_LevelText;
    [SerializeField] private TextMeshProUGUI m_StageText;
    [SerializeField] private TextMeshProUGUI m_GoldText;

    //Background
    [SerializeField] private Image m_BackgroundImage;
    [SerializeField] private Sprite[] m_Backgrounds;
    private int m_CurrentBackground;
    private int m_EnemiesUntilBackgroundChange;

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
        m_EnemiesUntilBackgroundChange = 5;
    }

    public void AddGold(int amount)
    {
        m_Gold += amount;
        m_GoldText.text = $"Gold: {m_Gold}";
    }

    public void TakeGold(int amount)
    {
        m_Gold -= amount;
        m_GoldText.text = $"Gold: {m_Gold}";
    }

    public bool IsPurchasePossible(int cost)
    {
        if (cost <= m_Gold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetCurrentLevel()
    {
        return m_Level;
    }

    public int GetCurrentStage()
    {
        return m_Stage;
    }

    public void NextStage()
    {
        m_Stage++;
        if (m_Stage > m_MaxStage)
        {
            m_Level++; //Start next level
            m_Stage = 1; //First stage of next level
            m_LevelText.text = $"Level {m_Level}";

            //Change background
            m_CurrentBackground++;
            if (m_CurrentBackground == m_Backgrounds.Length)
            {
                m_CurrentBackground = 0; // Reset to 0 if end of background array is reached
            }
            m_BackgroundImage.sprite = m_Backgrounds[m_CurrentBackground];
        }
        m_StageText.text = $"Stage {m_Stage}/{m_MaxStage}";
    }

    // Starts a new game
    public void ResetGame()
    {
        //Destroys current enemy and creates a new one
        Destroy(EnemyManager.s_Instance.CurrentEnemy.gameObject);
        EnemyManager.s_Instance.CreateNewEnemy();
        m_CurrentBackground = 0;

        m_Level = 1;
        m_Stage = 1;
        m_Gold = 0;

        m_GoldText.text = $"Gold: {m_Gold}";
        m_LevelText.text = $"Level {m_Level}";
        m_StageText.text = $"Stage {m_Stage}/{m_MaxStage}";
        m_BackgroundImage.sprite = m_Backgrounds[m_CurrentBackground];
    }
}