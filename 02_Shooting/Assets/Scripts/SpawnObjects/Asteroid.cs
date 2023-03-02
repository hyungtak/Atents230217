using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : PoolObject
{
    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 4.0f;
    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;

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
    }

    private void Update()
    {
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
}
