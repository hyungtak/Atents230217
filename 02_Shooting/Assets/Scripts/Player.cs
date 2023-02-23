using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어의 이동 속도
    /// </summary>
    public float speed = 10.0f;

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 0.5f;

    /// <summary>
    /// 플레이어의 총알 프리팹
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// 발사 위치 표시용 트랜스폼
    /// </summary>
    private Transform fireTransform;

    /// <summary>
    /// 총알 발사 이팩트
    /// </summary>
    private GameObject fireFlash;

    /// <summary>
    /// 에니메이터 컴포넌트
    /// </summary>
    private Animator anim;

    /// <summary>
    /// 입력처리용 InputAction
    /// </summary>
    private PlayerInputActions inputActions;

    /// <summary>
    /// 현재 입력된 입력 방향
    /// </summary>
    private Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 플레이어의 점수
    /// </summary>
    private int score = 0;

    /// <summary>
    /// 연사용 코루틴을 저장할 변수
    /// </summary>
    IEnumerator fireCoroutine;


    // 델리게이트(Delegate) : 신호를 보내는 것. 함수를 등록할 수 있다.

    /// <summary>
    /// 점수가 변경되면 실행될 델리게이트. 파라메터가 int 하나이고 리턴타입이 void인 함수를 등록할 수 있다.
    /// </summary>
    public Action<int> onScoreChange;    


    // 프로퍼티(Property) : 값을 넣거나 읽을 때 추가적으로 할일이 많을 때 사용

    /// <summary>
    /// 플레이어의 점수를 확인할 수 있는 프로퍼티(읽기 전용)
    /// </summary>
    public int Score    
    {
        // get : 다른 곳에서 특정 값을 확인할 때 사용됨
        // set : 다른 곳에서 특정 값을 설정할 때 사용됨

        //get     
        //{
        //    return score;
        //}
        get => score;   // 위에 주석으로 처리된 get을 요약한 것

        private set     // 앞에 private를 붙이면 자신만 사용가능
        {
            score = value;
            //if( onScoreChange != null )
            //{
            //    onScoreChange.Invoke(score);
            //}
            onScoreChange?.Invoke(score);   // 위의 4줄을 줄인 것. 점수가 변경되었음을 사방에 알림.

            Debug.Log($"점수 : {score}");
        }
    }

    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        anim = GetComponent<Animator>();            // GetComponent는 성능 문제가 있기 때문에 한번만 찾도록 코드 작성
        inputActions = new PlayerInputActions();
        fireTransform = transform.GetChild(0);
        fireFlash = transform.GetChild(1).gameObject;        
        fireFlash.SetActive(false);

        fireCoroutine = FireCoroutine();            // 코루틴 미리 만들어 놓기
    }

    // 이 게임 오브젝트가 완성된 이후에 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        //inputActions.Player.Fire.started;       // 버튼을 누른 직후
        //inputActions.Player.Fire.performed;     // 버튼을 충분히 눌렀을 때
        //inputActions.Player.Fire.canceled;      // 버튼을 땐 직후
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
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
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Disable();
    }

    // 시작할 때 한번 실행되는 함수
    void Start()
    {
        //Debug.Log("Start");
        //gameObject.SetActive(false);  // 게임 오브젝트 비활성화 시키기
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"충돌영역에 들어감 - 충돌 대상 : {collision.gameObject.name}");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log($"충돌영역에서 나감 - 충돌 대상 : {collision.gameObject.name}");        
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    Debug.Log("충돌영역에 접촉해 있으면서 움직이는 중");
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"트리거 안에 들어감 - 대상 트리거 : {collision.gameObject.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log($"트리거에서 나감 - 대상 트리거 : {collision.gameObject.name}");        
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("트리거 안에서 움직임");        
    //}

    private void OnFireStart(InputAction.CallbackContext _)
    {
        //Debug.Log("Fire");

        StartCoroutine(fireCoroutine);      // 눌렀을 때 코루틴 시작
    }

    private void OnFireStop(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);       // 땠을 때 코루틴 정지
    }

    /// <summary>
    /// 연사용 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            GameObject obj = Instantiate(bullet);               // 총알 생성
            obj.transform.position = fireTransform.position;    // 위치 변경
            StartCoroutine(FlashEffect());                      // flash 이팩트 깜박이기
            yield return new WaitForSeconds(fireInterval);      // 연사 간격만큼 대기
        }
    }

    /// <summary>
    /// flash가 깜빡하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashEffect()
    {
        fireFlash.SetActive(true);              // 켜고
        yield return new WaitForSeconds(0.1f);  // 0.1초 대기하고
        fireFlash.SetActive(false);             // 끄고
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

    /// <summary>
    /// Score에 점수를 추가하는 함수
    /// </summary>
    /// <param name="plus">추가할 점수</param>
    public void AddScore(int plus)
    {
        Score += plus;
    }
}
