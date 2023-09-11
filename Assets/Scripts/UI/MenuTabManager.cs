using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTabManager : BaseObject
{
    [SerializeField] private List<MenuTab> m_Tabs = new List<MenuTab>();


    private void Awake()
    {
        foreach (MenuTab tab in m_Tabs)
        {
            tab.RegisterEvents();
            tab.OnShow += HideAllTabs;
        }
    }

    private void HideAllTabs()
    {
        foreach(MenuTab tab in m_Tabs)
        {
            tab.Hide();
        }
    }

    private void OnDestroy()
    {
        foreach (MenuTab tab in m_Tabs)
        {
            tab.UnregisterEvents();
            tab.OnShow -= HideAllTabs;
        }
    }

}
