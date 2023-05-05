using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlManager : MonoBehaviour
{
    public delegate void PlayerHitDelegate(int _curHp); // 플레이어체력 알려주기
    private PlayerHitDelegate playerHitCallback = null;

    public delegate void BossHpDelegate(float _curHp); // 보스체력 알려주기
    private BossHpDelegate bossHpCallback = null;

    public delegate void PlayerMaxHpDelegate(int _maxHp); // 플레이어 최대체력 알려주기
    private PlayerMaxHpDelegate playerMaxHpCallback = null;

    public delegate void ComboCountDelegate(int _comboCount); // 콤보 알려주기
    private ComboCountDelegate comboCountCallback = null;

    public delegate void SkillActiveDelegate(bool _bool); // 스킬 사용상태 알려주기
    private SkillActiveDelegate skillActiveCallback = null;

    public delegate void DashActiveDelegate(bool _bool); // 대쉬 사용상태 알려주기
    private DashActiveDelegate dashActiveCallback = null;

    // 추가
    public delegate void SetActiveBossHpDelegate(bool _active); // 보스 체력바 활성화/비활성화
    private SetActiveBossHpDelegate setActiveBossHpBarCallback = null;
    //

    [SerializeField] private PlayerManager playerManager = null;
    [SerializeField] private EnemyManager enemyManager = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private ComboController comboController = null;
    [SerializeField] private BossManager bossManager = null;
    [SerializeField] private BossSpawnManager bossSpawnManager = null;
    [SerializeField] private FadeInOutManager fadeManager = null;
    [SerializeField] private Stage4 stage4 = null;
    [SerializeField] private Transform bossCameraTarget = null;
    [SerializeField] private RectTransform failedUI = null;


    public void Init(
        PlayerHitDelegate _playerHitCallback,
        PlayerMaxHpDelegate _playerMaxHpCallback,
        BossHpDelegate _bossHpCallback,
        ComboCountDelegate _comboCountCallback,
        SkillActiveDelegate _skillActiveCallback,
        DashActiveDelegate _dashActiveCallback,
        SetActiveBossHpDelegate _setActiveBossHpBarCallback
        )
    {
        comboController.tw = playerManager.playerSpawnManager.player.GetComponentInChildren<TestWeapon>();
        comboController.tw2 = playerManager.playerSpawnManager.player.GetComponentInChildren<SkillBAttack>();
        comboController.ta = playerManager.playerSpawnManager.player.GetComponentInChildren<TestAttack>();
        playerManager.pm = playerManager.playerSpawnManager.player.GetComponent<PlayerMovement>();
        cameraController.target = playerManager.playerSpawnManager.player.transform;

        // 외부
        GetPlayerHitDelegate(_playerHitCallback);
        GetPlayerMaxHpDelegate(_playerMaxHpCallback);
        GetBossHpDelegate(_bossHpCallback);
        GetComboCountDelegate(_comboCountCallback);
        GetSkillActiveDelegate(_skillActiveCallback);
        GetDashActiveDelegate(_dashActiveCallback);
        SetActiveBossHpBarDelegate(_setActiveBossHpBarCallback);

        // Base_Scene을 위한 실행 조건문 추가
        if (enemyManager != null)
            enemyManager.SetEnemyManagerDelegate(SetEnemyTarget);
        playerManager.SetPlayerManagerDelegate(PlayerIsDead, GetPlayerHpDelegate);
        if (bossSpawnManager != null)
            bossSpawnManager.SetBossSpawnDelegate(BossInit);
        if (stage4 != null)
            stage4.SetFadeDelegate(PlayFade);
        if (bossManager != null)
        {
            bossManager.GetBossHpDelegate(GetBossHp); // 보스Hp 위로 올려주는 델리게이트
            bossManager.SetActiveBossHpBarDelegate(SetActiveBossHpBar); // 보스 HpBar 활성/비활성 델리게이트
        }
        comboController.GetComboCountDelegate(GetComboCount); // 현재 콤보수치 위로 올려주는 델리게이트
        playerManager.GetPlayerMaxHpDelegate(GetMaxHpDelegate);
        playerManager.GetSkillActiveDelegate(GetSkillActive);
        playerManager.GetDashActiveDelegate(GetDashActive);

        playerManager.Init();
        comboController.Init();
        if (enemyManager != null)
        {
            enemyManager.Init();
            enemyManager.enemySpawnManager.Init();
        }
        if (bossManager != null)
            bossManager.ChangeStoryActionDelegate(ChangeStoryAction);

        SoundManager.Instance.Play("FieldBGM3");
    }

    private void SetEnemyTarget() // 플레이어랑 적 소환하고 적의 타겟이 플레이어다 라고 지정해줌.
    {
        for (int i = 0; i < enemyManager.enemySpawnManager.enemies.Count; ++i)
        {
            enemyManager.enemySpawnManager.enemies[i].gameObject.GetComponent<EnemyStateMachine>().pm =
            playerManager.playerSpawnManager.player.GetComponent<PlayerMovement>();
        }
        for (int i = 0; i < enemyManager.enemySpawnManager.enemies2.Count; ++i)
        {
            enemyManager.enemySpawnManager.enemies2[i].gameObject.GetComponent<EnemyStateMachine>().pm =
            playerManager.playerSpawnManager.player.GetComponent<PlayerMovement>();
        }
    }

    private void PlayerIsDead()
    {
        for (int i = 0; i < enemyManager.enemySpawnManager.enemies.Count; ++i)
        {
            enemyManager.enemySpawnManager.enemies[i].gameObject.GetComponent<EnemyStateMachine>().playerAlive = false;
        }
        for (int i = 0; i < enemyManager.enemySpawnManager.enemies2.Count; ++i)
        {
            enemyManager.enemySpawnManager.enemies2[i].gameObject.GetComponent<EnemyStateMachine>().playerAlive = false;
        }
        StartCoroutine(failedCoroutine());
    }

    private void BossInit()
    {
        bossManager.gameObject.SetActive(true);
        bossManager.bossStateMachine = bossSpawnManager.boss.GetComponent<BossStateMachine>();
        bossManager.bossControl = bossSpawnManager.boss.GetComponentInChildren<BossControl>();
        bossManager.bossStateMachine.pm = playerManager.playerSpawnManager.player.GetComponent<PlayerMovement>();
        bossManager.bossAttackAction = bossSpawnManager.boss.GetComponentInChildren<BossAttackAction>();

        cameraController.target = bossCameraTarget;
        cameraController.smoothTime = 1;
        cameraController.SetBossRoomCamera();
    }

    private IEnumerator failedCoroutine()
    {
        yield return new WaitForSeconds(2f);
        failedUI.gameObject.SetActive(true);
    }

    private void PlayFade()
    {
        fadeManager.StartFade();
    }

    private void ChangeStoryAction()
    {
        playerManager.playerSpawnManager.player.GetComponent<PlayerMovement>().ChangeStoryAction();
    }

    private void GetPlayerHpDelegate(int _curHp) // 피격 후 남은 체력 전달
    {
        playerHitCallback?.Invoke(_curHp);
    }

    public void GetPlayerHitDelegate(PlayerHitDelegate _playerHitCallback) 
    {
        playerHitCallback = _playerHitCallback;
    }

    private void GetBossHp(float _curHp) // 보스체력 전달
    {
        bossHpCallback?.Invoke(_curHp);
    }

    public void GetBossHpDelegate(BossHpDelegate _bossHpCallback) // 보스체력 전달
    {
        bossHpCallback = _bossHpCallback;
    }

    // 추가
    private void SetActiveBossHpBar(bool _active) // 보스 체력바 활성/비활성
    {
        setActiveBossHpBarCallback?.Invoke(_active);
    }

    public void SetActiveBossHpBarDelegate(SetActiveBossHpDelegate _setActiveBossHpBarCallback)
    {
        setActiveBossHpBarCallback = _setActiveBossHpBarCallback;
    }
    //

    private void GetComboCount(int _comboCount)
    {
        comboCountCallback?.Invoke(_comboCount);
    }

    public void GetComboCountDelegate(ComboCountDelegate _comboCountCallback)
    {
        comboCountCallback = _comboCountCallback;
    }

    private void GetMaxHpDelegate(int _maxHp)
    {
        playerMaxHpCallback?.Invoke(_maxHp);
    }

    public void GetPlayerMaxHpDelegate(PlayerMaxHpDelegate _playerMaxHpCallback)
    {
        playerMaxHpCallback = _playerMaxHpCallback;
    }

    private void GetSkillActive(bool _bool)
    {
        skillActiveCallback?.Invoke(_bool);
    }

    public void GetSkillActiveDelegate(SkillActiveDelegate _skillActiveCallback)
    {
        skillActiveCallback = _skillActiveCallback;
    }

    private void GetDashActive(bool _bool)
    {
        dashActiveCallback?.Invoke(_bool);
    }

    public void GetDashActiveDelegate(DashActiveDelegate _dashActiveCallback)
    {
        dashActiveCallback = _dashActiveCallback;
    }

    // 추가 함수 (스킬 버튼 클릭 사용. Button Listener에 사용)
    public void UseDash()
    {
        playerManager.UseDash();
    }

    public void UseSkill()
    {
        comboController.UseActiveSkill();
    }
}
