using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject bullet;

    Transform fireTransform;
    Animator anim;
    PlayerInputActions inputActions;
    Vector3 inputDir = Vector3.zero;

    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        anim = GetComponent<Animator>();
        inputActions = new PlayerInputActions();
        fireTransform = transform.GetChild(0);        
    }

    // 이 게임 오브젝트가 완성된 이후에 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        //inputActions.Player.Fire.started;       // 버튼을 누른 직후
        //inputActions.Player.Fire.performed;     // 버튼을 충분히 눌렀을 때
        //inputActions.Player.Fire.canceled;      // 버튼을 땐 직후
        inputActions.Player.Fire.performed += OnFire;
        inputActions.Player.Bomb.performed += OnBomb;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    // 이 게임 오브젝트가 비활성화 될 때 실행되는 함수
    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Bomb.performed -= OnBomb;
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

        //Time.deltaTime * speed * inputDir     // 곱하기 총 4번
        //inputDir * Time.deltaTime * speed     // 곱하기 총 6번


        //transform.position += Time.deltaTime * speed * inputDir;
        transform.Translate(Time.deltaTime * speed * inputDir); // 초당 speed의 속도로 inputDir방향으로 이동
        // Time.deltaTime : 이전 프레임에서 현재 프레임까지의 시간

        // 30프레임 컴퓨터의 deltaTime = 1/30초 = 0.333333
        // 120프레임 컴퓨터의 deltaTime = 1/120초 = 0.0083333

    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");

        GameObject obj = Instantiate(bullet);
        obj.transform.position = fireTransform.position;
    }

    private void OnBomb(InputAction.CallbackContext context)
    {
        Debug.Log("Bomb");
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        anim.SetFloat("InputY", dir.y);         // 에니메이터에 있는 InputY 파라메터에 dir.y값을 준다.
        inputDir = dir;
    }
}
