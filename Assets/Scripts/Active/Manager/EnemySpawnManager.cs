using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public delegate void EnemySpawnDelegate();
    private EnemySpawnDelegate enemySpawnCallback = null;

    [SerializeField] private GameObject enemyAPrefab = null;
    [SerializeField] private GameObject enemyBPrefab = null;
    [SerializeField] private GameObject enemyCPrefab = null;

    [Header("# Stage")]
    [SerializeField] private Stage1 stage1 = null;
    [SerializeField] private Stage2 stage2 = null;
    [SerializeField] private Stage3 stage3 = null;
    [SerializeField] private DoorTrigger doorTrigger = null;

    [Header("# Enemy")]
    [SerializeField] private GameObject[] enemySpawnPoint1 = null;
    [SerializeField] private GameObject[] enemySpawnPoint2 = null;
    [SerializeField] private GameObject[] enemySpawnPoint3 = null;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> enemies2 = new List<GameObject>();
    private List<GameObject> prefabs = new List<GameObject>();


    public void Init()
    {
        stage1.SetStage1Delegate(SpawnEnemy1);
        stage2.SetStage2Delegate(SpawnEnemy2);
        stage3.SetStage3Delegate(SpawnEnemy3);
        prefabs.Add(enemyAPrefab);
        prefabs.Add(enemyBPrefab);
        prefabs.Add(enemyCPrefab);
    }
  
    private void Update()
    {
        RemoveEnemy();
    }

    public void SpawnEnemy1()
    {
        for (int i = 0; i < enemySpawnPoint1.Length; ++i)
        {
            int random = Random.Range(0, 3);
            GameObject enemy = Instantiate(prefabs[random], enemySpawnPoint1[i].transform.position, Quaternion.identity);
            enemies.Add(enemy);
        }
        enemySpawnCallback?.Invoke();
    }

    public void SpawnEnemy2()
    {
        for (int i = 0; i < enemySpawnPoint2.Length; ++i)
        {
            int random = Random.Range(0, 3);
            GameObject enemy = Instantiate(prefabs[random], enemySpawnPoint2[i].transform.position, Quaternion.identity);
            enemies2.Add(enemy);
        }
        enemySpawnCallback?.Invoke();
    }

    public void SpawnEnemy3()
    {
        for (int i = 0; i < enemySpawnPoint3.Length; ++i)
        {
            int random = Random.Range(0, 3);
            GameObject enemy = Instantiate(prefabs[random], enemySpawnPoint3[i].transform.position, Quaternion.identity);
            enemies.Add(enemy);
        }
        enemySpawnCallback?.Invoke();
    }

    private void RemoveEnemy()
    {
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i].GetComponent<EnemyStateMachine>().currentEnemyState == enemies[i].GetComponent<EnemyStateMachine>().enemyDead)
            {
                enemies.Remove(enemies[i]);
            }
        }
        for (int i = 0; i < enemies2.Count; ++i)
        {
            if (enemies2[i].GetComponent<EnemyStateMachine>().currentEnemyState == enemies2[i].GetComponent<EnemyStateMachine>().enemyDead)
            {
                enemies2.Remove(enemies2[i]);
            }
            if (enemies2.Count == 0)
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        if (enemies2.Count == 0)
        {
            for (int i = 0; i < doorTrigger.doorBox.Length; ++i) 
            {
                doorTrigger.doorBox[i].SetStartPos();
            }
        }
    }

    public void SetSpawnDelegate(EnemySpawnDelegate _enemySpawnCallback)
    {
        enemySpawnCallback = _enemySpawnCallback;
    }
}
