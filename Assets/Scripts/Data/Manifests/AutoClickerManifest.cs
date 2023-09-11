using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoClicker Manifest", menuName = "Idle Army/Manifests/AutoClicker Manifest", order = 0)]
public class AutoClickerManifest : BaseManifestData
{
    [SerializeField] private List<AutoClickerData> m_AllAutoClickers = new List<AutoClickerData>();

    public List<AutoClickerData> AllAutoClickers => m_AllAutoClickers;

    public AutoClickerData GetAutoClickerByType(AutoClickerTypes type)
    {
        AutoClickerData autoClicker = null;
        foreach (AutoClickerData clicker in m_AllAutoClickers)
        {
            if(clicker.AutoClickerType == type)
            {
                autoClicker = clicker;
            }
        }

        return autoClicker;
    }

    public override void GatherData()
    {
        base.GatherData();
        m_AllAutoClickers.Clear();
        foreach (var autoClicker in AssetDatabaseUtils.FindAssetsByType<AutoClickerData>())
        {
            m_AllAutoClickers.Add(autoClicker);
        }
    }
}