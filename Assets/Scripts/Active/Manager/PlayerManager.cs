using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public delegate void PlayerManagerDelegate();
    private PlayerManagerDelegate playerManagerCallback = null;
    
    public delegate void PlayerManagerHitDelegate(int _curHp);
    private PlayerManagerHitDelegate playerHitCallback = null;

    public delegate void PlayerMaxHpDelegate(int _maxHp);
    private PlayerMaxHpDelegate playerMaxHpCallback = null;

    //public delegate void SkillActiveDelegate(bool _bool);
    //private SkillActiveDelegate skillActiveCallback = null;

    public delegate void DashActiveDelegate(bool _bool);
    private DashActiveDelegate dashActiveCallback = null;

    public PlayerSpawnManager playerSpawnManager = null;
    public PlayerMovement pm = null;

    //private TestAttack ta = null;
    private Dashing dash = null;

    public void Init()
    {
        //ta = pm.GetComponentInChildren<TestAttack>();
        dash = pm.GetComponent<Dashing>();

        playerSpawnManager.player.GetComponent<PlayerState>().
            SetPlayerStateDelegate(SetPlayerIsDead, SetDashCheck, SetHitFalse, SetLaserCheck, SetHitDelegate);
        playerSpawnManager.player.GetComponent<PlayerState>().GetPlayerMaxHpDelegate(GetMaxHpDelegate);
        playerSpawnManager.SetPlayerSpawnManagerDelegate(SetDamagePlayerHp);
        //ta.GetSkillActiveDelegate(GetSkillActive);
        dash.GetDashActiveDelegate(GetDashActive);
        playerSpawnManager.player.GetComponent<PlayerState>().Init();
    }

    public void SetDamagePlayerHp() // ���� ������ ü��1��
    {
        playerSpawnManager.player.GetComponent<PlayerState>().SetDrowingCoroutine();
    }

    private void SetDashCheck()
    {
        if (pm.dashing)
        {
            playerSpawnManager.player.GetComponent<PlayerState>().SetDashInvincibilityCoroutine(); // �뽬�� �ǰ�
        }
        else
        {
            playerSpawnManager.player.GetComponent<PlayerState>().SetHitInvincibilityCoroutine(); // �׳� �ǰ�
            pm.hit = true;
        }
    }

    private void SetLaserCheck()
    {
        playerSpawnManager.player.GetComponent<PlayerState>().SetHitInvincibilityCoroutine(); // �׳� �ǰ�
        pm.hit = true;
    }

    private void SetHitFalse()
    {
        pm.skill = false;
        pm.hit = false;
    }

    private void SetHitDelegate(int _curHp)
    {
        playerHitCallback?.Invoke(_curHp);
    }

    private void SetPlayerIsDead() // �÷��̾� ü�� �ٱ�����
    {
        playerManagerCallback?.Invoke();
        pm.dead = true;
    }

    public void SetPlayerManagerDelegate(PlayerManagerDelegate _playerManagerCallback, PlayerManagerHitDelegate _playerHitCallback) // ü���� �ٱ��ΰ� GameManager���� �Ѱ���.
    {
        playerManagerCallback = _playerManagerCallback;
        playerHitCallback = _playerHitCallback;
    }

    private void GetMaxHpDelegate(int _maxHp)
    {
        playerMaxHpCallback?.Invoke(_maxHp);
    }

    public void GetPlayerMaxHpDelegate(PlayerMaxHpDelegate _playerMaxHpCallback)
    {
        playerMaxHpCallback = _playerMaxHpCallback;
    }

    //private void GetSkillActive(bool _bool)
    //{
    //    skillActiveCallback?.Invoke(_bool);
    //}

    //public void GetSkillActiveDelegate(SkillActiveDelegate _skillActiveCallback)
    //{
    //    skillActiveCallback = _skillActiveCallback;
    //}

    private void GetDashActive(bool _bool)
    {
        dashActiveCallback?.Invoke(_bool);
    }

    public void GetDashActiveDelegate(DashActiveDelegate _dashActiveCallback)
    {
        dashActiveCallback = _dashActiveCallback;
    }

    // �߰� �Լ� (��ų ��ư Ŭ�� ���. Button Listener�� ���)
    public void UseDash()
    {
        dash.UseDash();
    }
}