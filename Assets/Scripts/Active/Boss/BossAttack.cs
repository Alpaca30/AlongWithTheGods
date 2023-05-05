using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    [SerializeField] private BossStateMachine bFSM = null;
    private float readyTime;

    public override void CurrentBossState()
    {
        if (!bFSM.bossAction.action && readyTime > 0.5f) // curTime <= 0f
        {
            bFSM.SetState(BossStateMachine.BossStateEnum.Idle);
        }
    }

    public override void CurrentBossStartAction()
    {
        readyTime = 0f;
        bFSM.bossAction.SetBossLaserDelegate(SetLaserOff);
        bFSM.anim.SetBool("isIdle", false);
        AttackAction();
    }

    public override void CurrentBossUpdateAction()
    {
        readyTime += Time.deltaTime;
    }

    private void AttackAction()
    {
        int random = Random.Range(0, 6);
        switch (random)
        {
            case 0:
                bFSM.anim.SetTrigger("isSlider");
                break;
            case 1:
                bFSM.anim.SetTrigger("isSlider");
                break;
            case 2:
                bFSM.anim.SetTrigger("isSlider");
                break;
            case 3:
                bFSM.anim.SetTrigger("isSmash");
                EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Slash, Effect.EAttackType.None, this.gameObject.transform);
                if (bFSM.boss.GetBerserk())
                    StartCoroutine(GroggySmashCoroutine());
                break;
            case 4:
                bFSM.anim.SetTrigger("isSmash");
                EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Slash, Effect.EAttackType.None, this.gameObject.transform);
                if (bFSM.boss.GetBerserk())
                    StartCoroutine(GroggySmashCoroutine());
                break;
            case 5:
                bFSM.anim.SetTrigger("isLaser");
                break;
        }
    }
    
    private IEnumerator GroggySmashCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.Play("BossBerserkSmash");
        Vector3 playerX = bFSM.groggySmash.transform.position;
        playerX.x = bFSM.pm.transform.position.x;
        bFSM.groggySmash.transform.position = playerX;
        bFSM.groggySmash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bFSM.groggySmash.gameObject.SetActive(false);
    }

    private void SetLaserOff()
    {
        bFSM.anim.SetTrigger("isLaserEnd");
    }
}
