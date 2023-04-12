using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System;

public class Enemy_정답 : MonoBehaviour
{
    /// <summary>
    /// 시야 범위
    /// </summary>
    public float sightRange = 10.0f;    
        
    /// <summary>
    /// 이 적이 순찰할 웨이포인트의 배열
    /// </summary>
    Transform[] waypoints;

    /// <summary>
    /// 이 적이 가야할 웨이포인트의 인덱스
    /// </summary>
    int index = 0;

    /// <summary>
    /// 이 적이 추적할 플레이어(시야 범위 안에 있을 때만 설정)
    /// </summary>
    Transform target;

    /// <summary>
    /// 네브메시 에이전트 컴포넌트
    /// </summary>
    NavMeshAgent agent;

    /// <summary>
    /// 트리거 역할을 할 스피어 컬라이더
    /// </summary>
    SphereCollider sphereCollider;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();        
    }

    private void Start()
    {        
        sphereCollider.radius = sightRange; // 시야 범위 만큼 트리거 크기 변경

        // 웨이포인트 모두 저장하기
        Transform way = GameObject.Find("Waypoints").transform;        
        waypoints = new Transform[way.childCount];  // 자식 수 만큼 배열 확보
        for(int i = 0; i < way.childCount; i++)
        {
            waypoints[i] = way.GetChild(i);         // 하나씩 추가
        }

        agent.SetDestination(waypoints[index].position);    // 처음 시작할 때 첫번째 위치로 이동하기
    }

    private void OnDrawGizmos()
    {
        // 시야 반경 원으로 그리기
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);
    }

    private void Update()
    {
        if( target != null )    // target이 있으면 
        {
            agent.SetDestination(target.position);  // target 위치로 계속 이동
            return;
        }

        // 길찾기 계산 중이 아니고 도착했으면
        if( !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance )
        {
            GoNext();   // 다음 웨이포인트로 이동
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player")) 
        {
            target = other.transform;   // 플레이어가 들어왔으면 플레이어를 타겟으로 지정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            target = null;          // 플레이어가 나가면 타겟을 비우고
            ReturnPatrol();         // 복귀 작업
        }
    }

    /// <summary>
    /// 웨이포인트의 다음 지점으로 SetDestination하는 함수
    /// </summary>
    void GoNext()
    {
        index++;
        index %= waypoints.Length;  // 인덱스를 0~(waypoints.Length-1)로 변경

        agent.SetDestination(waypoints[index].position);    // 인덱스 설정된 곳으로 이동
    }

    /// <summary>
    /// 추적 도중에 순찰로 돌아갈 때 실행하는 함수
    /// </summary>
    void ReturnPatrol()
    {
        agent.SetDestination(waypoints[index].position);    // 인덱스 위치로 돌아가기
    }
}
