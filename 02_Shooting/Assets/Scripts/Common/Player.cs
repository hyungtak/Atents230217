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
    /// 피격 되었을 때의 무적 시간
    /// </summary>
    public float invincibleTime = 2.0f;

    /// <summary>
    /// 무적 상태인지 아닌지 표시용.
    /// </summary>
    private bool isInvincibleMode = false;

    /// <summary>
    /// 무적일 때 시간 누적용(cos에서 사용할 용도)
    /// </summary>
    private float timeElapsed = 0.0f;

    /// <summary>
    /// 시작할 때 생명
    /// </summary>
    public int initialLife = 3;

    /// <summary>
    /// 현재 생명
    /// </summary>
    private int life = 3;

    /// <summary>
    /// 사망 표시
    /// </summary>
    private bool isDead = false;

    /// <summary>
    /// 플레이어 비행기 터지는 이팩트
    /// </summary>
    public PoolObjectType explosion = PoolObjectType.Explosion;

    /// <summary>
    /// 수명 처리용 프로퍼티
    /// </summary>
    private int Life
    {
        get => life;
        set
        {
            if(!isDead)
            {
                if( life > value)
                {
                    // 라이프가 감소한 상황이면
                    OnHit();    // 맞았을 때의 동작이 있는 함수 실행
                }

                life = value;
                Debug.Log($"Life : {life}");

                if( life <= 0 )
                {
                    OnDie();    // 죽었을 때의 동작이 있는 함수 실행
                }
                onLifeChange?.Invoke( life );   // 델리게이트에 연결된 함수들 실행
            }
        }
    }

    /// <summary>
    /// 수명이 변경되었을 때 실행될 델리게이트
    /// </summary>
    public Action<int> onLifeChange;

    /// <summary>
    /// 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action<Player> onDie;

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
        get => score;   // 위에 주석으로 처리된 get을 요약한 것

        private set     // 앞에 private를 붙이면 자신만 사용가능
        {
            score = value;
            onScoreChange?.Invoke(score);   // 위의 4줄을 줄인 것. 점수가 변경되었음을 사방에 알림.
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

    /// <summary>
    /// 스프라이트 랜더러 컴포넌트
    /// </summary>
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        InputDisable();
    }

    // 시작할 때 한번 실행되는 함수
    void Start()
    {        
        Power = 1;          // power는 1로 시작
        Life = initialLife; // 초기 생명 설정
    }

    private void Update()
    {
        if( isInvincibleMode )
        {
            timeElapsed += Time.deltaTime * 30;     // 1초당으로 처리하면 한번 깜박이는데 3.141592... 초가 필요
            float alpha = (MathF.Cos(timeElapsed) + 1.0f) * 0.5f;   // cos 결과를 1~0~1로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }
    }

    // 일정한 시간 간격으로 호출되은 업데이트 함수(물리처리용)
    private void FixedUpdate()
    {
        if (isDead)
        {
            rigid.AddForce(Vector2.left * 0.3f, ForceMode2D.Impulse);  // 왼쪽으로 폭팔적으로 힘 추가
            rigid.AddTorque(30.0f);     // 반시계 방향으로 회전
        }
        else
        {
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * speed * inputDir);
        }
    }

    // 충돌 했을 때 실행되는 함수(2D용)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("Enemy"))
        {
            Life--;
        }
        else if(collision.gameObject.CompareTag("PowerUp"))
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

    /// <summary>
    /// 입력 연결 끊고 비활성화 시키기
    /// </summary>
    private void InputDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Disable();
    }

    // 피격 관련 -----------------------------------------------------------------------------------

    /// <summary>
    /// 맞았을 때 실행되는 함수
    /// </summary>
    private void OnHit()
    {
        Power--;                                // 파워1 줄이고
        StartCoroutine(EnterInvincibleMode());  // 무적 모드 들어가기
    }

    /// <summary>
    /// 무적 상태 진입용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterInvincibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible"); // 레이어를 무적 레이어로 변경
        isInvincibleMode = true;    // 무적 모드로 들어갔다고 표시
        timeElapsed = 0.0f;         // cos용 시간 카운터 초기화

        yield return new WaitForSeconds(invincibleTime);        // invincibleTime만큼 기다리기

        spriteRenderer.color = Color.white;                     // 색이 변한 상태에서 무적모드가 끝날 때를 대비해서 색상도 초기화
        isInvincibleMode = false;                               // 무적 모드 끝났다고 표시
        gameObject.layer = LayerMask.NameToLayer("Player");     // 레이어 되돌리기
    }

    private void OnDie()
    {
        isDead = true;
        life = 0;

        Collider2D bodyCollider = GetComponent<Collider2D>();
        bodyCollider.enabled = false;                           // 컬라이더 꺼서 양향력 제거

        GameObject effect = Factory.Inst.GetObject(explosion);  // 터지는 이팩트 만들고
        effect.transform.position = transform.position;         // 이팩트 내 위치에 배치

        InputDisable();                 // 입력 막기
        inputDir = Vector3.zero;        // 이동 입력도 초기화

        StopCoroutine(fireCoroutine);   // 총을 쏘던 중이면 더 이상 쏘지 않게 만들기

        // 무적모드 취소
        spriteRenderer.color = Color.white;                     
        isInvincibleMode = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        rigid.gravityScale = 1.0f;      // 중력 다시 적용
        rigid.freezeRotation = false;   // 회전도 풀기

        onDie?.Invoke(this);    // 죽었다고 알리기
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
