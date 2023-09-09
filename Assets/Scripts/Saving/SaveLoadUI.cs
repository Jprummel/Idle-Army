using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Button m_SaveButton;
    [SerializeField] private Button m_LoadButton;

    private void Awake()
    {
        m_SaveButton.onClick.AddListener(SaveGame);
        m_LoadButton.onClick.AddListener(LoadGame);
    }

    private void OnDestroy()
    {
        m_SaveButton.onClick.RemoveListener(SaveGame);
        m_LoadButton.onClick.RemoveListener(LoadGame);
    }

    private void SaveGame()
    {
        GameManager.Instance.SaveManager.Save("Idle_Army_Save");
    }

    private void LoadGame()
    {
        GameManager.Instance.SaveManager.Load("Idle_Army_Save");
    }
}
