using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : EnemyState
{
    [SerializeField] private EnemyStateMachine enemyStateMachine;
    [SerializeField] private EnemyIdle enemyIdle;
    [SerializeField] private EnemyDead enemyDead;

    public override void CurrentEnemyState()
    {
        if (enemyStateMachine.curHp <= 0)
        {
            enemyStateMachine.SetState(enemyDead);
        }
        else if (enemyStateMachine.readyTime > 0.3)
        {
            enemyStateMachine.SetState(enemyIdle);
        }
    }

    public override void CurrentEnemyAction()
    {
        enemyStateMachine.readyTime += Time.deltaTime;
    }
    public override void EnterState()
    {
        //if (enemyStateMachine.curHp > 0)
        //enemyStateMachine.anim.SetTrigger("isHit");
    }
}
