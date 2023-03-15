using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 180.0f;

    float moveDir = 0;      // -1(뒤) ~ 1(앞) 사이
    float rotateDir = 0;    // -1(좌) ~ 1(우) 사이

    Rigidbody rigid;

    PlayerInputActions inputActions;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Use.performed += OnUseInput;
        inputActions.Player.Jump.performed += OnJumpInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        rotateDir = input.x;    // 좌우(좌:-1, 우:+1)
        moveDir = input.y;      // 앞뒤(앞:+1, 뒤:-1)
        //Debug.Log(input);
    }

    private void OnUseInput(InputAction.CallbackContext context)
    {
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
    }

    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDir * transform.forward);
    }

    void Rotate()
    {
        //rigid.AddTorque();      // 회전력 추가
        //rigid.MoveRotation();   // 특정 회전으로 설정하기
        //Quaternion rotate2 = Quaternion.Euler(
        //    0, 
        //    Time.fixedDeltaTime * rotateSpeed * rotateDir, 
        //    0);
        Quaternion rotate = Quaternion.AngleAxis(
            Time.fixedDeltaTime * rotateSpeed * rotateDir, transform.up);

        rigid.MoveRotation(rigid.rotation * rotate);
    }

}
