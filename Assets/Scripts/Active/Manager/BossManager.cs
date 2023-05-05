using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public delegate void BossHpDelegate(float _curHp);
    private BossHpDelegate bossHpCallback = null;

    public delegate void StoryActionDelegate();
    private StoryActionDelegate storyActionCallback = null;

    // 추가
    public delegate void SetActiveBossHpDelegate(bool _active);
    private SetActiveBossHpDelegate activeBossHpCallback = null;
    //

    [SerializeField] private BossMeteor bossMeteor = null;
    [SerializeField] private BoxCollider boxCollider = null;

    public BossAttackAction bossAttackAction = null;
    public BossStateMachine bossStateMachine;
    public BossControl bossControl;
    private BossAbility boss;

    private void Awake()
    {
        boss = new BossAbility();
    }

    private void Start()
    {
        bossControl.boss = boss;
        bossStateMachine.boss = boss;
        bossStateMachine.SetActiveBossHpBarDelegate(SetActiveBossHpBar);
        bossControl.SetBossControllDelegate(SetGroggy, GetBossHp);
        bossControl.SetBossDeadDelegate(SetDead);
        bossAttackAction.ChangeStoryActionDelegate(ChangeStoryAction, OffFloorCollider);
    }

    private void SetGroggy()
    {
        bossStateMachine.SetGroggyState();
        StartCoroutine(SpawnMeteorCoroutine());
    }
    private void SetDead()
    {
        bossStateMachine.SetDeadState();
        bossMeteor.StopMeteor();
    }
    private IEnumerator SpawnMeteorCoroutine()
    {
        yield return new WaitForSeconds(4f); // 그로기시간
        bossMeteor.SpawnMeteor();
    }

    private void OffFloorCollider()
    {
        boxCollider.enabled = false;
    }

    private void GetBossHp()
    {
        float bossHp = boss.curHp / boss.maxHp; // 보스 체력을 0~1 비율로 변경해서 전달해줌.
        bossHpCallback?.Invoke(bossHp);
    }

    public void GetBossHpDelegate(BossHpDelegate _bossHpCallback)
    {
        bossHpCallback = _bossHpCallback;
    }

    // 추가
    private void SetActiveBossHpBar(bool _active)
    {
        activeBossHpCallback?.Invoke(_active);
    }

    public void SetActiveBossHpBarDelegate(SetActiveBossHpDelegate _activeBossHpBarCallback)
    {
        activeBossHpCallback = _activeBossHpBarCallback;
    }
    //

    private void ChangeStoryAction()
    {
        storyActionCallback?.Invoke();
    }

    public void ChangeStoryActionDelegate(StoryActionDelegate _storyActionCallback)
    {
        storyActionCallback = _storyActionCallback;
    }
}