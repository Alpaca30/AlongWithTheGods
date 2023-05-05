using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private int damage;
    [SerializeField] private float initialSpeed = 10f; // �ʱ� �ӵ�
    [SerializeField] private float acceleration = 5f; // ���ӵ�
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.velocity = Vector3.down * initialSpeed;
    }

    private void Update()
    {
        rb.velocity += Vector3.down * acceleration * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Platform"))
        {
            SoundManager.Instance.PlayOneShot("BossMeteor");
            //������ ����Ʈ �־����.
            Vector3 pos = transform.position;
            pos.y -= .8f;
            EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Hit, Effect.EAttackType.Magic, pos, transform.rotation);
            Destroy(gameObject);
        }
        else if (_other.CompareTag("Player"))
        {
            _other.GetComponent<PlayerState>().UnderAttack(damage);
            SoundManager.Instance.PlayOneShot("BossMeteor");
            //������ ����Ʈ �־����.
            Vector3 pos = transform.position;
            pos.y -= 2f;
            EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Hit, Effect.EAttackType.Magic, pos, transform.rotation);
            Destroy(gameObject);
        }
    }
}
