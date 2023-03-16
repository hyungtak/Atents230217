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
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// 점프력
    /// </summary>
    public float jumpForce = 6.0f;

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
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();                       // Player 액션맵 활성화
        inputActions.Player.Move.performed += OnMoveInput;  // 액션들에게 함수 바인딩하기
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Use.performed += OnUseInput;
        inputActions.Player.Jump.performed += OnJumpInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJumpInput;  // 액션에 연결된 함수들의 바인딩 해제
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();                      // Player 액션맵 비활성화
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
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();   // 현재 키보드 입력 상황 받기
        rotateDir = input.x;    // 좌우(좌:-1, 우:+1)
        moveDir = input.y;      // 앞뒤(앞:+1, 뒤:-1)

        // context.performed : 액션에 연결된 키 중 하나라도 입력 중이면 true, 아니면 false
        // context.canceled : 액션에 연결된 키가 모두 입력 중이지 않으면 true, 아니면 false

        anim.SetBool("IsMove", !context.canceled);      // 애니메이션 파라메터 변경(Idle, Move중 선택)
    }

    private void OnUseInput(InputAction.CallbackContext context)
    {
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
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDir * transform.forward);
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
        if (!isJumping)         // 점프 중이 아닐때만
        {
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
}
