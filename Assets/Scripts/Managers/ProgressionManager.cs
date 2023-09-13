using Sirenix.OdinInspector;
using StardustInteractive.Saving;
using UnityEngine;

public class ProgressionManager : ManagerBase, ISaveable
{
    [SerializeField] private LevelManifest m_LevelManifest;
    [ShowInInspector, ReadOnly] private int m_CurrentLevel = 1;
    [ShowInInspector, ReadOnly] private int m_CurrentStage = 1;

    private int m_MaxStage = 10;


    public int CurrentLevel => m_CurrentLevel;
    public int CurrentStage => m_CurrentStage;

    public int MaxStage => m_MaxStage;
    public bool IsAtFinalStage => m_CurrentStage == m_MaxStage;

    public override void Initialize()
    {
        base.Initialize();
        GameEvents.OnProgressStage += Progress;
    }

    public override void DeInitialize()
    {
        GameEvents.OnProgressStage -= Progress;
        base.DeInitialize();
    }

    public LevelData CurrentLevelData()
    {
        return m_LevelManifest.GetLevelData(m_CurrentLevel);
    }

    public void Progress()
    {
        m_CurrentStage++;
        if (m_CurrentStage > m_MaxStage)
        {
            m_CurrentLevel++; //Start next level
            m_CurrentStage = 1; //First stage of next level

            GameEvents.ProgressLevel();
            UIEvents.ProgressLevel();
        }
        UIEvents.ProgressStage();
    }

    public void Save(string uniqueIdentifier, string saveFile)
    {
        ES3.Save($"Progression_Level_{uniqueIdentifier}", m_CurrentLevel, saveFile);
        ES3.Save($"Progression_Stage_{uniqueIdentifier}", m_CurrentStage, saveFile);
    }

    public void Load(string uniqueIdentifier, string saveFile)
    {
        if (ES3.KeyExists($"Progression_Level_{uniqueIdentifier}", saveFile))
        {
            m_CurrentLevel = ES3.Load<int>($"Progression_Level_{uniqueIdentifier}", saveFile);
        }

        if (ES3.KeyExists($"Progression_Stage_{uniqueIdentifier}", saveFile))
        {
            m_CurrentStage = ES3.Load<int>($"Progression_Stage_{uniqueIdentifier}", saveFile);
        }

        GameEvents.ProgressLevel();
        //GameEvents.ProgressStage();
        UIEvents.ProgressLevel();
        UIEvents.ProgressStage();
    }

    public void ResetData(string uniqueIdentifier, string saveFile)
    {
        m_CurrentLevel = 1;
        m_CurrentStage = 1;
    }
}
