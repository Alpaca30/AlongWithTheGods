using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    public delegate void ComboAttackDelegate();
    private ComboAttackDelegate comboAttackCallback = null;
    private ComboAttackDelegate backAttackCallback = null;

    public MeshRenderer swordMr = null;
    private Material swordMat = null;

    public float attackDamage;
    private float comboBuffDamage;
    private float backAttackDamage;
    private float finalDamage;
    private bool comboBuff = false;


    private void Awake()
    {
        if (swordMr != null)
            swordMat = swordMr.material;
    }

    private void Update()
    {
        AttackDamage();
    }

    public void SetComboBuff(bool _comboBuff)
    {
        comboBuff = _comboBuff;
    }

    public void SetComboSwordEffect(int _combo)
    {
        if (swordMat == null) return;

        if (_combo < 30)
        {
            swordMat.SetColor("_MaskColor", new Color(1f, 1f, 1f, 1f));
            swordMat.SetColor("_SubMaskColor", new Color(0.2735849f, 0.2735849f, 0.2735849f, 1f));
        }
        else if (_combo < 60)
        {
            swordMat.SetColor("_MaskColor", new Color(8f, 6.305882f, 2.603922f, 1f));
            swordMat.SetColor("_SubMaskColor", new Color(0.8559341f, 0.5884547f, 0.4160791f, 1f));
        }
        else if (_combo < 100)
        {
            swordMat.SetColor("_MaskColor", new Color(9.887781f, 2.492382f, 0f, 1f));
            swordMat.SetColor("_SubMaskColor", new Color(1.212573f, 0.3328632f, 0.02377595f, 1f));
        }
        else if (_combo >= 100)
        {
            swordMat.SetColor("_MaskColor", new Color(16f, 2.572549f, 0f, 1f));
            swordMat.SetColor("_SubMaskColor", new Color(2.996078f, 0.2093372f, 0f, 1f));
        }
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
                AttackSound();
            }
            else
            {
                _other.GetComponent<EnemyStateMachine>().UnderAttack(finalDamage);
                comboAttackCallback?.Invoke();
                AttackSound();
            }
            Vector3 pos = _other.transform.position;
            pos.y += 1.2f;
            pos.z -= 1f;
            Quaternion rot = _other.transform.rotation;
            EffectManager.Instance.Play(Effect.EActorType.Monster, Effect.EActionType.Hit, Effect.EAttackType.None, pos, rot);
        }
        else if (_other.CompareTag("Boss"))
        {
            _other.GetComponent<BossControl>().UnderAttack(finalDamage * 1.5f);
            comboAttackCallback?.Invoke();
            AttackSound();
            Vector3 pos = _other.transform.position;
            pos.z -= 2f;
            Quaternion rot = _other.transform.rotation;
            EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Hit, Effect.EAttackType.None, pos, rot);
        }
        else if (_other.CompareTag("BossParts"))
        {
            _other.GetComponent<BossParts>().Delegate(finalDamage);
            comboAttackCallback?.Invoke();
            AttackSound();
            Vector3 pos = _other.transform.position;
            pos.y += 0.7f;
            pos.z -= 1f;
            Quaternion rot = _other.transform.rotation;
            EffectManager.Instance.Play(Effect.EActorType.Boss, Effect.EActionType.Hit, Effect.EAttackType.None, pos, rot);
        }
    }

    private void AttackSound()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                SoundManager.Instance.PlayOneShot("AttackSword1");
                break;
            case 1:
                SoundManager.Instance.PlayOneShot("AttackSword2");
                break;
            case 2:
                SoundManager.Instance.PlayOneShot("AttackSword3");
                break;
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
