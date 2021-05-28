using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System; 

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;

    public delegate void Reset();
    public static Reset s_OnResetGame;

    [SerializeField,HideInInspector]
    private GameState state = new GameState();

    //Player stats
    [ShowInInspector] private int m_Level
    {
        get { return this.state.Level; }
        set { this.state.Level = value; }
    }
    [ShowInInspector] private int m_Stage
    {
        get { return this.state.Stage; }
        set { this.state.Stage = value; }
    }
    [SerializeField, HideInInspector] private int m_MaxStage = 5;
    [ShowInInspector] private int m_Gold
    {
        get { return this.state.Gold; }
        set { this.state.Gold = value; }
    }

    //UI
    [TabGroup("HUD")] [SerializeField] private TextMeshProUGUI m_LevelText;
    [TabGroup("HUD")] [SerializeField] private TextMeshProUGUI m_StageText;
    [TabGroup("HUD")] [SerializeField] private TextMeshProUGUI m_GoldText;

    //Background
    [TabGroup("Background")] [SerializeField] private Image m_BackgroundImage;
    [TabGroup("Background")] [SerializeField] private Sprite[] m_Backgrounds;
    [SerializeField,HideInInspector] private int m_CurrentBackground
    {
        get { return this.state.CurrentBackground; }
        set { this.state.CurrentBackground = value; }
    }

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
        LoadState();
        s_OnResetGame += ResetGameData;
    }

    private void Start()
    {
        
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
        DataManager.Save(FileNameConfig.GAMEDATA, this.state);
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
        DataManager.Save(FileNameConfig.GAMEDATA, this.state);
    }

    private void LoadState()
    {
        this.state = DataManager.Load<GameState>(FileNameConfig.GAMEDATA, this.state);
        if (state != null)
        {
            m_Level = state.Level;
            m_Stage = state.Stage;
            m_Gold = state.Gold;
            m_CurrentBackground = state.CurrentBackground;
        }

        //UI & Background
        m_LevelText.text = $"Level {m_Level}";
        m_StageText.text = $"Stage {m_Stage}/{m_MaxStage}";
        m_BackgroundImage.sprite = m_Backgrounds[m_CurrentBackground];
        m_GoldText.text = $"Gold: {m_Gold}";
        //EnemyManager.s_Instance.CurrentEnemy = GameObject.FindObjectOfType<Enemy>();
    }

    // Starts a new game
    private void ResetGameData()
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
        DataManager.Save(FileNameConfig.GAMEDATA, this.state);
    }

    public void ResetGame()
    {
        s_OnResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        s_OnResetGame -= ResetGameData;
    }

    [Serializable]
    public class GameState
    {
        public int Level;
        public int Stage;
        public int Gold;
        public int CurrentBackground;
    }
}