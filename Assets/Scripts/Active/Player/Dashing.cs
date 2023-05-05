using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public delegate void DashActiveDelegate(bool _bool);
    private DashActiveDelegate dashActiveCallback = null;

    [Header("Dashing")]
    [SerializeField] private float dashForce; // 대쉬 거리

    [Header("Cooldown")]
    [SerializeField] private float dashCd; // 대쉬 쿨타임
    private float dashCdTimer;

    private Rigidbody rb;
    private PlayerMovement pm;
    private Animator anim;
    private KeyCode dashKey = KeyCode.Space;
    private float horizontalInput;
    private bool dash = true; // 대쉬 활성화, 비활성화


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
                // dash가 준비됐다고 콜백 알려주기
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

    // 추가 함수 (스킬 버튼 클릭 사용. Button Listener에 사용)
    public void UseDash()
    {
        Dash();
    }
}