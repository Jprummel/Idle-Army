using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Idle Army/Enemies/Enemy", order = 0)]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string m_EnemyName;
    [SerializeField] private EnemyDifficulty m_Difficulty;
    [SerializeField] private Sprite m_EnemySprite;
    [SerializeField] private float m_BaseMaxHP;
    [SerializeField] private int m_BaseGoldReward;
    [SerializeField] private AnimationClip m_IdleAnim;
    [SerializeField] private AnimationClip m_DamageAnim;

    public string Name => m_EnemyName;
    public EnemyDifficulty Difficulty => m_Difficulty;
    public Sprite EnemySprite => m_EnemySprite;
    public float BaseMaxHP => m_BaseMaxHP;
    public int BaseGoldReward => m_BaseGoldReward;
    public AnimationClip IdleAnim => m_IdleAnim;
    public AnimationClip DamageAnim => m_DamageAnim;
}
