using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointUser : MonoBehaviour
{
    /// <summary>
    /// 이 오브젝트가 움직일 웨이포인트
    /// </summary>
    public Waypoints targetWaypoints;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 목적지 웨이포인트
    /// </summary>
    Transform target;

    private void Start()
    {
        SetTarget(targetWaypoints.CurrentWaypoint); // 첫번째 웨이포인트 지점 설정
    }

    protected void Move()
    {
        transform.Translate(Time.deltaTime * moveSpeed * transform.forward, Space.World);   // 이동

        // (거리 < 0.1), (거리의 제곱 < 0.1의 제곱) 둘의 결과는 같다.
        if ( (target.position - transform.position).sqrMagnitude < 0.01f )  // 거리가 0.1보다 작을 때
        {
            // 도착
            SetTarget(targetWaypoints.GetNextWaypoint());   // 도착했으면 다음 웨이포인트 지점 가져와서 설정하기
        }
    }

    /// <summary>
    /// 다음 웨이포인트 지정하는 함수
    /// </summary>
    /// <param name="target">다음 웨이포인트의 트랜스폼</param>
    void SetTarget(Transform target)
    {
        this.target = target;           // 목적지 설정하고
        transform.LookAt(this.target);  // 목적지 바라보기
    }
}
