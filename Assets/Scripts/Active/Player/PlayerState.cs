using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public delegate void PlayerStateDelegate();
    private PlayerStateDelegate playerStateCallback = null;

    public delegate void PlayerHitDelegate(int _curHp);
    private PlayerHitDelegate playerHitCallback = null;

    public delegate void PlayerMaxHpDelegate(int _maxHp);
    private PlayerMaxHpDelegate playerMaxHpCallback = null;

    public delegate void PlayerInvincibilityDelegate();
    private PlayerInvincibilityDelegate playerInvincibilityCallback = null;
    private PlayerInvincibilityDelegate playerHitFalseCallback = null;
    private PlayerInvincibilityDelegate playerDashHitCallback = null;
    private PlayerInvincibilityDelegate playerLaserHitCallback = null;

    [SerializeField] private int maxHp;

    private TestOnHitColorChange cc;
    private PlayerEffect pe;
    private Animator anim;
    
    private int curHp;
    private int damage;
    private bool invincibility = false;
    private bool isInvinibility = false;
    private bool isDead = false;

    private void Awake()
    {
        cc = GetComponentInChildren<TestOnHitColorChange>();
        pe = GetComponentInChildren<PlayerEffect>();
        anim = GetComponentInChildren<Animator>();
        
    }

    public void Init()
    {
        curHp = maxHp;
        playerHitCallback?.Invoke(curHp);
        playerMaxHpCallback?.Invoke(maxHp); // Init
    }

    private void Update()
    {
        Dead();
        if (Input.GetKeyDown(KeyCode.P))
        {
            isInvinibility = !isInvinibility;
        }
    }

    private IEnumerator DashInvincibilityCoroutine() // 대쉬중 피격
    {
        invincibility = true;
        playerDashHitCallback?.Invoke();
        Debug.Log("대쉬피격");
        StartCoroutine(TimeScaleCoroutine());
        yield return new WaitForSeconds(2.0f); // 무적시간
        invincibility = false;
    }

    private IEnumerator TimeScaleCoroutine() // 대쉬 역경직
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(0.35f); // 역경직 시간
        Time.timeScale = 1f;
    }
    
    private IEnumerator HitInvincibilityCoroutine() // 그냥 피격
    {
        if (!isInvinibility)
        {
            curHp -= damage;
        }
        playerHitCallback?.Invoke(curHp);
        //uiHpCallback?.Inovke(curHp); // ui쪽에 현재 hp를 출력하게하는 콜백 함수
        cc.SetCoroutine(); // 피격이펙트 이거로는 조금 심심함.
        anim.SetTrigger("isHit");
        // 여기에 invoke 해서 0.2초쯤 후에 콜백쏴서 움직일수있게, 이거수치는 hit 애니메이션 시간이랑 비교해가면서 설정하면됨.
        Invoke(nameof(SetHitFalse), 0.35f);
        invincibility = true;
        Debug.Log("그냥피격");
        yield return new WaitForSeconds(5.0f); // 무적시간
        invincibility = false;
    }

    private IEnumerator DrowningCoroutine() // 물에 빠졌을때
    {
        if (!isInvinibility) 
        {
            curHp -= 1; // 물에 빠진 데미지.
        }
         playerHitCallback?.Invoke(curHp);
        invincibility = true;
        yield return new WaitForSeconds(5.0f); // 무적시간
        invincibility = false;
        // 피격무적, 피격이펙트
    }

    private void Dead()
    {
        if (curHp <= 0 && isDead == false)
        {
            isDead = true;
            anim.SetTrigger("isDead");
            pe.SetDieEffect();
            playerStateCallback?.Invoke();
        }
    }

    public void UnderAttack(int _damage)
    {
        if (invincibility) return;
        damage = _damage;
        
        playerInvincibilityCallback?.Invoke();
    }

    public void LaserUnderAttack(int _damage)
    {
        if (invincibility) return;
        damage = _damage;
        playerLaserHitCallback?.Invoke();
    }

    public void SetHitFalse()
    {
        playerHitFalseCallback?.Invoke();
    }

    public void SetHitInvincibilityCoroutine() // 그냥피격
    {
        StartCoroutine(HitInvincibilityCoroutine());
    }

    public void SetDashInvincibilityCoroutine() // 대쉬중 피격
    {
        StartCoroutine(DashInvincibilityCoroutine());
    }

    public void SetDrowingCoroutine()
    {
        StartCoroutine(DrowningCoroutine());
    }

    public void SetPlayerStateDelegate(PlayerStateDelegate _playerStateCallback, PlayerInvincibilityDelegate _playerInvincivilityCallback,
                                        PlayerInvincibilityDelegate _playerHitFalseCallback, PlayerInvincibilityDelegate _playerLaserHitCallback,
                                        PlayerHitDelegate _playerHitCallback)
    {
        playerStateCallback = _playerStateCallback;
        playerInvincibilityCallback = _playerInvincivilityCallback;
        playerHitFalseCallback = _playerHitFalseCallback;
        playerLaserHitCallback = _playerLaserHitCallback;
        playerHitCallback = _playerHitCallback;
    }

    public void GetPlayerMaxHpDelegate(PlayerMaxHpDelegate _playerMaxHpCallback)
    {
        playerMaxHpCallback = _playerMaxHpCallback;
    }

    public void SetPlayerDashHitDelegate(PlayerInvincibilityDelegate _playerDashHitCallback) // 대쉬콤보
    {
        playerDashHitCallback = _playerDashHitCallback;
    }
}
