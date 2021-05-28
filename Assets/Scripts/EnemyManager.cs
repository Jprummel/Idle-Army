using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class EnemyManager : SerializedMonoBehaviour
{
    [SerializeField,HideInInspector]
    private EnemyState state = new EnemyState(); 
    public static EnemyManager s_Instance;

    [OdinSerialize] private List<Enemy> m_Enemies = new List<Enemy>();
    [OdinSerialize]public Enemy CurrentEnemy
    {
        get { return this.state.CurrentEnemy; }
        set { this.state.CurrentEnemy = value; }
    }
    [SerializeField] private Transform m_Canvas;

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //LoadState();
    }

    public void CreateNewEnemy()
    {
        Enemy enemyToSpawn = m_Enemies[UnityEngine.Random.Range(0, m_Enemies.Count)];
        CurrentEnemy = Instantiate(enemyToSpawn, m_Canvas);
    }

    public void CreateSpecificEnemy(int enemyID)
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if(enemyID == m_Enemies[i].GetEnemyID())
            {
                Debug.Log(i);
                Enemy enemyToSpawn = Instantiate(m_Enemies[i], m_Canvas);
                CurrentEnemy = enemyToSpawn;
            }
        }
    }

    public void DefeatEnemy(GameObject enemy)
    {
        Destroy(enemy);
        if (GameManager.s_Instance.GetCurrentStage() != 4)
        {
            CreateNewEnemy();
        }
        else
        {
            CreateSpecificEnemy(3);
        }
    }

    private void LoadState()
    {
        state = DataManager.Load<EnemyState>(FileNameConfig.ENEMYDATA,state);
        CurrentEnemy = state.CurrentEnemy;
    }

    [Serializable]
    public class EnemyState
    {
        public Enemy CurrentEnemy;
    }
}