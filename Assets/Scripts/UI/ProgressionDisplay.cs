using TMPro;
using UnityEngine;

public class ProgressionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_LevelText;
    [SerializeField] private TextMeshProUGUI m_StageText;

    private void Awake()
    {
        UIEvents.OnProgressStage += UpdateStageText;
        UIEvents.OnProgressLevel += UpdateLevelText;
    }

    private void Start()
    {
        UpdateLevelText();
        UpdateStageText();
    }

    private void OnDestroy()
    {
        UIEvents.OnProgressStage -= UpdateStageText;
        UIEvents.OnProgressLevel -= UpdateLevelText;
    }

    private void UpdateLevelText()
    {
        m_LevelText.SetText($"{GameManager.Instance.ProgressionManager.CurrentLevelData().LevelName} Lvl: {GameManager.Instance.ProgressionManager.CurrentLevel}");
    }

    private void UpdateStageText()
    {
        m_StageText.SetText($"{GameManager.Instance.ProgressionManager.CurrentStage}/{GameManager.Instance.ProgressionManager.MaxStage}");
    }

}
