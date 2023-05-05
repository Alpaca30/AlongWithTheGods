using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float groundDrag; // ������Ʈ �������
    [SerializeField] private float airMultiplier; // �ٶ�����. ������ ���߿��� ���ӵ� ������

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float speedIncreaseMultiplier;
    [SerializeField] private float maxMoveSpeedX;
    [SerializeField] private float maxMoveSpeedY;
    private float moveSpeed;
    private float desiredMoveSpeed;

    [Header("# slopeMovement")]
    public float slopeIncreaseMultiplier;
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public float playerHeight;
    private float lastDesiredMoveSpeed;

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask ujGround;
    public bool grounded;
    public bool ujGrounded;

    private Rigidbody rb;
    public Animator anim;
    public GameObject model;
    private float horizontalInput;

    public MovementState state;
    public enum MovementState
    {
        walking,
        climbing,
        dashing,
        air
    }

    [Header("# State")]
    public bool climbing;
    public bool dashing;
    public bool dead = false;
    public bool hit = false;
    public bool attack = false;
    public bool skill = false;
    public bool isStartJumping = false;
    public bool isJumpGround = false;
    public bool isStoryAction = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        GroundCheck();
        StateHandler();
        SetAnim();
        SpeedControl();
        DragControl();
    }

    private void FixedUpdate()
    {
        if (!dead && !hit && !skill && !dashing && !isStoryAction) MovePlayer();
    }

    private void StateHandler()
    {
        // Mode - Climbing
        if (climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }
        // Mode - Dashing
        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
        }
        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else moveSpeed = desiredMoveSpeed;

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void SetAnim()
    {
        if (state == MovementState.air)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isClimbing", false);
            anim.SetBool("isJump2", true);
        }
        else if (state == MovementState.walking)
        {
            anim.SetBool("isJump2", false);
            anim.SetBool("isClimbing", false);
            anim.SetBool("isWalking", true);
        }
        else if (state == MovementState.climbing)
        {
            anim.SetBool("isJump2", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isClimbing", true);
        }
    }

    private void GroundCheck()
    {
        Vector3 frontVec = transform.position + transform.right * horizontalInput * -0.3f + transform.up * 0.5f;
        bool groundCheck = Physics.Raycast(frontVec, Vector3.down, 1f, whatIsGround);

        bool ujGroundCheck = Physics.Raycast(frontVec, Vector3.down, 0.6f, ujGround);

        if (ujGroundCheck && !climbing) ujGrounded = true;
        else ujGrounded = false;

        if (groundCheck && !climbing && !isStartJumping)
        {
            grounded = true;
            rb.mass = 1;
        }
        else grounded = false;
    }

    private void MovePlayer()
    {
        Vector3 moveDirection;
        horizontalInput = Input.GetAxisRaw("Horizontal");

        moveDirection = transform.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        anim.SetFloat("isRun", Mathf.Abs(horizontalInput));
    }

    private void SpeedControl()
    {
        Vector3 curV = rb.velocity;
        if (Mathf.Abs(curV.x) > maxMoveSpeedX) curV.x = Mathf.Sign(curV.x) * maxMoveSpeedX;
        if (curV.y > maxMoveSpeedY) curV.y = maxMoveSpeedY;
        rb.velocity = curV;
    }

    private void DragControl()
    {
        // Drag = �������׷�. 0�̸� ���������� ���� Ŭ���� ���������� ��������.
        if (state == MovementState.walking)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    public void SetStartJumping(bool _isStartJumping)
    {
        isStartJumping = _isStartJumping;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    public bool OnSlope() // �������� �ƴ����� üũ�ϴ� �Լ�.
    {
        if (Physics.Raycast(model.transform.position + model.transform.up * 0.2f, Vector3.down, out slopeHit, 0.3f)) // �Ʒ��������� ���̸� �� ������ slopeHit�� �����Ѵ�.
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal); // �Ʒ��� �� ������ ��簪�� ���Ѵ�.
            return angle < maxSlopeAngle && angle != 0; // ��簪�� �ְ��簪 ���ϰų� 0�� �ƴϸ� true�� return�Ѵ�.
        }

        return false;
    }

    public void ChangeStoryAction()
    {
        isStoryAction = !isStoryAction;
        anim.SetFloat("isRun", 0);
    }
}
