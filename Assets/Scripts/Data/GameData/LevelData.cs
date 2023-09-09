using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Idle Army/Level Data", order = 0)]
public class LevelData : SerializedScriptableObject
{
    [SerializeField] private string m_LevelName;
    [SerializeField] private Sprite m_Background;

    public string LevelName => m_LevelName;
    public Sprite Background => m_Background;
}
