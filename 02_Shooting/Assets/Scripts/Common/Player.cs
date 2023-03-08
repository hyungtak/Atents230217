using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------
    [Header("플레이어 데이터")]
    /// <summary>
    /// 플레이어의 이동 속도
    /// </summary>
    public float speed = 10.0f;

    /// <summary>
    /// 플레이어의 점수
    /// </summary>
    private int score = 0;

    /// <summary>
    /// 점수가 변경되면 실행될 델리게이트. 파라메터가 int 하나이고 리턴타입이 void인 함수를 등록할 수 있다.
    /// </summary>
    public Action<int> onScoreChange;

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

    /// <summary>
    /// 총알 간 간격
    /// </summary>
    private float fireAngle = 30.0f;

    /// <summary>
    /// 파워가 최대치일 때 파워업 아이템을 먹으면 얻는 보너스
    /// </summary>
    private int extraPowerBonus = 300;

    /// <summary>
    /// 현재 플레이어의 파워
    /// </summary>
    private int power = 0;

    /// <summary>
    /// 파워를 증감시키기 위한 프로퍼티(설정시 추가처리 있음)
    /// </summary>
    private int Power
    {
        get => power;
        set
        {
            power = value;
            if (power > 3)                      // 3을 넘어가면 
                AddScore(extraPowerBonus);      // 보너스 점수 추가
            power = Mathf.Clamp(power, 1, 3);   // 1~3사이로 설정되게 Clamp 처리

            RefreshFirePostions(power);         // FireTransforms의 위치와 회전 처리
        }
    }

    // --------------------------------------------------------------------------------------------
    [Header("총알 데이터")]
    /// <summary>
    /// 플레이어의 총알 타입
    /// </summary>
    public PoolObjectType bulletType;

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 0.5f;

    /// <summary>
    /// 발사 위치 표시용 트랜스폼들
    /// </summary>
    private Transform[] fireTransforms;

    /// <summary>
    /// 총알 발사 이팩트
    /// </summary>
    private GameObject fireFlash;

    /// <summary>
    /// 연사용 코루틴을 저장할 변수
    /// </summary>
    IEnumerator fireCoroutine;

    // --------------------------------------------------------------------------------------------
    [Header("컴포넌트")]
    /// <summary>
    /// 에니메이터 컴포넌트
    /// </summary>
    private Animator anim;

    /// <summary>
    /// 리지드바디2D 컴포넌트
    /// </summary>
    private Rigidbody2D rigid;

    // --------------------------------------------------------------------------------------------
    [Header("입력 처리용")]
    /// <summary>
    /// 입력처리용 InputAction
    /// </summary>
    private PlayerInputActions inputActions;

    /// <summary>
    /// 현재 입력된 입력 방향
    /// </summary>
    private Vector3 inputDir = Vector3.zero;

    // --------------------------------------------------------------------------------------------

    // 유니티 이벤트 함수들 --------------------------------------------------------------------------------
    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        anim = GetComponent<Animator>();            // GetComponent는 성능 문제가 있기 때문에 한번만 찾도록 코드 작성
        rigid = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();
        Transform fireRoot = transform.GetChild(0);
        fireTransforms = new Transform[fireRoot.childCount];
        for (int i = 0;i < fireRoot.childCount; i++)
        {
            fireTransforms[i] = fireRoot.GetChild(i);
        }

        fireFlash = transform.GetChild(1).gameObject;        
        fireFlash.SetActive(false);

        fireCoroutine = FireCoroutine();            // 코루틴 미리 만들어 놓기
    }

    // 이 게임 오브젝트가 완성된 이후에 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    // 이 게임 오브젝트가 비활성화 될 때 실행되는 함수
    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Disable();
    }

    // 시작할 때 한번 실행되는 함수
    void Start()
    {        
        Power = 1;  // power는 1로 시작
    }

    // 일정한 시간 간격으로 호출되은 업데이트 함수(물리처리용)
    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position + Time.fixedDeltaTime * speed * inputDir);
    }

    // 충돌 했을 때 실행되는 함수(2D용)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PowerUp"))
        {
            Power++;
            collision.gameObject.SetActive(false);
        }
    }

    // 입력 이벤트에 연결된 함수들 ----------------------------------------------------------------------------

    // 발사 입력 처리용 함수
    private void OnFireStart(InputAction.CallbackContext _)
    {
        StartCoroutine(fireCoroutine);      // 눌렀을 때 코루틴 시작
    }

    // 발사 중지 입력 처리용 함수
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
            for (int i = 0; i < power; i++)
            {
                GameObject obj = Factory.Inst.GetObject(bulletType);   // bulletType에 맞는 총알 생성
                Transform firePos = fireTransforms[i];
                obj.transform.position = firePos.position;
                obj.transform.rotation = firePos.rotation;
            }

            StartCoroutine(FlashEffect());                      // flash 이팩트 깜박이기
            yield return new WaitForSeconds(fireInterval);      // 연사 간격만큼 대기
        }
    }

    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        anim.SetFloat("InputY", dir.y);         // 에니메이터에 있는 InputY 파라메터에 dir.y값을 준다.
        inputDir = dir;
    }

    // 기타 함수 --------------------------------------------------------------------------------------

    /// <summary>
    /// Score에 점수를 추가하는 함수
    /// </summary>
    /// <param name="plus">추가할 점수</param>
    public void AddScore(int plus)
    {
        Score += plus;
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

    /// <summary>
    /// 파워에 따라 발사 위치 조정하는 함수
    /// </summary>
    /// <param name="power">현재 적용된 파워</param>
    private void RefreshFirePostions(int power)
    {
        // 기존 fireRoot의 자식 비활성화 하기
        for (int i = 0; i < fireTransforms.Length; i++)
        {
            fireTransforms[i].gameObject.SetActive(false);
        }

        // fireRoot에 power 숫자에 맞게 자식 활성화
        for(int i=0;i<power;i++)
        {
            // 파워1: 0
            // 파워2: -15, 15
            // 파워3: -30, 0, 30
            
            Transform firePos = fireTransforms[i];
            firePos.localPosition = Vector3.zero;
            firePos.rotation = Quaternion.Euler(0, 0, (power - 1) * (fireAngle * 0.5f) + i * -fireAngle);
            firePos.Translate(1, 0, 0);

            fireTransforms[i].gameObject.SetActive(true);
        }
    }
}
