using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class AutoClicker : SerializedScriptableObject
{
    public string Name;
    public int Damage;
    public float AttackCooldown;
    public float TimeTillAttack;

    public int UnitCost;
    public float CostMultiplier;
    public int NextUnitCost;

    public void Init(string name, int damage, float attackCooldown)
    {
        Name = name;
        Damage = damage;
        AttackCooldown = attackCooldown;
    }

    /// <summary>
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
    }
}