using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBAttackAnimation : MonoBehaviour
{
    [SerializeField] private FireBall fireBall = null;

    public void SpawnFireBall()
    {
        fireBall.gameObject.SetActive(true);
        SoundManager.Instance.PlayOneShot("EnemyBAttackFire");
    }

    public void ShotFireBall()
    {
        fireBall.ShotFireBall();
        GetComponentInParent<EnemyStateMachine>().enemyAttack.SetRotate();
    }

    public void DisappearedFireBall()
    {
        fireBall.ResetFireBall();
    }
}
