using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public delegate void BossLaserDelegate();
    private BossLaserDelegate bossLaserCallback = null;

    [SerializeField] private Transform center = null;
    [SerializeField] private float speed = 10.0f;
    public bool isRotating = true;
    public float rotateDir = -1;
    private int damage = 1;
    public float timer;

    private void Update()
    {
        RotateLaser();
    }

    private void RotateLaser()
    {
        if (isRotating)
        {
            timer += Time.deltaTime;
            transform.RotateAround(center.position, Vector3.forward * rotateDir, speed * Time.deltaTime);
            if (speed * timer >= 180f) // 스피드 * Time.deltaTime = 한바퀴 도는데 걸리는 시간. 스피드가 60일때 Time.deltaTime이 6초가 지나면 한바퀴를 돔
            {
                isRotating = false;
                bossLaserCallback?.Invoke();
                timer = 0f;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<PlayerState>().LaserUnderAttack(damage);
        }
    }

    public void SetLaserOffDelegate(BossLaserDelegate _bossLaserCallback)
    {
        bossLaserCallback = _bossLaserCallback;
    }
}

