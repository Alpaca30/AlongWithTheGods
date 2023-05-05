using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossAbility;

public class BossControl : MonoBehaviour
{
    public delegate void BossAbilityDelegate();
    private BossAbilityDelegate bossGroggyCallback = null;
    private BossAbilityDelegate bossDeadCallback = null;
    private BossAbilityDelegate bossHpCallback = null;
   
    public BossAbility boss;
    private bool bossAlive = true;
    private bool groggied = false;

    private void Start()
    {
        bossHpCallback?.Invoke();
    }

    public void UnderAttack(float _damage)
    {
        if (boss.GetGroggied()) _damage *= 1.2f;
        boss.curHp -= _damage;
        bossHpCallback?.Invoke(); // 한대 맞을때마다 콜백으로 알려줌

        Debug.Log("보스체력" + boss.curHp);
        // 이펙트는 무기에서 충돌할때 터트려야함. 데미지 안입었을때도 이펙트 터지게끔

        if (boss.SetGroggyHp() && !boss.GetGroggied() && !groggied) // 그로기체력보다 현제체력이 작아진상태에서 그로기상태가 아니라면
        {
            boss.SetGroggiedTrue(true); // 그로기true로
            bossGroggyCallback?.Invoke(); // 콜백
            groggied = true;
        }
        if (IsDead() && bossAlive) // 사망체력보다 현제체력이 작다면
        {
            bossAlive = false;
            boss.SetDeadTrue(true); // 사망 true로
            bossDeadCallback?.Invoke(); // 콜백
            gameObject.tag = "Untagged";
            GetComponent<SphereCollider>().isTrigger = true;
        }
    }

   
    ////
    private IEnumerator SetTimeScale()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1f;
    }
    ////
   
    private bool IsDead()
    {
        if (boss.SetCurHpComparisonHp(boss.GetDeadHp())) return true;
        else return false;
    }

    public void SetBossControllDelegate(BossAbilityDelegate _bossGroggyCallback, BossAbilityDelegate _bossHpCallback)
    {
        bossGroggyCallback = _bossGroggyCallback;
        bossHpCallback = _bossHpCallback;
    }
    public void SetBossDeadDelegate(BossAbilityDelegate _bossDeadCallback)
    {
        bossDeadCallback = _bossDeadCallback;
    }
}