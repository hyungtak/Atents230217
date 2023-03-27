using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    /// <summary>
    /// 이 웨이포인트에서 사용하는 웨이포인트 지점들
    /// </summary>
    Transform[] waypoints;

    /// <summary>
    /// 현재 가고 있는 웨이포인트의 인덱스(번호)
    /// </summary>
    int index = 0;

    /// <summary>
    /// 현재 향하고 있는 웨이포인트의 트랜스폼 확인용 프로퍼티
    /// </summary>
    public Transform CurrentWaypoint => waypoints[index];   

    private void Awake()
    {
        waypoints = new Transform[transform.childCount];
        for(int i=0;i<waypoints.Length;i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 다음에 이동해야 할 웨이포인트를 알려주는 함수
    /// </summary>
    /// <returns>다음에 이동할 웨이포인트의 트랜스폼</returns>
    public Transform GetNextWaypoint()
    {
        index++;                    // index 증가
        index %= waypoints.Length;  // index는 0~(waypoints.Length-1)까지만 되어야 한다.
        return waypoints[index];    // 해당 트랜스폼 리턴
    }
}
