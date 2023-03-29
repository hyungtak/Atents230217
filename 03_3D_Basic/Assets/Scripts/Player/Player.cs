using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 현재 이동 속도
    /// </summary>
    private float currentMoveSpeed = 5.0f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// 점프력
    /// </summary>
    public float jumpForce = 6.0f;

    /// <summary>
    /// 플레이어의 최대 수명.
    /// </summary>
    public float lifeTimeMax = 3.0f;

    /// <summary>
    /// 플레이어의 현재 수명. 수명이 0보다 작거나 같아지면 사망
    /// </summary>
    float lifeTime = 3.0f;

    /// <summary>
    /// 수명용 프로퍼티. 쓰기는 private
    /// </summary>
    public float LifeTime
    {
        get => lifeTime;
        private set
        {
            lifeTime = value;       // 수명 변경
            onLifeTimeChange?.Invoke(lifeTime/lifeTimeMax); // 수명 변경을 알림. (비율을 알려줌)

            if(lifeTime <= 0.0f)    // 수명이 다되면 사망
            {
                Die();
            }
        }
    }

    /// <summary>
    /// 생존 여부 표시
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 수명 변경을 알리기 위한 델리게이트(수명 남은 비율을 알려줌)
    /// </summary>
    public Action<float> onLifeTimeChange;

    /// <summary>
    /// 현재 이동 방향. -1(뒤) ~ 1(앞)
    /// </summary>
    float moveDir = 0;

    /// <summary>
    /// 현재 회전 방향. -1(좌) ~ 1(우)
    /// </summary>
    float rotateDir = 0;

    /// <summary>
    /// 현재 점프 여부. true면 점프 중, false면 점프 중이 아님
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// 현재 남아있는 점프 쿨 타임
    /// </summary>
    float jumpCoolTime = 5.0f;
    
    /// <summary>
    /// 점프 쿨 타임
    /// </summary>
    public float jumpCoolTimeMax = 5.0f;

    /// <summary>
    /// 쿨 타임 변경 될 때마다 신호를 보내기 위한 프로퍼티
    /// </summary>
    private float JumpCoolTime
    {
        get => jumpCoolTime;
        set
        {
            jumpCoolTime = value;
            if(jumpCoolTime < 0)
            {
                jumpCoolTime = 0;
            }
            onJumpCoolTimeChange?.Invoke(jumpCoolTime / jumpCoolTimeMax);   // 쿨타임이 변경되면 비율을 알려줌
        }
    }

    /// <summary>
    /// 쿨타임 변경될 때 실행될 델리게이트
    /// </summary>
    Action<float> onJumpCoolTimeChange;

    /// <summary>
    /// true면 점프 쿨 완료, false 점프 쿨타임 중
    /// </summary>
    private bool IsJumpCoolEnd => jumpCoolTime <= 0;
        

    /// <summary>
    /// 플레이어가 사망했음을 알리는 델리게이트
    /// </summary>
    public Action onDie;

    /// <summary>
    /// rigid 읽기 전용 프로퍼티
    /// </summary>
    public Rigidbody Rigid => rigid;

    Rigidbody rigid;    // 리지드바디 컴포넌트
    Animator anim;      // 애니메이터 컴포넌트

    /// <summary>
    /// 인풋 시스템용 인풋액션
    /// </summary>
    PlayerInputActions inputActions;

    private void Awake()
    {        
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        inputActions = new PlayerInputActions();

        // 아이템 사용 알람이 울리면 실행될 함수 등록
        ItemUseAlarm alarm = GetComponentInChildren<ItemUseAlarm>();
        alarm.onUseableItemUsed += UseObject;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();                       // Player 액션맵 활성화
        inputActions.Player.Move.performed += OnMoveInput;  // 액션들에게 함수 바인딩하기
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Use.performed += OnUseInput;
        inputActions.Player.Jump.performed += OnJumpInput;

        isAlive = true;
        //lifeTime = lifeTimeMax;     // 변수 값을 변경하는 것
        LifeTime = lifeTimeMax;     // 프로퍼티를 실행시키는 것
        JumpCoolTime = 0.0f;        // 점프 쿨타임 초기화(시작하자마자 사용 가능)

        ResetMoveSpeed();           // 처음 속도 지정
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJumpInput;  // 액션에 연결된 함수들의 바인딩 해제
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();                      // Player 액션맵 비활성화
    }

    private void Start()
    {
        // 가상 스틱 연결
        VirtualStick stick = FindObjectOfType<VirtualStick>();
        if(stick != null)
        {
            stick.onMoveInput += (input) => SetInput(input, input != Vector2.zero); // 가상 스틱의 입력이 있으면 이동 처리
        }

        // 가상 버튼 연결
        VirtualButton button = FindObjectOfType<VirtualButton>();
        if(button != null)
        {
            button.onClick += Jump;                             // 가상 버튼이 눌려지면 점프
            onJumpCoolTimeChange += button.RefreshCoolTime;     // 점프 쿨타임이 변하면 버튼의 쿨타임 표시 변경
        }
    }

    private void Update()
    {
        LifeTime -= Time.deltaTime;
        JumpCoolTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();     // 이동 처리
        Rotate();   // 회전 처리
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // Ground와 충돌했을 때만
        {
            OnGrounded();   // 착지 함수 실행
        }
        else if(collision.gameObject.CompareTag("Platform"))
        {
            OnGrounded();   // 바닥에 도착한 처리도 추가로 해주기
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            Platform platform = other.GetComponent<Platform>();
            platform.onMove += OnRideMovingObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            Platform platform = other.GetComponent<Platform>();
            platform.onMove -= OnRideMovingObject;
        }
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();   // 현재 키보드 입력 상황 받기
        // context.performed : 액션에 연결된 키 중 하나라도 입력 중이면 true, 아니면 false
        // context.canceled : 액션에 연결된 키가 모두 입력 중이지 않으면 true, 아니면 false
        SetInput(input, !context.canceled);
    }

    /// <summary>
    /// 입력에 따라 이동 처리용 변수에 값을 설정하는 함수
    /// </summary>
    /// <param name="input">이동 방향</param>
    /// <param name="isMove">이동 중인지 아닌지(true면 이동중, false면 정지 중)</param>
    private void SetInput(Vector2 input, bool isMove)
    {
        rotateDir = input.x;    // 좌우(좌:-1, 우:+1)
        moveDir = input.y;      // 앞뒤(앞:+1, 뒤:-1)

        anim.SetBool("IsMove", isMove);      // 애니메이션 파라메터 변경(Idle, Move중 선택)
    }

    private void OnUseInput(InputAction.CallbackContext context)
    {
        anim.SetTrigger("Use");
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        Jump(); // 점프 처리 함수 실행  
    }

    /// <summary>
    /// 이동 처리 함수
    /// </summary>
    void Move()
    {
        // moveDir 방향으로 이동 시키기(앞 아니면 뒤)
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDir * transform.forward);
    }

    /// <summary>
    /// 회전 처리 함수
    /// </summary>
    void Rotate()
    {
        //rigid.AddTorque();      // 회전력 추가
        //rigid.MoveRotation();   // 특정 회전으로 설정하기
        //Quaternion rotate2 = Quaternion.Euler(
        //    0, 
        //    Time.fixedDeltaTime * rotateSpeed * rotateDir, 
        //    0);
        Quaternion rotate = Quaternion.AngleAxis(   // 특정 축을 기준으로 회전하는 쿼터니언을 만드는 함수
            Time.fixedDeltaTime * rotateSpeed * rotateDir, transform.up);   // 플레이어의 Up 방향을 기준으로 회전

        rigid.MoveRotation(rigid.rotation * rotate);    // 위에서 만든 회전을 적용
    }

    /// <summary>
    /// 점프 처리 함수
    /// </summary>
    void Jump()
    {
        if (!isJumping && IsJumpCoolEnd)    // 점프 중이 아니고 쿨타임이 다 되었을 때만 가능
        {
            JumpCoolTime = jumpCoolTimeMax; // 쿨타임 초기화
            rigid.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);  // 월드의 Up방향으로 힘을 즉시 가하기
            isJumping = true;   // 점프중이라고 표시
        }
    }

    /// <summary>
    /// 착지했을 때 처리 함수
    /// </summary>
    private void OnGrounded()
    {
        isJumping = false;      // 점프가 끝났다고 표시
    }

    /// <summary>
    /// 아이템 사용한다는 알람이 오면 실행되는 함수
    /// </summary>
    /// <param name="obj">사용할 오브젝트</param>
    private void UseObject(IUseableObject obj)
    {
        if (obj.IsDirectUse)
        {
            obj.Used(); // 사용
        }
    }

    /// <summary>
    /// 플레이어가 사망했을 때 실행이 되는 함수
    /// </summary>
    public void Die()
    {
        if(isAlive) // 살아 있는 플레이어만 죽을 수 있음
        {
            //Debug.Log("Die");
            anim.SetTrigger("Die");

            // Player 액션맵 비활성화
            inputActions.Player.Disable();

            // pitch와 roll 회전이 막혀있던 것을 풀기
            rigid.constraints = RigidbodyConstraints.None;

            // 머리 위치에 플레이어의 뒷방향으로 0.5만큼의 힘을 가하기
            Transform head = transform.GetChild(0);
            rigid.AddForceAtPosition(-transform.forward * 0.5f, head.position, ForceMode.Impulse);

            // 플레이어의 up벡터를 축으로 1만큼 회전력 더하기
            rigid.AddTorque(transform.up * 1.0f, ForceMode.Impulse);

            // 델리게이트로 알림 보내기
            onDie?.Invoke();

            // 사망 표시
            isAlive = false;
        }        
    }

    /// <summary>
    /// 점프 중이라고 강제로 설정하는 함수
    /// </summary>
    public void SetForceJumpMode()
    {
        isJumping = true;
    }

    /// <summary>
    /// 속도를 절반으로 줄이는 함수
    /// </summary>
    public void SetHalfSpeed()
    {
        currentMoveSpeed = moveSpeed * 0.5f;
    }

    /// <summary>
    /// 속도를 원래 속도로 되돌리는 함수
    /// </summary>
    public void ResetMoveSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }

    private void OnRideMovingObject(Vector3 delta)
    {
        rigid.MovePosition(rigid.position + delta);
    }
}
