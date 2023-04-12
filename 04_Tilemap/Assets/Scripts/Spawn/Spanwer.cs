using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spanwer : MonoBehaviour
{
    /// <summary>
    /// 슬라임 스폰 간격
    /// </summary>
    public float interval = 1.0f;

    /// <summary>
    /// 동시에 유지되는 최대 슬라임 수
    /// </summary>
    public int capacity = 2;

    /// <summary>
    /// 스포너의 크기(transform의 position에서 부터의 크기)
    /// </summary>
    public Vector2 size;

    /// <summary>
    /// 마지막 스폰에서부터 경과한 시간
    /// </summary>
    float elapsed = 0.0f;

    /// <summary>
    /// 현재 스폰된 슬라임의 수
    /// </summary>
    int count = 0;

    /// <summary>
    /// 스폰 영역 중에서 벽이 아닌 지역
    /// </summary>
    List<Node> spawnAreaList;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 p0 = Vector3.zero;
        Vector3 p1 = Vector3.one;
        Vector3 p2 = Vector3.zero;
        Vector3 p3 = Vector3.one;

        Handles.color = Color.yellow;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p3, p0, 5);
    }
#endif
}
