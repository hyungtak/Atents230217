using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointUser : MonoBehaviour
{
    /// <summary>
    /// 이 오브젝트가 움직일 웨이포인트(반드시 설정되어야 한다)
    /// </summary>
    public Waypoints targetWaypoints;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 이동량을 알려주는 델리게이트
    /// </summary>
    public Action<Vector3> onMove;

    /// <summary>
    /// 이번 프레임에 이동한 정도
    /// </summary>
    protected Vector3 moveDelta = Vector3.zero;

    /// <summary>
    /// 목적지 웨이포인트
    /// </summary>
    Transform target;

    /// <summary>
    /// 이 오브젝트의 이동 방향
    /// </summary>
    Vector3 moveDir;

    private void Start()
    {
        SetTarget(targetWaypoints.CurrentWaypoint); // 첫번째 웨이포인트 지점 설정
    }

    /// <summary>
    /// 이동 처리용 함수. FixedUpdate에서 호출 할 것
    /// </summary>
    protected void Move()
    {
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDir;              // 이동 방향으로 움직이기
        transform.Translate(moveDelta, Space.World);   // 이동

        // (거리 < 0.1), (거리의 제곱 < 0.1의 제곱) 둘의 결과는 같다.
        if ((target.position - transform.position).sqrMagnitude < 0.01f)    // 거리가 0.1보다 작을 때
        {
            // 도착
            SetTarget(targetWaypoints.GetNextWaypoint());   // 도착했으면 다음 웨이포인트 지점 가져와서 설정하기
            moveDelta = Vector3.zero;
            OnArrived();
        }
        onMove?.Invoke(moveDelta);
    }

    /// <summary>
    /// 다음 웨이포인트 지정하는 함수
    /// </summary>
    /// <param name="target">다음 웨이포인트의 트랜스폼</param>
    protected virtual void SetTarget(Transform target)
    {
        this.target = target;               // 목적지 설정하고        
        moveDir = (this.target.position - transform.position).normalized;   // 이동 방향 기록해 놓기
    }

    protected virtual void OnArrived()
    {
    }
}
