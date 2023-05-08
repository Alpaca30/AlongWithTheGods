using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class ComboController : MonoBehaviour
{
    public delegate void ComboCountDelegate(int _count);
    private ComboCountDelegate comboCountCallback = null;

    public delegate void WeaponEffectDelegate(int _count);
    private WeaponEffectDelegate weaponEffectCallback = null;
    
    public delegate void SkillActiveDelegate(bool _active);
    private SkillActiveDelegate skillActiveCallback = null;

    //[SerializeField] private TextMeshProUGUI comboText = null;
    public TestWeapon tw = null;
    public SkillBAttack tw2 = null;
    public TestAttack ta = null;
  
    private float lastAttackTime;
    private float comboValidTime = 5f;
    public int comboCount;
    private int skillACount = 30;
    private int comboBuffCount = 60;
    private int skillBCount = 100;

    private float currentTime;
    private bool comboIsValid;

    public void Init()
    {
        ta.SetSkillDelegate(ActiveSkill);
        tw.SetComboAttackDelegate(Attack);
        tw.SetBackAttackDelegate(BackAttack);
        tw.GetComponentInParent<PlayerState>().SetPlayerDashHitDelegate(DashHit);
        tw2.SetComboAttackDelegate(Attack);
        tw2.SetBackAttackDelegate(BackAttack);

        ChangeWeaponEffectDelegate(tw.SetComboSwordEffect);
    }

    private void Update()
    {
        // 현재 시간을 기록
        currentTime = Time.time;

        // 이전 공격 시간과의 차이를 계산하여 콤보 유효 여부를 판단
        comboIsValid = currentTime - lastAttackTime <= comboValidTime;

        //comboText.text = "Combo : " + comboCount.ToString();

        ComboBuff();
    }

    private void Attack()
    {
        // 콤보 유효 시간 내에 공격을 한 경우
        if (comboIsValid)
        {
            // 콤보 카운트 증가
            comboCount++;
        }
        else // 콤보 유효 시간 내에 공격을 못 한 경우
        {
            // 콤보 카운트 초기화
            comboCount = 1;
        }

        comboCountCallback?.Invoke(comboCount);
        weaponEffectCallback?.Invoke(comboCount);
        if (comboCount < skillACount)
            skillActiveCallback?.Invoke(false);
        else
            skillActiveCallback?.Invoke(true);


        // 현재 공격 시간을 마지막 공격 시간으로 저장
        lastAttackTime = currentTime;

        // 공격 후 3초간 공격이 없으면 콤보 카운트 초기화
        StartCoroutine(ResetComboAfterDelay());
    }

    private void BackAttack()
    {
        if (comboIsValid)
        {
            comboCount += 2;
        }
        else
        {
            comboCount = 2;
        }

        comboCountCallback?.Invoke(comboCount);
        weaponEffectCallback?.Invoke(comboCount);
        if (comboCount < skillACount)
            skillActiveCallback?.Invoke(false);
        else
            skillActiveCallback?.Invoke(true);

        lastAttackTime = currentTime;
        StartCoroutine(ResetComboAfterDelay());
    }

    private void DashHit()
    {
        if (comboIsValid)
        {
            comboCount += 5;
        }
        else
        {
            comboCount = 5;
        }

        comboCountCallback?.Invoke(comboCount);
        weaponEffectCallback?.Invoke(comboCount);
        if (comboCount < skillACount)
            skillActiveCallback?.Invoke(false);
        else
            skillActiveCallback?.Invoke(true);

        lastAttackTime = currentTime;
        StartCoroutine(ResetComboAfterDelay());
    }

    // 3초 후에 콤보 카운트를 초기화하는 함수
    private IEnumerator ResetComboAfterDelay()
    {
        yield return new WaitForSeconds(comboValidTime);

        // 마지막 공격 이후 3초 이상이 지난 경우 콤보 카운트 초기화
        if (Time.time - lastAttackTime >= comboValidTime)
        {
            comboCount = 0;
            comboCountCallback?.Invoke(comboCount);
            weaponEffectCallback?.Invoke(comboCount);
            if (comboCount < skillACount)
                skillActiveCallback?.Invoke(false);
            else
                skillActiveCallback?.Invoke(true);
        }
    }

    private void ActiveSkill()
    {
        Debug.Log("스킬사용");
        if (comboCount >= skillBCount)
        {
            // 2스킬 발동
            comboCount -= skillBCount;
            ta.SetSkillBAnimation();
            comboCountCallback?.Invoke(comboCount);
            weaponEffectCallback?.Invoke(comboCount);
            if (comboCount < skillACount)
                skillActiveCallback?.Invoke(false);
            else
                skillActiveCallback?.Invoke(true);
        }
        else if (comboCount >= skillACount)
        {
            // 1스킬 발동
            comboCount -= skillACount;
            ta.SetSkillAAnimation();
            comboCountCallback?.Invoke(comboCount);
            weaponEffectCallback?.Invoke(comboCount);
            if (comboCount < skillACount)
                skillActiveCallback?.Invoke(false);
            else
                skillActiveCallback?.Invoke(true);
        }
    }

    private void ComboBuff()
    {
        tw.SetComboBuff(comboCount >= comboBuffCount);
    }

    public void GetComboCountDelegate(ComboCountDelegate _comboCountCallback)
    {
        comboCountCallback = _comboCountCallback;
    }
    
    public void GetSkillActiveDelegate(SkillActiveDelegate _skillActiveCallback)
    {
        skillActiveCallback = _skillActiveCallback;
    }

    public void ChangeWeaponEffectDelegate(WeaponEffectDelegate _weaponEffectCallback)
    {
        weaponEffectCallback = _weaponEffectCallback;
    }

    // 추가 함수 (스킬 버튼 클릭 사용. Button Listener에 사용)
    public void UseActiveSkill()
    {
        ActiveSkill();
    }
}
