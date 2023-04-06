using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 이동 관련 -----------------------------------------------------------------------------------
    /// <summary>
    /// 플레이어의 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 플레이어의 입력 방향
    /// </summary>
    Vector2 inputDir;

    /// <summary>
    /// 공격했을 때 저장해 놓은 원래 이동 방향
    /// </summary>
    Vector2 oldInputDir;

    /// <summary>
    /// 현재 이동 중인지 표시
    /// </summary>
    bool isMove = false;

    // 공격 관련 -----------------------------------------------------------------------------------
    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 1.0f;

    /// <summary>
    /// 현재 공격 중인지 표시
    /// </summary>
    bool isAttacking = false;

    /// <summary>
    /// 공격이 유효한 애니메이션 상태인지 표시하는 변수
    /// </summary>
    bool isAttackValid = false;

    /// <summary>
    /// 현재 남아있는 공격 쿨타임
    /// </summary>
    float currentAttackCoolTime = 0.0f;

    /// <summary>
    /// 공격 영역의 중심축
    /// </summary>
    Transform attackAreaCenter;

    /// <summary>
    /// 플레이어의 공격 범위 안에 들어와 있는 모든 슬라임
    /// </summary>
    List<Slime> attackTargetList;

    // 기타 ---------------------------------------------------------------------------------------
    // 컴포넌트들
    Animator anim;
    Rigidbody2D rigid;

    // 입력 인풋 액션
    PlayerInputActions inputActions;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();

        attackAreaCenter = transform.GetChild(0);

        attackTargetList = new List<Slime>(4);
        EnemySensor sensor = attackAreaCenter.GetComponentInChildren<EnemySensor>();    // 센서 찾고
        sensor.onEnemyEnter += (slime) =>
        {
            // 센서 안에 슬라임이 들어오면 
            if(isAttackValid)
            {
                // 공격이 유효한 상태면 바로 죽이기
                slime.OnAttacked();
            }
            else
            {
                // 공격이 아직 유효하지 않으면 리스트에 담아 놓기
                attackTargetList.Add(slime);    // 리스트에 추가하고
                slime.ShowOutline(true);        // 아웃라인 표시
            }
        };
        sensor.onEnemyExit += (slime) =>
        { 
            // 센서에서 슬라임이 나가면
            attackTargetList.Remove(slime); // 리스트에서 제거하고
            slime.ShowOutline(false);       // 아웃라인 끄기
        };
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;    // 무조건 쿨타임 감소시키기
    }

    private void FixedUpdate()
    {
        //transform.Translate(Time.fixedDeltaTime * speed * inputDir);
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * inputDir);    // 이동 처리
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (isAttacking)
        {
            oldInputDir = input; // 공격 중일 때는 백업해 놓은 값만 변경
        }
        else
        {
            inputDir = input;    // 입력 방향 저장            
            anim.SetFloat("InputX", inputDir.x);        // 애니메이션 파라메터 설정
            anim.SetFloat("InputY", inputDir.y);

            AttackAreaRotate(inputDir);                 // 입력 방향에 따라 회전
        }

        isMove = true;                              // 이동 중이라고 표시
        anim.SetBool("IsMove", isMove);        
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        inputDir = Vector2.zero;                // 입력 방향 (0,0)으로 설정
        isMove = false;                         // 멈췄다고 표시

        anim.SetBool("IsMove", isMove);         // 애니메이션 파라메터 설정
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if( currentAttackCoolTime < 0 )             // 쿨타임이 0미만일 때만 가능
        {
            isAttacking = true;
            oldInputDir = inputDir;                 // 입력 방향 백업
            inputDir = Vector2.zero;                // 입력 방향 초기화(안움직이게 만드는 목적)
            anim.SetTrigger("Attack");              // 공격 애니메이션 재생
            currentAttackCoolTime = attackCoolTime; // 쿨타임 초기화
        }
    }

    /// <summary>
    /// 공격이 효과가 있을 때 실행
    /// 애니메이션 이벤트로 실행할 함수.
    /// </summary>
    public void AttackValid()
    {
        isAttackValid = true;

        while(attackTargetList.Count > 0)       // 리스트에 슬라임이 남아있으면 계속 반복
        {
            Slime slime = attackTargetList[0];  // 하나를 꺼내서
            attackTargetList.RemoveAt(0);
            slime.OnAttacked();                 // 공격하기
        }
    }

    /// <summary>
    /// 공격 효과가 없어졌을 때 실행
    /// 애니메이션 이벤트로 실행할 함수.
    /// </summary>
    public void AttackNotValid()
    {
        isAttackValid = false;
    }

    /// <summary>
    /// 백업해 놓은 입력 방향을 복원하는 함수
    /// </summary>
    public void RestoreInputDir()
    {
        if( isMove )                    // 아직 이동 중일 때만
        {
            inputDir = oldInputDir;                 // 입력 방향 복원
            anim.SetFloat("InputX", inputDir.x);    // 애니메이션 파라메터 설정
            anim.SetFloat("InputY", inputDir.y);

            AttackAreaRotate(inputDir);             // 공격 영역 회전 시키기
        }
        isAttacking = false;    // 이동 중이든 아니든 무조건 false로 초기화
    }

    /// <summary>
    /// 공격 영역 회전 시키는 함수
    /// </summary>
    /// <param name="input">입력된 방향</param>
    private void AttackAreaRotate(Vector2 input)
    {
        // 공격 영역 중심축 회전하기
        if (input.y < 0)            // 아래로 이동
        {
            attackAreaCenter.rotation = Quaternion.identity;
        }
        else if (input.y > 0)       // 위로 이동
        {
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, 180.0f);
        }
        else if (input.x > 0)       // 오른쪽으로 이동
        {
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, 90.0f);  // 반시계방향으로 90도
        }
        else if (input.x < 0)       //왼쪽으로 이동
        {
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, -90.0f); // 시계방향으로 90도
        }
        else
        {
            attackAreaCenter.rotation = Quaternion.identity;    // 중립
        }
    }
}
