using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReady : EnemyState
{
    [SerializeField] private EnemyStateMachine eFSM;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyFollow enemyFollow;

    public override void CurrentEnemyState()
    {
        if (!eFSM.playerAlive)
        {
            eFSM.SetState(enemyFollow);
        }
        else if (eFSM.enemyType == 1 || eFSM.enemyType == 3)
        {
            if (!eFSM.FrontCheck())
            {
                
                eFSM.SetState(enemyFollow);
            }
            else
            {
                eFSM.SetState(enemyAttack);
            }
        }
        else if (eFSM.enemyType == 2)
        {
            if (eFSM.disBtwPlayer > eFSM.attackRange)
            {
                eFSM.SetState(enemyFollow);
            }
            else
            {
                eFSM.SetState(enemyAttack);
            }
        }
    }
    public override void CurrentEnemyAction()
    {
        
    }
    public override void EnterState()
    {
        eFSM.anim.SetBool("isFollow", false);
        eFSM.anim.SetBool("isReady", true);
        Rotate();
        eFSM.DebugBox.GetComponent<MeshRenderer>().material.color = Color.gray;
    }

    private void Rotate()
    {
        if (eFSM.enemyType != 2) return;
        if (eFSM.pm.transform.position.x < transform.position.x)
        {
            eFSM.model.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else eFSM.model.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }
}
