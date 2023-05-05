using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject fireBallSpawnPoint = null;
    [SerializeField] private float fireBallSpeed;
    private PlayerMovement pm;
    private Vector3 maxScale;
    private Vector3 targetDir;
    private SphereCollider sc;
    private Rigidbody rb;
    private bool isShot = false;

    private void Awake()
    {
        pm = GetComponentInParent<EnemyStateMachine>().pm;
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        maxScale = transform.localScale;
    }

    private void Update()
    {
        SetPos();
        SizeUp();
    }

    private void SizeUp()
    {
        if (maxScale.x * 2 < transform.localScale.x) return;
        transform.localScale += (Vector3.one * 1f * Time.deltaTime);
    }

    private void SetPos()
    {
        if (!isShot)
        transform.position = fireBallSpawnPoint.transform.position;
    }

    public void ShotFireBall()
    {
        targetDir = pm.gameObject.transform.position + transform.up * 0.8f - transform.position;
        isShot = true;
        sc.enabled = true;
        rb.velocity = targetDir.normalized * fireBallSpeed;
        Invoke(nameof(ResetFireBall), 1.5f);
    }

    public void ResetFireBall()
    {
        SoundManager.Instance.Stop("EnemyBAttackFire");
        transform.localScale = maxScale;
        isShot = false;
        sc.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<PlayerState>().UnderAttack(damage);
            //EffectManager.Instance.Play("MonsterFireballHit", _other.transform);
            Vector3 pos = transform.position;
            EffectManager.Instance.Play("MonsterFireballHit", pos, transform.rotation);
            ResetFireBall();
        }
    }
}
