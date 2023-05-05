using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    [SerializeField] private EnemyStateMachine eFSM;
    [SerializeField] private EnemyFollow enemyFollow;

    public override void CurrentEnemyState()
    {
        if (eFSM.disBtwPlayer <= eFSM.followRange && eFSM.playerAlive)
        {
            eFSM.SetState(enemyFollow);
        }
    }

    public override void CurrentEnemyAction()
    {
        ChangeDir();
        EnemyPatrol();
        RotateEnemy();
        PatrolAnimation();
        eFSM.DebugBox.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public override void EnterState()
    {
        eFSM.anim.SetBool("isFollow", false);
        EnemyMoveAI();
    }

    private void EnemyPatrol()
    {
        eFSM.rb.velocity =
            new Vector3(eFSM.nextMove * eFSM.moveSpeed, eFSM.rb.velocity.y, eFSM.rb.velocity.z);
    }

    private void EnemyMoveAI()
    {
        eFSM.nextMove = Random.Range(-1, 2); //-1 0 1 Áß ÇÑ°³
        Invoke("EnemyMoveAI", 2); // 2ÃÊ Äð Àç±Í
    }

    private void RotateEnemy()
    {
        if (eFSM.rb.velocity.x < 0)
            eFSM.model.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        else if (eFSM.rb.velocity.x > 0)
            eFSM.model.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    private void PatrolAnimation()
    {
        eFSM.anim.SetFloat("isRun", eFSM.nextMove);
    }
    private void ChangeDir()
    {
        if (eFSM.grounded) return;
        eFSM.nextMove *= -1;
        CancelInvoke();
        Invoke(nameof(EnemyMoveAI), 2);
    }
}
