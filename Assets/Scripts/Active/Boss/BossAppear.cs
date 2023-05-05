using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppear : BossState
{
    [SerializeField] private BossStateMachine bFSM = null;
    private float readyTime;

    public override void CurrentBossState()
    {
        if (readyTime >= 6.2f)
        {
            bFSM.SetState(BossStateMachine.BossStateEnum.Idle);
            bFSM.SetActiveBossHpBar(true);
            EffectManager.Instance.Stop("BossAppear");
        }
    }

    public override void CurrentBossStartAction()
    {
        Vector3 pos = this.transform.position;
        pos.y -= 5.25f;
        pos.z += 2.48f;
        Quaternion rot = this.transform.rotation;
        EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Appear, Effect.EAttackType.None, pos, rot);
    }

    public override void CurrentBossUpdateAction()
    {
        readyTime += Time.deltaTime;
    }
}
