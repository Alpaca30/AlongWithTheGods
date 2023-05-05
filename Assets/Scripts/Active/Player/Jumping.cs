using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float jumpCntLeft;
    [SerializeField] private bool readyToJump = true;

    private KeyCode jumpKey = KeyCode.X;
    private KeyCode underJumpKey = KeyCode.DownArrow;

    private Rigidbody rb;
    private PlayerMovement pm;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (pm.dead || pm.hit || pm.skill || pm.isStoryAction) return;
        ResetJump();
        JumpReady();
    }

    private void JumpReady()
    {
        if (Input.GetKeyDown(jumpKey) && readyToJump && pm.ujGrounded && Input.GetKey(underJumpKey))
        {
            ++jumpCntLeft;
            readyToJump = false;
            gameObject.layer = 9;
            UnderJump();
        }

        else if (Input.GetKeyDown(jumpKey) && readyToJump && pm.grounded)
        {
            readyToJump = false;
            ++jumpCntLeft;
            gameObject.layer = 9;
            Jump();
        }
        else if (Input.GetKeyDown(jumpKey) && jumpCntLeft > 0)
        {
            readyToJump = false;
            gameObject.layer = 9;
            Jump2();
        }
    }

    private void Jump()
    {
        --jumpCntLeft;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        SoundManager.Instance.Play("Jump");
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        pm.SetStartJumping(true);
        StopCoroutine(JumpLayerChangeCoroutine());
        StartCoroutine(JumpLayerChangeCoroutine());
        anim.SetTrigger("isJump");
    }

    private void Jump2()
    {
        --jumpCntLeft;
        SoundManager.Instance.Play("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        StopCoroutine(JumpLayerChangeCoroutine());
        StartCoroutine(JumpLayerChangeCoroutine());
        EffectManager.Instance.Play(Effect.EActorType.Player, Effect.EActionType.Jump, Effect.EAttackType.None, this.transform);
    }

    private void UnderJump()
    {
        --jumpCntLeft;
        SoundManager.Instance.Play("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(-transform.up * jumpForce, ForceMode.Impulse);
        StopCoroutine(JumpLayerChangeCoroutine());
        StartCoroutine(JumpLayerChangeCoroutine());
        anim.SetTrigger("isJump");
    }

    private IEnumerator JumpLayerChangeCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = 12;
    }

    private void ResetJump()
    {
        if (!readyToJump && pm.grounded)
        {
            readyToJump = true;
            jumpCntLeft = 1;
        }
    }
}
