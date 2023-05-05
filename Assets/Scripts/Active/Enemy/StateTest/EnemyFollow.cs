using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : EnemyState
{
    [SerializeField] private EnemyStateMachine eFSM;
    [SerializeField] private EnemyReady enemyReady;
    [SerializeField] private EnemyIdle enemyIdle;

    

    public override void CurrentEnemyState()
    {
        if (!eFSM.playerAlive)
        {
            eFSM.SetState(enemyIdle);
        }
        else if (eFSM.enemyType == 1 || eFSM.enemyType == 3)
        {
            if (eFSM.FrontCheck())
            {
                eFSM.SetState(enemyReady);
            }
            else if (eFSM.disBtwPlayer > eFSM.followRange)
            {
                eFSM.SetState(enemyIdle);
            }
        }
        else if (eFSM.enemyType == 2)
        {
            if (eFSM.disBtwPlayer < eFSM.attackRange)
            {
                eFSM.SetState(enemyReady);
            }
            else if (eFSM.disBtwPlayer > eFSM.followRange)
            {
                eFSM.SetState(enemyIdle);
            }
        }
    }

    public override void CurrentEnemyAction()
    {
        FollowPlayer();
        FollowRotation();
    }

    public override void EnterState()
    {
        CancelInvoke();
        FollowAnimation();
        eFSM.DebugBox.GetComponent<MeshRenderer>().material.color = Color.blue;
        FollowSound();
    }

    private void FollowSound()
    {
        if (eFSM.enemyType == 3)
        {
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    SoundManager.Instance.PlayOneShot("EnemyCFollow1");
                    break;
                case 1:
                    SoundManager.Instance.PlayOneShot("EnemyCFollow2");
                    break;
            }
        }
    }

    private void FollowAnimation()
    {
        eFSM.anim.SetBool("isReady", false);
        eFSM.anim.SetBool("isFollow", true);
    }

    private void FollowPlayer()
    {
        eFSM.dirToPlayer = eFSM.pm.transform.position - transform.position;
        eFSM.dirToPlayer.Normalize();
        if (eFSM.grounded) // 땅이라면 이동하기.
        {
            eFSM.rb.velocity =
                new Vector3(eFSM.dirToPlayer.x * eFSM.moveSpeed * 1.5f, eFSM.rb.velocity.y, eFSM.rb.velocity.z);
        }
    }

    private void FollowRotation()
    {
        if (eFSM.pm.transform.position.x < transform.position.x)
        {
            eFSM.model.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else eFSM.model.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }
}
