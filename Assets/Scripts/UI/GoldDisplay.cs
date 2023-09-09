using TMPro;
using UnityEngine;

public class GoldDisplay : BaseObject
{
    [SerializeField] private TextMeshProUGUI m_GoldText;

    public override void Initialize()
    {
        UIEvents.OnGoldUpdated += UpdateDisplay;
        base.Initialize();
    }

    public override void PostInitialize()
    {
        base.PostInitialize();
        UpdateDisplay(GameManager.Instance.Wallet.Gold);
    }

    public override void DeInitialize()
    {
        UIEvents.OnGoldUpdated += UpdateDisplay;
        base.DeInitialize();
    }

    private void UpdateDisplay(int goldValue)
    {
        m_GoldText.SetText($"{goldValue}");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_GoldText == null)
        {
            m_GoldText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
#endif
}
