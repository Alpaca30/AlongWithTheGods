using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour // 이 스크립트를 물지역 전체에 깔아두면됨.
{
    public delegate void PlayerSpawnManagerDelegate();
    private PlayerSpawnManagerDelegate playerSpawnManagerCallback = null;

    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform drowingPos;

    public GameObject player;

    private void Awake()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, startPos);
        player.transform.parent = null;
        //SoundManager.Instance.Play("FieldBGM3");
    }

    private void DrowingPlayerPos() // 물에 빠졌으니 물지역 시작부분으로 옮겨주기
    {
        player.transform.position = drowingPos.transform.position;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            DrowingPlayerPos();
            playerSpawnManagerCallback?.Invoke(); // 물에 빠졌다고 알려주기
        }
    }

    public void SetPlayerSpawnManagerDelegate(PlayerSpawnManagerDelegate _playerSpawnManagerCallback) // 물에 빠졌다고 알려주기
    {
        playerSpawnManagerCallback = _playerSpawnManagerCallback;
    }

}
