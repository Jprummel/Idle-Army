using StardustInteractive.Tools;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Button m_SaveButton;
    [SerializeField] private Button m_LoadButton;
    [SerializeField] private Button m_ResetButton;

    private void Awake()
    {
        m_SaveButton.onClick.AddListener(SaveGame);
        m_LoadButton.onClick.AddListener(LoadGame);
        m_ResetButton.onClick.AddListener(ResetGameData);
    }

    private void OnDestroy()
    {
        m_SaveButton.onClick.RemoveListener(SaveGame);
        m_LoadButton.onClick.RemoveListener(LoadGame);
        m_ResetButton.onClick.RemoveListener(ResetGameData);
    }

    private void SaveGame()
    {
        GameManager.Instance.SaveManager.Save("Idle_Army_Save");
    }

    private void LoadGame()
    {
        GameManager.Instance.SaveManager.Load("Idle_Army_Save");
    }

    private void ResetGameData()
    {
        GameManager.Instance.SaveManager.ResetAllData("Idle_Army_Save");

        GameEvents.HerosChanged();
        GameEvents.UpgradeUnlocked();
        GameEvents.ProgressLevel();

        UIEvents.HerosChanged();
        UIEvents.UpgradeUnlocked();
        UIEvents.ProgressStage();
        UIEvents.ProgressLevel();
        UIEvents.GoldUpdated(GameManager.Instance.GoldManager.Gold);
        TimerManager.DisposeOfAllTimers();

    }
}
