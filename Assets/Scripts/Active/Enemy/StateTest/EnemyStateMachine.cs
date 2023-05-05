using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyState;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyEffect effect;
    public Transform transf;
    public EnemyState currentEnemyState;
    [Header("")]

    [SerializeField] private float maxhp;
    [SerializeField] private LayerMask playerLayer;

    public EnemyAttack enemyAttack;
    public EnemyDead enemyDead;
    public Rigidbody rb;
    public Animator anim;
    public TestOnHitColorChange cc;
    public PlayerMovement pm;
    public GameObject model;

    public float moveSpeed;
    public float disBtwPlayer;
    public int nextMove = 0;
    public float readyTime;
    public bool playerAlive = true;
    public float curHp;
    public bool grounded = true;
    public Vector3 dirToPlayer;

    public float followRange;
    public float attackRange;
    public int enemyType;

    public GameObject DebugBox;
    public Color DebugColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        cc = GetComponentInChildren<TestOnHitColorChange>();
    }

    private void Start()
    {
        curHp = maxhp;
        DebugColor = DebugBox.GetComponent<MeshRenderer>().material.color;
    }

    private void Update()
    {
        GroundCheck();
        dirCheck();
        RunStateMachine();
    }

    private void dirCheck()
    {
        if(playerAlive)
        disBtwPlayer = Vector3.Distance(pm.transform.position, transform.position);
    }

    private void RunStateMachine()
    {
        if (currentEnemyState != null)
        {
            currentEnemyState.CurrentEnemyState();
            currentEnemyState.CurrentEnemyAction();
        }
    }

    public void SetState(EnemyState _nextState)
    {
        currentEnemyState = _nextState;
        currentEnemyState.EnterState();
    }

    public void UnderAttack(float _damage) // 플레이어 무기가 실행하는 함수
    {
        curHp -= _damage;
        cc.SetCoroutine();
        if (curHp <= 0) SetState(enemyDead);
    }

    private void GroundCheck()
    {
        Vector3 frontVec = model.transform.position + model.transform.forward + model.transform.up;
        bool rayHit1 = Physics.Raycast(frontVec, Vector3.down, 2);
        Debug.DrawRay(frontVec, Vector3.down, rayHit1 ? Color.red : Color.yellow);
        if (!rayHit1)
        {
            grounded = false;
        }
        else grounded = true;
    }

    public bool FrontCheck()
    {
        Vector3 frontVec = model.transform.position - model.transform.forward * 0.3f + model.transform.up * 0.3f;
        bool rayHit = Physics.Raycast(frontVec, model.transform.forward, attackRange, playerLayer);
        Debug.DrawRay(model.transform.position,Vector3.right, rayHit? Color.red : Color.yellow);
       
        if (rayHit) return true;
        else return false;
    }

    public void SetRotate()
    {
        enemyAttack.SetRotate();
    }
}