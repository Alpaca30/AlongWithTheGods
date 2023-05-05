using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : BossState
{
    [SerializeField] private BossStateMachine bFSM = null;
    

    public override void CurrentBossState()
    {
        if (bFSM.boss.SetCurTimeToReadyTime(bFSM.boss.readyTime)) // curTime <= 0
        {
            bFSM.SetState(BossStateMachine.BossStateEnum.Attack);
        }
    }
    public override void CurrentBossStartAction()
    {
        bFSM.boss.SetCurTimeToTime(bFSM.boss.GetWaitingTime()); // curTime = WaitingTime
        //Debug.Log("WaitingTime : " + bFSM.boss.GetWaitingTime());
        bFSM.anim.SetBool("isIdle", true);
    }
    public override void CurrentBossUpdateAction()
    {
        bFSM.boss.SetCurTimeMinusDeltaTime(Time.deltaTime); // curTime -= Time.deltaTime
    }

}
