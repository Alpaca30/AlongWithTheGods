using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossParts : MonoBehaviour
{
    [SerializeField] private BossControl bc = null;
    private void Start()
    {
        bc.SetBossDeadDelegate(ChangeTag);
    }

    public void Delegate(float _damage)
    {
        bc.UnderAttack(_damage);
    }

    private void ChangeTag()
    {
        gameObject.tag = "Untagged";
    }
}
