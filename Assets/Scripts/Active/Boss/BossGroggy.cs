using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroggy : BossState
{
    [SerializeField] private BossStateMachine bFSM = null;

    public override void CurrentBossState()
    {
        if (bFSM.boss.SetCurTimeToReadyTime(bFSM.boss.readyTime)) // curTime <= 0f
        {
            bFSM.SetState(BossStateMachine.BossStateEnum.Idle);
        }
    }
    public override void CurrentBossStartAction()
    {
        bFSM.anim.SetBool("isIdle", false);
        bFSM.anim.SetTrigger("isGroggyStart");
        bFSM.boss.SetGroggiedTrue(true);
        bFSM.boss.SetBerserkTrue(true);
        bFSM.boss.SetCurTimeToTime(bFSM.boss.GetGroggyTime()); // curTime = groggyTime
    }
    public override void CurrentBossUpdateAction()
    {
        bFSM.boss.SetCurTimeMinusDeltaTime(Time.deltaTime); // curTime -= Time.deltaTime
        if (bFSM.boss.SetCurTimeToReadyTime(bFSM.boss.readyTime)) // curTime <= 0f
        {
            bFSM.anim.SetTrigger("isGroggyEnd");
            bFSM.boss.SetGroggiedTrue(false);
        }
    }
}
