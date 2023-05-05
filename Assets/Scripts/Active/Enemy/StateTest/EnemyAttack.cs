using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    [SerializeField] private EnemyStateMachine eFSM;
    [SerializeField] private EnemyReady enemyReady;
    private float curReadyTime;

    public override void CurrentEnemyState()
    {
        if (!eFSM.playerAlive && eFSM.readyTime <= 0)
        {
            eFSM.SetState(enemyReady);
        }
        else if (eFSM.readyTime <= 0)
        {
            eFSM.readyTime = curReadyTime;
            eFSM.SetState(enemyReady);
        }
    }

    public override void CurrentEnemyAction()
    {
        eFSM.readyTime -= Time.deltaTime;
    }

    public override void EnterState()
    {
        curReadyTime = eFSM.readyTime;
        if (eFSM.enemyType == 1)
        {
            eFSM.anim.SetTrigger("isAttackA");
            Vector3 pos;
            ReversePosX(0f, out pos);
            EffectManager.Instance.Play("MonsterAttack", this.transform, _time: 0.6f);
        }
        else if (eFSM.enemyType == 2)
        {
            eFSM.anim.SetTrigger("isAttackB");
        }
        else if (eFSM.enemyType == 3)
        {
            eFSM.anim.SetTrigger("isAttackC");
            Vector3 pos;
            ReversePosX(2f, out pos);
            EffectManager.Instance.Play("MonsterSmash", this.transform, _time: 1f);
            EffectManager.Instance.Play("MonsterSmashImpact", pos, Quaternion.identity, _time: 1.2f);
        }
        eFSM.DebugBox.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void ReversePosX(float _distance, out Vector3 _pos)
    {
        Vector3 pos = this.transform.position;
        float rotY = eFSM.transf.rotation.y;

        float changer = 1f;
        if (rotY < 0) changer *= -1f;

        pos.x += _distance * changer;

        _pos = pos;
    }

    public void SetRotate()
    {
        if (eFSM.pm.transform.position.x < transform.position.x)
        {
            eFSM.model.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else eFSM.model.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }
}
