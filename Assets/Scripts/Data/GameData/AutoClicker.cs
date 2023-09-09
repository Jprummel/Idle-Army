using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class AutoClicker : SerializedScriptableObject
{
    [SerializeField] private AutoClickerTypes m_AutoClickerType;
    [SerializeField] private string m_AutoClickerName;
    [SerializeField] private string m_Description;
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_AttackSpeed;

    [SerializeField] private int m_BaseCost;
    [SerializeField] private float m_CostMultiplier;
    [SerializeField] private string m_AttackTimerID;

    public AutoClickerTypes AutoClickerType => m_AutoClickerType;
    public string Name => m_AutoClickerName;
    public string Description => m_Description;
    public float Damage => m_Damage;
    public float AttackSpeed => m_AttackSpeed;
    public int BaseCost => m_BaseCost;
    public float CostMultiplier => m_CostMultiplier;
    public string AttackTimerID => m_AttackTimerID;

    [Button]
    private void GenerateAttackTimerID()
    {
        m_AttackTimerID = $"{m_AutoClickerName}_Attack";
        m_AttackTimerID = m_AttackTimerID.ToUpper();
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

    /*    public void Init(string name, int damage, float attackCooldown)
        {
            Name = name;
            Damage = damage;
            AttackCooldown = attackCooldown;
        }*/

    /*    /// <summary>
        /// Checks if unit is ready to attack. If so deal damage, if not do nothing
        /// </summary>
        public void Attack()
        {
            TimeTillAttack -= Time.deltaTime;
            if (TimeTillAttack <= 0)
            {
                EnemyManager.s_Instance.CurrentEnemy.Damage(Damage);
                TimeTillAttack = AttackCooldown;
            }
        }*/
}