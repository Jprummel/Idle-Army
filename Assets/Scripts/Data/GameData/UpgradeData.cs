using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData : Unlockable
{
    [SerializeField] private string m_UpgradeName;
    [SerializeField] private string m_UpgradeDescription;
    [SerializeField] private Sprite m_UpgradeIcon;
    [SerializeField] private Heros m_Hero;
    [SerializeField] private UpgradeTypes m_UpgradeTypes;
    [SerializeField] private List<UnlockCondition> m_UnlockRequirements = new List<UnlockCondition>();
    [SerializeField] private int m_Cost;
    [Tooltip("Every 1 equals 100% so 1.5 would be a 150% modifier")]
    [SerializeField] private float m_Multiplier;

    public string UpgradeName => m_UpgradeName;
    public string UpgradeDescription => m_UpgradeDescription;
    public Sprite UpgradeIcon => m_UpgradeIcon;
    public Heros Hero => m_Hero;
    public UpgradeTypes UpgradeType => m_UpgradeTypes;
    public List<UnlockCondition> UnlockRequirements => m_UnlockRequirements;
    public int Cost => m_Cost;
    public float Multiplier => m_Multiplier;
}
