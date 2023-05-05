using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : EnemyState
{
    [SerializeField] private EnemyStateMachine eFSM;

    public override void CurrentEnemyState()
    {
        
    }

    public override void CurrentEnemyAction()
    {
        
    }

    public override void EnterState()
    {
        DeadSound();
        eFSM.effect.DeadMats();
        eFSM.DebugBox.GetComponent<MeshRenderer>().material.color = Color.black;
        eFSM.rb.velocity = Vector3.zero;
        eFSM.anim.SetTrigger("isDead");
        eFSM.gameObject.GetComponent<Rigidbody>().useGravity = false;
        eFSM.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        if (eFSM.enemyType == 2)
        {
            eFSM.anim.GetComponent<EnemyBAttackAnimation>().DisappearedFireBall();
        }
        Destroy(eFSM.transform.gameObject, 3);
    }

    private void DeadSound()
    {
        if (eFSM.enemyType == 3)
        {
            SoundManager.Instance.PlayOneShot("EnemyCDied");
        }
    }
}
