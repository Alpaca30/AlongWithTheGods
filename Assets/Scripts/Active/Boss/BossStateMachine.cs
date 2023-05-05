using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    // 추가
    public delegate void ActiveBossHpDelegate(bool _active);
    private ActiveBossHpDelegate activeBossHpCallback = null;
    //

    [SerializeField] private BossState currentBossState = null;

    [Header("# Boss State")]
    [SerializeField] private BossAppear bossAppear = null;
    [SerializeField] private BossIdle bossIdle = null;
    [SerializeField] private BossAttack bossAttack = null;
    [SerializeField] private BossGroggy bossGroggy = null;
    [SerializeField] private BossDead bossDead = null;

    public GameObject groggySmash = null;
    public PlayerMovement pm = null;
    public Animator anim = null;
    public BossAbility boss = null;
    public BossAttackAction bossAction = null;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public enum BossStateEnum
    {
        Appear,
        Idle,
        Attack,
        Groggy,
        Dead
    }

    private void Start()
    {
        SetState(BossStateEnum.Appear);
    }

    private void Update()
    {
        StateMachine();
    }

    private void StateMachine()
    {
        if (currentBossState != null)
        {
            currentBossState.CurrentBossState();
            currentBossState.CurrentBossUpdateAction();
        }
    }

    public void SetState(BossStateEnum _nextState)
    {
        switch (_nextState)
        {
            case BossStateEnum.Appear:
                {
                    currentBossState = bossAppear;
                }
                break;
            case BossStateEnum.Idle:
                {
                    currentBossState = bossIdle;
                }
                break;
            case BossStateEnum.Attack:
                {
                    currentBossState = bossAttack;
                }
                break;
            case BossStateEnum.Groggy:
                {
                    currentBossState = bossGroggy;
                }
                break;
            case BossStateEnum.Dead:
                {
                    currentBossState = bossDead;
                }
                break;
        }
        currentBossState.CurrentBossStartAction();
    }

    public void SetGroggyState()
    {
        SetState(BossStateEnum.Groggy);
        anim.SetFloat("isAttack", 1.1f);
        bossAction.SetLaserOff();
    }

    public void SetDeadState()
    {
        SetState(BossStateEnum.Dead);
        bossAction.SetLaserOff();
        Vector3 pos = this.transform.position;
        pos.y += 0.5f;
        Quaternion rot = this.transform.rotation;
        EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Dead, Effect.EAttackType.None, pos, rot);
    }

    // 추가
    public void SetActiveBossHpBar(bool _active)
    {
        if (activeBossHpCallback == null) return;

        activeBossHpCallback?.Invoke(_active);
    }

    public void SetActiveBossHpBarDelegate(ActiveBossHpDelegate _activeBossHpBarCallback)
    {
        activeBossHpCallback = _activeBossHpBarCallback;
    }
    //
}
