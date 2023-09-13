using log4net.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class UpgradeButton : MonoBehaviour
{
    public event Action<UpgradeData> OnSelectUpgrade;

    [SerializeField] private Button m_button;
    [SerializeField] private UpgradeData m_UpgradeData;
    [SerializeField] private Image m_UpgradeIcon;

    private void Awake()
    {
        m_button.onClick.AddListener(SelectUpgrade);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(SelectUpgrade);
    }

    private void SelectUpgrade()
    {
        OnSelectUpgrade?.Invoke(m_UpgradeData);
    }

    void SetupUI()
    {
        m_UpgradeIcon.sprite = m_UpgradeData.UpgradeIcon;
    }

    public void SetUpgradeData(UpgradeData upgrade)
    {
        m_UpgradeData = upgrade;
        SetupUI();
    }
}
