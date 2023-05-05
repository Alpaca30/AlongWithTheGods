using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBAttack : MonoBehaviour
{
    public delegate void ComboAttackDelegate();
    private ComboAttackDelegate comboAttackCallback = null;
    private ComboAttackDelegate backAttackCallback = null;

    public float attackDamage;
    private float comboBuffDamage;
    private float backAttackDamage;
    private float finalDamage;
    private bool comboBuff = false;


    private void Update()
    {
        AttackDamage();
    }

    public void SetComboBuff(bool _comboBuff)
    {
        comboBuff = _comboBuff;
    }

    private void AttackDamage()
    {
        comboBuffDamage = attackDamage * 1.5f;
        backAttackDamage = attackDamage * 1.5f;
        finalDamage = (comboBuff) ? comboBuffDamage : attackDamage;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Enemy"))
        {
            if (GetComponentInParent<PlayerMovement>().model.transform.rotation.y == _other.GetComponent<EnemyStateMachine>().model.transform.rotation.y)
            {
                _other.GetComponent<EnemyStateMachine>().UnderAttack(backAttackDamage); // ╧И╬Нец
                backAttackCallback?.Invoke();
            }
            else
            {
                _other.GetComponent<EnemyStateMachine>().UnderAttack(finalDamage);
                comboAttackCallback?.Invoke();
            }
        }
        else if (_other.CompareTag("Boss"))
        {
            _other.GetComponent<BossControl>().UnderAttack(finalDamage * 1.5f);
            comboAttackCallback?.Invoke();
        }
        else if (_other.CompareTag("BossParts"))
        {
            _other.GetComponent<BossParts>().Delegate(finalDamage);
            comboAttackCallback?.Invoke();
        }
    }

    public void SetComboAttackDelegate(ComboAttackDelegate _comboAttackCallback)
    {
        comboAttackCallback = _comboAttackCallback;
    }
    public void SetBackAttackDelegate(ComboAttackDelegate _backAttackCallback)
    {
        backAttackCallback = _backAttackCallback;
    }
}
