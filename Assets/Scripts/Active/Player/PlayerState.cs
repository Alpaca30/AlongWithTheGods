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

    private IEnumerator DashInvincibilityCoroutine() // �뽬�� �ǰ�
    {
        invincibility = true;
        playerDashHitCallback?.Invoke();
        Debug.Log("�뽬�ǰ�");
        StartCoroutine(TimeScaleCoroutine());
        yield return new WaitForSeconds(2.0f); // �����ð�
        invincibility = false;
    }

    private IEnumerator TimeScaleCoroutine() // �뽬 ������
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(0.35f); // ������ �ð�
        Time.timeScale = 1f;
    }
    
    private IEnumerator HitInvincibilityCoroutine() // �׳� �ǰ�
    {
        if (!isInvinibility)
        {
            curHp -= damage;
        }
        playerHitCallback?.Invoke(curHp);
        //uiHpCallback?.Inovke(curHp); // ui�ʿ� ���� hp�� ����ϰ��ϴ� �ݹ� �Լ�
        cc.SetCoroutine(); // �ǰ�����Ʈ �̰ŷδ� ���� �ɽ���.
        anim.SetTrigger("isHit");
        // ���⿡ invoke �ؼ� 0.2���� �Ŀ� �ݹ���� �����ϼ��ְ�, �̰ż�ġ�� hit �ִϸ��̼� �ð��̶� ���ذ��鼭 �����ϸ��.
        Invoke(nameof(SetHitFalse), 0.35f);
        invincibility = true;
        Debug.Log("�׳��ǰ�");
        yield return new WaitForSeconds(5.0f); // �����ð�
        invincibility = false;
    }

    private IEnumerator DrowningCoroutine() // ���� ��������
    {
        if (!isInvinibility) 
        {
            curHp -= 1; // ���� ���� ������.
        }
         playerHitCallback?.Invoke(curHp);
        invincibility = true;
        yield return new WaitForSeconds(5.0f); // �����ð�
        invincibility = false;
        // �ǰݹ���, �ǰ�����Ʈ
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

    public void SetHitInvincibilityCoroutine() // �׳��ǰ�
    {
        StartCoroutine(HitInvincibilityCoroutine());
    }

    public void SetDashInvincibilityCoroutine() // �뽬�� �ǰ�
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

    public void SetPlayerDashHitDelegate(PlayerInvincibilityDelegate _playerDashHitCallback) // �뽬�޺�
    {
        playerDashHitCallback = _playerDashHitCallback;
    }
}
