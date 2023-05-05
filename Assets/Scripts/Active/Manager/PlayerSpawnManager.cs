using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour // �� ��ũ��Ʈ�� ������ ��ü�� ��Ƶθ��.
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

    private void DrowingPlayerPos() // ���� �������� ������ ���ۺκ����� �Ű��ֱ�
    {
        player.transform.position = drowingPos.transform.position;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            DrowingPlayerPos();
            playerSpawnManagerCallback?.Invoke(); // ���� �����ٰ� �˷��ֱ�
        }
    }

    public void SetPlayerSpawnManagerDelegate(PlayerSpawnManagerDelegate _playerSpawnManagerCallback) // ���� �����ٰ� �˷��ֱ�
    {
        playerSpawnManagerCallback = _playerSpawnManagerCallback;
    }

}
