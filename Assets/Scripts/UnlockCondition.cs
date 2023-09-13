using System;
using UnityEngine;

[Serializable]
public struct UnlockCondition
{
    [SerializeField] private Heros m_Hero;
    [SerializeField] private int m_RequiredLevel;

    public Heros Hero => m_Hero;
    public int RequiredLevel => m_RequiredLevel;
}
