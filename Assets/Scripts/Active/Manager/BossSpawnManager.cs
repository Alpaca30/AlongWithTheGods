using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnManager : MonoBehaviour
{
    public delegate void BossSpawnDelegate();
    private BossSpawnDelegate bossSpawnCallback = null;

    [SerializeField] private BossStateMachine bossPrefab = null;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private DoorBox door3 = null;


    public GameObject boss;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            SpawnBoss();
            door3.SetTargetPosition();
            SoundManager.Instance.Play("BossBGM1");
            gameObject.SetActive(false);
        }
    }

    private void SpawnBoss()
    {
        boss = Instantiate(bossPrefab.gameObject, bossSpawnPoint.transform.position, Quaternion.identity);
        bossSpawnCallback?.Invoke();
    }

    public void SetBossSpawnDelegate(BossSpawnDelegate _bossSpawnCallback)
    {
        bossSpawnCallback = _bossSpawnCallback;
    }
}