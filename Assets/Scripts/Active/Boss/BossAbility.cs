using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbility
{
    public float maxHp { get; set; }
    public float curHp { get; set; }
    public float SetCurHpMinusDamage(float _damage) { return curHp - _damage; }
    public bool SetCurHpComparisonHp(float _hp) { return curHp <= _hp; }
    private float curTime;
    public float GetCurTime() { return curTime; }
    public float SetCurTimeToTime(float _time) { return curTime = _time; }
    public float SetCurTimeMinusDeltaTime(float _deltaTime) { curTime -= _deltaTime; return curTime; }
    public bool SetCurTimeToReadyTime(float _readyTime) { return curTime <= _readyTime; }
    private float waitingTime;
    public float GetWaitingTime() { return groggied? waitingTime * 0.8f : waitingTime; }
    public float readyTime { get; private set; }
    private float attackOverTime;
    public float GetAttackOverTime() { return groggied? attackOverTime * 0.7f : attackOverTime; }
    private float groggyHp;
    public float GetGroggyHp() { return groggyHp; }
    public bool SetGroggyHp() { if (curHp <= groggyHp ) return true; else return false;  }
    private float groggyTime;
    public float GetGroggyTime() { return groggyTime; }
    private bool groggied;
    public bool GetGroggied() { return groggied; }
    public void SetGroggiedTrue(bool _bool) { groggied = _bool; }
    private bool berserk;
    public bool GetBerserk() { return berserk; }
    public void SetBerserkTrue(bool _bool) { berserk = _bool; }
    public float rotateSpeed { get; set; }
    public float rotateDir { get; set; }
    private bool dead;
    public bool GetDead() { return dead; }
    public bool SetDeadTrue(bool _bool) { return dead = _bool; }
    private float deadHp;
    public float GetDeadHp() { return deadHp; }
    private float sliderAttack;
    public float GetSliderAttack() { return sliderAttack; }
    private float smashAttack;
    public float GetSmashAttack() { return smashAttack; }
    private float laserAttack;
    public float GetLaserAttack() { return laserAttack; }
    

    public BossAbility()
    {
        maxHp = 3000f;
        curHp = maxHp;
        groggyHp = maxHp * 0.5f;
        waitingTime = 3f;
        groggyTime = 4f;
        groggied = false;
        attackOverTime = 1f;
        deadHp = 0f;
        readyTime = 0f;
        curTime = 3f;
        
        //sliderAttack = 1f;
        //smashAttack = 1f;
        //laserAttack = 2f;
    }
}
