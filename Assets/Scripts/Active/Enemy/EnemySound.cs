using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public void EnemyAAttack()
    {
        SoundManager.Instance.PlayOneShot("EnemyABFollow1");
    }

    public void EnemyCAttackReady()
    {
        SoundManager.Instance.PlayOneShot("EnemyCAttackReady");
    }

    public void EnemyCAttack()
    {
        SoundManager.Instance.PlayOneShot("EnemyCAttack");
    }

    public void EnemyABDied()
    {
        SoundManager.Instance.PlayOneShot("EnemyABDied2");
    }

    public void EnemyCDied()
    {
        SoundManager.Instance.PlayOneShot("EnemyCDied");
    }
}
