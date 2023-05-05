using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDead : BossState
{
    [SerializeField] private BossStateMachine bFSM = null;
 

    public override void CurrentBossState()
    {
        
    }
    public override void CurrentBossStartAction()
    {
        bFSM.anim.SetBool("isIdle", false);
        bFSM.anim.SetTrigger("isDead");
        bFSM.SetActiveBossHpBar(false);
        Destroy(bFSM.gameObject, 7);
    }
    public override void CurrentBossUpdateAction()
    {

    }
}
