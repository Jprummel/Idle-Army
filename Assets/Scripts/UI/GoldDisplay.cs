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
        UpdateDisplay(GameManager.Instance.GoldManager.Gold);
    }

    public override void DeInitialize()
    {
        UIEvents.OnGoldUpdated += UpdateDisplay;
        base.DeInitialize();
    }

    private void UpdateDisplay(int goldValue)
    {
        m_GoldText.SetText($"{FormatCurrency(goldValue)}");

    }

    private string FormatCurrency(long value)
    {
        if (value == 0)
        {
            return "0";
        }

        string[] abbreviations = { "", "", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No" };

        int magnitude = Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(value)) / 3);

        if (magnitude == 0 || magnitude == 1)
        {
            return value.ToString();
        }

        float formattedValue = (float)value / Mathf.Pow(1000, magnitude);
        string abbreviation = abbreviations[magnitude];

        return $"{formattedValue:F2}{abbreviation}";
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
