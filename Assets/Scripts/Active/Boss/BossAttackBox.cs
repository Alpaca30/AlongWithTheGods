using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBox : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<PlayerState>().UnderAttack(damage);
        }
    }
}
