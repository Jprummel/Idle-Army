using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class HeroData : GameData
{
    [TitleGroup("AutoClicker","@m_AutoClickerName")]
    [HorizontalGroup("AutoClicker/Split", 0.5f, 0, 10)]
    [VerticalGroup("AutoClicker/Split/Left")]
    [BoxGroup("AutoClicker/Split/Left/Info")]
    [PreviewField(100), HideLabel]
    [HorizontalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup", 100)]
    [SerializeField] private Sprite m_Sprite;
    [VerticalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup/Right"), LabelWidth(120)] 
    [OnValueChanged("GenerateAttackTimerID")][SerializeField] private Heros m_Hero;
    [VerticalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup/Right"), LabelWidth(120)] [SerializeField, TextArea(2,3)] private string m_Description;
    [VerticalGroup("AutoClicker/Split/Left/Info/AutoClicker Setup/Right"), LabelWidth(120)] [SerializeField] private string m_FlavourText;

    [VerticalGroup("AutoClicker/Split/Right")]
    [BoxGroup("AutoClicker/Split/Right/Data")]
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")][SerializeField] private float m_BaseDamage;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")][SerializeField] private float m_AttackSpeed;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")][SerializeField] private string m_AttackTimerID;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Cost")][SerializeField] private int m_BaseCost;
    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Cost")][SerializeField] private float m_CostMultiplier;

    public Heros Hero => m_Hero;
    public string Name => GetCleanedName();
    public string Description => FilteredDescription();
    public string FlavourText => m_FlavourText;
    public Sprite Sprite => m_Sprite;
    public float Damage => m_BaseDamage;
    public float AttackSpeed => m_AttackSpeed;
    public int BaseCost => m_BaseCost;
    public float CostMultiplier => m_CostMultiplier;
    public string AttackTimerID => m_AttackTimerID;


    [TabGroup("AutoClicker/Split/Right/Data/Tabs", "Stats")]
    [Button]
    private void GenerateAttackTimerID()
    {
        m_AttackTimerID = $"{m_Hero}_Attack";
        m_AttackTimerID = m_AttackTimerID.ToUpper();
    }

    private string FilteredDescription()
    {
        string filteredDescription = m_Description;
        filteredDescription.Replace("DMG", $"{m_BaseDamage}");
        filteredDescription.Replace("SPEED", $"{m_AttackSpeed}");
        return filteredDescription;
    }

    private string GetCleanedName()
    {
        return m_Hero.ToString().Replace("_", " ");
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