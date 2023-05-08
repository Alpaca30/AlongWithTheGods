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
        // ���� �ð��� ���
        currentTime = Time.time;

        // ���� ���� �ð����� ���̸� ����Ͽ� �޺� ��ȿ ���θ� �Ǵ�
        comboIsValid = currentTime - lastAttackTime <= comboValidTime;

        //comboText.text = "Combo : " + comboCount.ToString();

        ComboBuff();
    }

    private void Attack()
    {
        // �޺� ��ȿ �ð� ���� ������ �� ���
        if (comboIsValid)
        {
            // �޺� ī��Ʈ ����
            comboCount++;
        }
        else // �޺� ��ȿ �ð� ���� ������ �� �� ���
        {
            // �޺� ī��Ʈ �ʱ�ȭ
            comboCount = 1;
        }

        comboCountCallback?.Invoke(comboCount);
        weaponEffectCallback?.Invoke(comboCount);
        if (comboCount < skillACount)
            skillActiveCallback?.Invoke(false);
        else
            skillActiveCallback?.Invoke(true);


        // ���� ���� �ð��� ������ ���� �ð����� ����
        lastAttackTime = currentTime;

        // ���� �� 3�ʰ� ������ ������ �޺� ī��Ʈ �ʱ�ȭ
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

    // 3�� �Ŀ� �޺� ī��Ʈ�� �ʱ�ȭ�ϴ� �Լ�
    private IEnumerator ResetComboAfterDelay()
    {
        yield return new WaitForSeconds(comboValidTime);

        // ������ ���� ���� 3�� �̻��� ���� ��� �޺� ī��Ʈ �ʱ�ȭ
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
        Debug.Log("��ų���");
        if (comboCount >= skillBCount)
        {
            // 2��ų �ߵ�
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
            // 1��ų �ߵ�
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

    // �߰� �Լ� (��ų ��ư Ŭ�� ���. Button Listener�� ���)
    public void UseActiveSkill()
    {
        ActiveSkill();
    }
}
