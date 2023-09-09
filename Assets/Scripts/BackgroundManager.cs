using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private LevelManifest m_LevelManifest;
    [SerializeField] private Image m_BackgroundImage;
    private LevelData m_CurrentLevelData;

    private void Awake()
    {
        GameEvents.OnProgressLevel += UpdateBackground;
    }

    private void OnDestroy()
    {
        GameEvents.OnProgressLevel -= UpdateBackground;
    }

    public void UpdateBackground()
    {
        m_CurrentLevelData = m_LevelManifest.GetLevelData(GameManager.Instance.ProgressionManager.CurrentLevel);
        m_BackgroundImage.sprite = m_CurrentLevelData.Background;
    }
}
