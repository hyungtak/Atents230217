using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;

    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    // 이 게임 오브젝트가 완성된 이후에 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        //inputActions.Player.Fire.started;       // 버튼을 누른 직후
        //inputActions.Player.Fire.performed;     // 버튼을 충분히 눌렀을 때
        //inputActions.Player.Fire.canceled;      // 버튼을 땐 직후
        inputActions.Player.Fire.performed += OnFire;
    }

    // 이 게임 오브젝트가 비활성화 될 때 실행되는 함수
    private void OnDisable()
    {
        inputActions.Player.Fire.performed -= OnFire;
        inputActions.Player.Disable();
    }

    // 시작할 때 한번 실행되는 함수
    void Start()
    {
        Debug.Log("Start");
    }

    // 매 프레임마다 계속 실행되는 함수
    void Update()
    {
        // 인풋 매니저 사용 방식 - 앞으로 사용안함
        //Debug.Log("Update");
        //if( Input.GetKeyDown(KeyCode.W))
        //{
        //    Debug.Log("W키가 눌러짐");
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("A키가 눌러짐");
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Debug.Log("S키가 눌러짐");
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Debug.Log("D키가 눌러짐");
        //}
        //float input = Input.GetAxis("Horizontal");  // 수평 방향 처리
        ////Debug.Log(input);
        //// 수직 입력 처리하기
        //input = Input.GetAxis("Vertical");
        //Debug.Log(input);

    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        Debug.Log("Fire");
    }
}
