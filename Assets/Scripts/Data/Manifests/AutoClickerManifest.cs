using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoClicker Manifest", menuName = "Idle Army/Manifests/AutoClicker Manifest", order = 0)]
public class AutoClickerManifest : BaseManifestData
{
    [SerializeField] private List<AutoClicker> m_AllAutoClickers = new List<AutoClicker>();

    public List<AutoClicker> AllAutoClickers => m_AllAutoClickers;

    public AutoClicker GetAutoClickerByType(AutoClickerTypes type)
    {
        AutoClicker autoClicker = null;
        foreach (AutoClicker clicker in m_AllAutoClickers)
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
        foreach (var autoClicker in AssetDatabaseUtils.FindAssetsByType<AutoClicker>())
        {
            m_AllAutoClickers.Add(autoClicker);
        }
    }
}