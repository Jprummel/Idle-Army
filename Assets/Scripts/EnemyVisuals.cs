using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisuals : BaseObject
{
    [SerializeField] private Image m_EnemyImage;
    [SerializeField] private Image m_HealthBarFill;

    public override void Initialize()
    {
        UIEvents.OnEnemySpawned += SetupVisuals;
        base.Initialize();
    }

    public override void DeInitialize()
    {
        UIEvents.OnEnemySpawned -= SetupVisuals;
        base.DeInitialize();
    }

    public void SetupVisuals(EnemyData enemy)
    {
        m_EnemyImage.sprite = enemy.EnemySprite;
        m_HealthBarFill.fillAmount = 1;
    }
}
