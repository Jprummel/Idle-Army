using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_EnemyPrefabs;
    public Enemy CurrentEnemy;
    [SerializeField] private Transform m_Canvas;

    public static EnemyManager s_Instance;

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
    }

    public void CreateNewEnemy()
    {
        GameObject enemyToSpawn = m_EnemyPrefabs[Random.Range(0, m_EnemyPrefabs.Length)];

        GameObject obj = Instantiate(enemyToSpawn, m_Canvas);

        CurrentEnemy = obj.GetComponent<Enemy>();
    }

    public void DefeatEnemy(GameObject enemy)
    {
        Destroy(enemy);
        CreateNewEnemy();
    }
}
