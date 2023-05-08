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

    public void SetDamagePlayerHp() // 물에 빠지면 체력1뺌
    {
        playerSpawnManager.player.GetComponent<PlayerState>().SetDrowingCoroutine();
    }

    private void SetDashCheck()
    {
        if (pm.dashing)
        {
            playerSpawnManager.player.GetComponent<PlayerState>().SetDashInvincibilityCoroutine(); // 대쉬중 피격
        }
        else
        {
            playerSpawnManager.player.GetComponent<PlayerState>().SetHitInvincibilityCoroutine(); // 그냥 피격
            pm.hit = true;
        }
    }

    private void SetLaserCheck()
    {
        playerSpawnManager.player.GetComponent<PlayerState>().SetHitInvincibilityCoroutine(); // 그냥 피격
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

    private void SetPlayerIsDead() // 플레이어 체력 다깎였을때
    {
        playerManagerCallback?.Invoke();
        pm.dead = true;
    }

    public void SetPlayerManagerDelegate(PlayerManagerDelegate _playerManagerCallback, PlayerManagerHitDelegate _playerHitCallback) // 체력이 다깎인걸 GameManager한테 넘겨줌.
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

    // 추가 함수 (스킬 버튼 클릭 사용. Button Listener에 사용)
    public void UseDash()
    {
        dash.UseDash();
    }
}