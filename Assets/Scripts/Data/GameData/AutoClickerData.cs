using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class AutoClickerData : GameData
{
    [TitleGroup("AutoClicker","@m_AutoClickerName")]
    [HorizontalGroup("AutoClicker/Split", 0.5f, 0, 10)]
    [VerticalGroup("AutoClicker/Split/Left")]
    [BoxGroup("AutoClicker/Split/Left/Info")]
    [PreviewField(100), HideLabel]
    [HorizontalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup", 100)]
    [SerializeField] private Sprite m_AutoClickerSprite;
    [VerticalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup/Right"), LabelWidth(120)] [SerializeField] private AutoClickerTypes m_AutoClickerType;
    [VerticalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup/Right"), LabelWidth(120)] [SerializeField] private string m_AutoClickerName;
    [VerticalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup/Right"), LabelWidth(120)] [SerializeField, TextArea(2,3)] private string m_Description;
    [VerticalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup/Right"), LabelWidth(120)] [SerializeField] private string m_FlavourText;

    [VerticalGroup("AutoClicker/Split/Right")]
    [BoxGroup("AutoClicker/Split/Right/Data")]
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")][SerializeField] private float m_BaseDamage;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")][SerializeField] private float m_AttackSpeed;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")][SerializeField] private string m_AttackTimerID;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Cost")][SerializeField] private int m_BaseCost;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Cost")][SerializeField] private float m_CostMultiplier;

    public AutoClickerTypes AutoClickerType => m_AutoClickerType;
    public string Name => m_AutoClickerName;
    public string Description => FilteredDescription();
    public string FlavourText => m_FlavourText;
    public Sprite AutoClickerSprite => m_AutoClickerSprite;
    public float Damage => m_BaseDamage;
    public float AttackSpeed => m_AttackSpeed;
    public int BaseCost => m_BaseCost;
    public float CostMultiplier => m_CostMultiplier;
    public string AttackTimerID => m_AttackTimerID;


    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")]
    [Button]
    private void GenerateAttackTimerID()
    {
        m_AttackTimerID = $"{m_AutoClickerName}_Attack";
        m_AttackTimerID = m_AttackTimerID.ToUpper();
    }

    private string FilteredDescription()
    {
        string filteredDescription = m_Description;
        filteredDescription.Replace("DMG", $"{m_BaseDamage}");
        filteredDescription.Replace("SPEED", $"{m_AttackSpeed}");
        return filteredDescription;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(m_AttackTimerID == string.Empty)
        {
            GenerateAttackTimerID();
        }
    }
#endif
}