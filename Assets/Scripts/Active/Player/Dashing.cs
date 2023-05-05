using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public delegate void DashActiveDelegate(bool _bool);
    private DashActiveDelegate dashActiveCallback = null;

    [Header("Dashing")]
    [SerializeField] private float dashForce; // �뽬 �Ÿ�

    [Header("Cooldown")]
    [SerializeField] private float dashCd; // �뽬 ��Ÿ��
    private float dashCdTimer;

    private Rigidbody rb;
    private PlayerMovement pm;
    private Animator anim;
    private KeyCode dashKey = KeyCode.Space;
    private float horizontalInput;
    private bool dash = true; // �뽬 Ȱ��ȭ, ��Ȱ��ȭ


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey) && !pm.dead && !pm.hit && !pm.isStoryAction)
        {
            Dash();
            
            if (pm.skill && pm.dashing)
            {
                pm.skill = false;
            }
        }

        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;

            if (dashCdTimer < 0)
            {
                dash = true;
                // dash�� �غ�ƴٰ� �ݹ� �˷��ֱ�
                dashActiveCallback?.Invoke(dash);
            }
        }
    }

    private void Dash()
    {   
        if (pm.model.transform.rotation.y < 0) horizontalInput = -1f;
        else if (pm.model.transform.rotation.y > 0) horizontalInput = 1f;
        
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;
        dash = false;
        dashActiveCallback?.Invoke(dash);

        rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
        rb.AddForce(dashForce * transform.right * horizontalInput, ForceMode.Impulse);
        rb.useGravity = false;

        anim.SetTrigger("isDash");

        Invoke(nameof(ResetDash), 0.3f);
        EffectManager.Instance.Play(Effect.EActorType.Player, Effect.EActionType.Dash, Effect.EAttackType.None, this.transform);
    }
        
    private void ResetDash()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        pm.dashing = false;
    }

    public void GetDashActiveDelegate(DashActiveDelegate _dashActiveCallback)
    {
        dashActiveCallback = _dashActiveCallback;
    }

    // �߰� �Լ� (��ų ��ư Ŭ�� ���. Button Listener�� ���)
    public void UseDash()
    {
        Dash();
    }
}