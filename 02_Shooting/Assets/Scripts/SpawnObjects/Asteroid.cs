using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Asteroid : PoolObject
{
    /// <summary>
    /// 파괴했을 때의 점수
    /// </summary>
    public int score = 30;

    /// <summary>
    /// 파괴 이팩트
    /// </summary>
    public PoolObjectType destroyEffect = PoolObjectType.Explosion;

    /// <summary>
    /// 최소 이동 속도
    /// </summary>
    public float minMoveSpeed = 2.0f;

    /// <summary>
    /// 최대 이동 속도
    /// </summary>
    public float maxMoveSpeed = 4.0f;

    /// <summary>
    /// 최소 회전 속도
    /// </summary>
    public float minRotateSpeed = 30.0f;

    /// <summary>
    /// 최대 회전 속도
    /// </summary>
    public float maxRotateSpeed = 360.0f;

    /// <summary>
    /// 운석의 HP
    /// </summary>
    int hitPoint = 3;

    /// <summary>
    /// 운석의 최대 HP
    /// </summary>
    public int maxHitPoint = 3;

    /// <summary>
    /// 살아있는지 여부
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// flip용 스프라이트 랜더러
    /// </summary>
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 실제 이동 속도(초당 이동 거리)
    /// </summary>
    float moveSpeed = 1.5f;

    /// <summary>
    /// 실제 회전 속도(초당 회전 각도(도:degree))
    /// </summary>
    float rotateSpeed = 30.0f;

    /// <summary>
    /// 운석의 이동 방향
    /// </summary>
    Vector3 dir = Vector3.left;

    /// <summary>
    /// 운석의 이동 방향 설정용 프로퍼티
    /// </summary>
    public Vector3 Direction
    {
        set => dir = value;
    }

    /// <summary>
    /// 플레이어에 대한 참조
    /// </summary>
    Player player = null;

    /// <summary>
    /// player에 처음 한번만 값을 설정 가능한 프로퍼티. 쓰기 전용.
    /// </summary>
    public Player TargetPlayer
    {
        set
        {
            if (player == null)     // player가 null일때만 설정
            {
                player = value;
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // 랜덤으로 이동 속도 결정
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        // 이동 속도에 따라 회전속도 변경하기

        // 최저일때 0이 되고 최고일때 1이 되는 수식만들기
        // moveSpeed가 minMoveSpeed이면 ratio는 0, moveSpeed가 maxMoveSpeed이면 ratio는 1
        float ratio = (moveSpeed - minMoveSpeed) / (maxMoveSpeed - minMoveSpeed);
        // ratio가 0이면 minRotateSpeed, 1이면 maxRotateSpeed
        rotateSpeed = Mathf.Lerp(minRotateSpeed, maxRotateSpeed, ratio);    // 보간(Interpolate)함수

        // 랜덤으로 좌우반전 시키기
        int flip = Random.Range(0, 4);
        spriteRenderer.flipX = (flip & 0b_01) != 0;
        spriteRenderer.flipY = (flip & 0b_10) != 0;

        // 다시 살아난 것 표시
        isAlive = true;

        // HP 최대치로 채우기
        hitPoint = maxHitPoint;
    }

    private void Update()
    {
        // 초당 moveSpeed의 속도로, dir 방향으로 이동 시키기
        transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);
        
        //transform.Rotate(0, 0, Time.deltaTime * -rotateSpeed);  // 시계방향으로 초당 rotateSpeed씩 회전
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);   // 반시계방향으로 초당 rotateSpeed씩 회전
    }

    private void OnDrawGizmos()
    {
        // 흰색으로 오브젝트 위치에서 dir의 1.5배 만큼 이동한 지점까지 선 그리기
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + dir * 1.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("Bullet") )
        {
            OnHit();
        }
    }

    void OnHit()
    {
        hitPoint--;         // 맞으면 hitPoint 감소
        if( hitPoint < 1 )  // hitPoint가 0아래로 내려가면
        {
            OnDie();        // 사망
        }
    }

    void OnDie()
    {
        if(isAlive)
        {
            isAlive = false;    // 죽었다 표시

            player.AddScore(score); // 점수 추가
            
            GameObject obj = Factory.Inst.GetObject(destroyEffect); // 터지는 이팩트 생성
            obj.transform.position = transform.position;    // 이팩트 위치 변경

            gameObject.SetActive(false);    // 비활성화

        }
    }
}
