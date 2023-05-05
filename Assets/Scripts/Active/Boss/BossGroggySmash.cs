using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroggySmash : MonoBehaviour
{
    private int damage = 1;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<PlayerState>().UnderAttack(damage);
            Vector3 pos = _other.transform.position;
            pos.y = 7.05f;
            EffectManager.Instance.Play("BossGroggySmash", pos, _other.transform.rotation);
        }
    }
}
