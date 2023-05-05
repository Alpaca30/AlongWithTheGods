using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;
    [SerializeField] private LayerMask notWhatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    public float climbTimer;

    private bool climbing;

    [Header("ClimbJumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;

    private KeyCode jumpKey = KeyCode.X;
    public int climbJumps;
    private int climbJumpsLeft;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private RaycastHit notFrontWallHit;
    public bool wallFront;
    
    

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    float horizontalInput;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (pm.dead) return;
        WallCheck();
        StateMachine();
        GravityChange();
        ClimbAnimation();
        if (climbing && !exitingWall) ClimbingMovement();
    }

    private void GravityChange()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (climbTimer < 0 && wallFront && Mathf.Abs(horizontalInput) == 1)
        {
            rb.mass = 3;
        }
        else if (climbTimer < 0 && !wallFront && Mathf.Abs(horizontalInput) == 1) rb.mass = 1;
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
       
        // State 1 - Climbing
        if (wallFront && Mathf.Abs(horizontalInput) == 1 && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if (!climbing && climbTimer > 0) StartClimbing();
            // timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;

            if (climbTimer < 0) StopClimbing();
        }
        // State 2 - Exiting
        else if (exitingWall)
        {
            if (climbing) StopClimbing();

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer < 0) exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (climbing) StopClimbing();
        }

        if (wallFront && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0) ClimbJump();
    }

    private void WallCheck()
    {
        bool notWallFront1;
        bool notWallFront2;
        bool notWallFront3;
      
        wallFront = Physics.SphereCast(transform.position + transform.up * 0.5f, sphereCastRadius, transform.right * horizontalInput, out frontWallHit, detectionLength, whatIsWall);
        pm.anim.SetBool("isFrontWall", wallFront);
        notWallFront2 = Physics.Raycast(transform.position + transform.up * 1.8f, transform.right * horizontalInput, out notFrontWallHit, detectionLength, notWhatIsWall);
        notWallFront1 = Physics.Raycast(transform.position + transform.up * 0.9f, transform.right * horizontalInput, out notFrontWallHit, detectionLength, notWhatIsWall);
        notWallFront3 = Physics.Raycast(transform.position, transform.right * horizontalInput, out notFrontWallHit, detectionLength, notWhatIsWall);
        wallLookAngle = Vector3.Angle(transform.right * horizontalInput, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;
        // ㄴ Mathf부분이 40도 보다 크면 새로운 벽으로 생각함. 같은벽이면 0도, 다른벽이면 180도 나오는게 정상인데 뭔가 버그생긴거같음 40도로 올려둠.
        if ((wallFront && newWall) || pm.grounded)
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
            rb.mass = 1;
        }
        else if ((notWallFront1 || notWallFront2 || notWallFront3) && !pm.grounded)
        {
            rb.mass = 3;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);

        // sound effect
    }

    private void StopClimbing()
    {
        climbing = false;
        pm.climbing = false;
        //Debug.Log("last : " + lastHorizontalInput);
        // if (horizontalInput == 0) transform.eulerAngles = new Vector3(0f, 90f * lastHorizontalInput, 0f);
        /// idea - particle effect
        /// idea - sound effect
    }

    private void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        climbJumpsLeft--;
        anim.SetBool("isJump", true);
        gameObject.layer = 9;
        Invoke(nameof(JumpLayerChange), 0.3f);
        SoundManager.Instance.Play("ClimbingJump");
    }

    private void JumpLayerChange()
    {
        gameObject.layer = 12;
    }

    private void ClimbAnimation()
    {
        if (pm.climbing == true)
        {
            anim.SetBool("isClimbing", true);
        }
        else anim.SetBool("isClimbing", false);
    }
}