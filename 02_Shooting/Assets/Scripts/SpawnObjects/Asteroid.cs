using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : PoolObject
{
    /// <summary>
    /// 이동 속도(초당 이동 거리)
    /// </summary>
    public float moveSpeed = 1.5f;

    /// <summary>
    /// 회전 속도(초당 회전 각도(도:degree))
    /// </summary>
    public float rotateSpeed = 30.0f;

    /// <summary>
    /// 운석의 이동 방향
    /// </summary>
    Vector3 dir = Vector3.left;

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

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);

        //transform.Rotate(0, 0, Time.deltaTime * -rotateSpeed);  // 시계방향으로 초당 rotateSpeed씩 회전
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);   // 반시계방향으로 초당 rotateSpeed씩 회전
    }
}
