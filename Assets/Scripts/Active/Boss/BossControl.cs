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
        bossHpCallback?.Invoke(); // �Ѵ� ���������� �ݹ����� �˷���

        Debug.Log("����ü��" + boss.curHp);
        // ����Ʈ�� ���⿡�� �浹�Ҷ� ��Ʈ������. ������ ���Ծ������� ����Ʈ �����Բ�

        if (boss.SetGroggyHp() && !boss.GetGroggied() && !groggied) // �׷α�ü�º��� ����ü���� �۾������¿��� �׷α���°� �ƴ϶��
        {
            boss.SetGroggiedTrue(true); // �׷α�true��
            bossGroggyCallback?.Invoke(); // �ݹ�
            groggied = true;
        }
        if (IsDead() && bossAlive) // ���ü�º��� ����ü���� �۴ٸ�
        {
            bossAlive = false;
            boss.SetDeadTrue(true); // ��� true��
            bossDeadCallback?.Invoke(); // �ݹ�
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