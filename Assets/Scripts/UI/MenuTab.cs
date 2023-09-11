using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MenuTab
{
    public event Action OnShow;
    public event Action OnHide;

    [SerializeField] private Button m_TabButton;
    [SerializeField] private GameObject m_TabContent;

    public Button TabButton => m_TabButton;

    [ButtonGroup("Visibility")]
    public void Show()
    {
        OnShow?.Invoke();
        m_TabContent.SetActive(true);
    }
    [ButtonGroup("Visibility")]
    public void Hide() 
    {
        OnHide?.Invoke();
        m_TabContent.SetActive(false);
    }

    public void RegisterEvents()
    {
        m_TabButton.onClick.AddListener(Show);
    }

    public void UnregisterEvents()
    {
        m_TabButton.onClick.RemoveListener(Show);
    }
}
