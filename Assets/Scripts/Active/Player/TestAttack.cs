using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static TestPlayer;

public class TestAttack : MonoBehaviour
{
    public delegate void SkillDelegate();
    private SkillDelegate skillCallback = null;

    public delegate void SkillActiveDelegate(bool _bool);
    private SkillActiveDelegate skillActiveCallback = null;

    [SerializeField] private BoxCollider weaponCollider;
    [SerializeField] private BoxCollider skillWaveCollider;
    [SerializeField] private SkillBAttack tw2 = null;
    private Animator anim;
    private PlayerMovement pm;
    private TestWeapon tw = null;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();
        tw = GetComponentInChildren<TestWeapon>();
    }

    private void Update()
    {
        AnimationAttack();
    }

    private void AnimationAttack()
    {
        if (pm.climbing || pm.dashing || pm.dead || pm.hit || pm.skill || pm.isStoryAction) return;

        if (Input.GetKeyDown(KeyCode.Z)) anim.SetTrigger("isAttack");
        if (Input.GetKeyDown(KeyCode.C)) skillCallback?.Invoke();
    }

    public void SetSkillAAnimation()
    {
        anim.SetTrigger("isSkillA");
        pm.skill = true;
        skillActiveCallback?.Invoke(!pm.skill);
    }
    public void SetSkillBAnimation()
    {
        anim.SetTrigger("isSkillB");
        pm.skill = true;
        skillActiveCallback?.Invoke(!pm.skill);
    }

    public void OnCollider1() // 1타
    {
        EffectManager.Instance.Play("PlayerAttack", this.transform, 0, 0.01f);
        weaponCollider.enabled = true;
        tw.attackDamage = 10;
    }

    public void OnCollider2() // 2타
    {
        EffectManager.Instance.Play("PlayerAttack", this.transform, 1, 0.01f);
        weaponCollider.enabled = true;
        tw.attackDamage = 20;
    }

    public void OnCollider3() // 3타
    {
        EffectManager.Instance.Play("PlayerAttack", this.transform, 2, 0.01f);
        weaponCollider.enabled = true;
        tw.attackDamage = 30;
    }

    public void OnSkillACollider1() // 스킬A 1타
    {
        weaponCollider.enabled = true;
        tw.attackDamage = 50;
        EffectManager.Instance.Play("PlayerSkillA", this.transform, 0, 0.01f);
    }

    public void OnSkillACollider2()// 스킬A 2타
    {
        weaponCollider.enabled = true;
        tw.attackDamage = 60;
        EffectManager.Instance.Play("PlayerSkillA", this.transform, 1, 0.01f);
    }

    public void OnSkillACollider3()// 스킬A 3타
    {
        weaponCollider.enabled = true;
        tw.attackDamage = 80;
        EffectManager.Instance.Play("PlayerSkillA", this.transform, 2, 0.01f);
    }

    public void OnSkillACollider4()// 스킬A 4타 
    {
        weaponCollider.enabled = true;
        tw.attackDamage = 100;
        EffectManager.Instance.Play("PlayerSkillA", this.transform, 3, 0.01f);
    }

    public void OnSkillBCollider1() // 스킬B 1타
    {
        weaponCollider.enabled = true;
        tw.attackDamage = 80;
        EffectManager.Instance.Play("PlayerSkillB", this.transform, 0, 0.01f);
    }

    public void OnSkillBCollider2() // 스킬B 2타
    {
        weaponCollider.enabled = true;
        tw.attackDamage = 60;
        EffectManager.Instance.Play("PlayerSkillB", this.transform, 1, 0.01f);
    }

    public void OnSkillBCollider3() // 스킬B 3타
    {
        weaponCollider.enabled = true;
        tw.attackDamage = 70;
        EffectManager.Instance.Play("PlayerSkillB", this.transform, 2, 0.01f);
        EffectManager.Instance.Play("PlayerSkillB", this.transform, 3, 0.2f);
    }

    public void OnSkillBCollider4() // 스킬B 4타
    {
        skillWaveCollider.enabled = true;
        tw2.attackDamage = 200;
        EffectManager.Instance.Play("PlayerSkillB", this.transform, 4, 0.01f);
    }

    public void OffCollider()
    {
        weaponCollider.enabled = false;
        skillWaveCollider.enabled = false;
    }

    public void OffSkillAction()
    {
        pm.skill = false;
        skillActiveCallback?.Invoke(!pm.skill);
    }

    public void FinishedStartJump()
    {
        pm.SetStartJumping(false);
    }

    public void SetSkillDelegate(SkillDelegate _skillCallback)
    {
        skillCallback = _skillCallback;
    }

    public void GetSkillActiveDelegate(SkillActiveDelegate _skillActiveCallback)
    {
        skillActiveCallback = _skillActiveCallback;
    }
}
