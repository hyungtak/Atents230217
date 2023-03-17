using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHunter : Turret
{
    /// <summary>
    /// 이 터렛의 시야 범위
    /// </summary>
    public float sightRange = 10.0f;

    /// <summary>
    /// 이 터렛이 추적할 대상
    /// </summary>
    Transform target = null;

    /// <summary>
    /// 이 터렛이 가지고 있는 시야범위를 표시하는 트리거
    /// </summary>
    SphereCollider sightTrigger;

    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();  // 컬라이더 찾기
    }

    protected override void Start()
    {
        base.Start();
        sightTrigger.radius = sightRange;   // 시야 범위로 컬라이더 크기 변경
    }

    private void Update()
    {
        LookTarget();   // 매 프레임마다 타겟을 바라보게 만들기
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.transform;   // 트리거에 플레이어가 들어오면 타겟으로 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;              // 트리거에서 플레이어가 나가면 타겟은 null로 설정
        }
    }

    /// <summary>
    /// 타겟이 있고 테렛이 볼 수 있으면 해당 방향으로 터렛의 고개를 돌리는 함수
    /// </summary>
    private void LookTarget()
    {
        if (IsVisibleTarget())  // 타겟이 있고 볼수 있는지 확인
        {
            Vector3 dir = target.position - barrelBodyTransform.position;
            dir.y = 0;                          // 높낮이는 영향을 안끼치게 0으로 설정
            barrelBodyTransform.forward = dir;  // 대상을 바라보기

            //Vector3 lookAt = other.transform.position + Vector3.up * barrelBodyTransform.position.y;
            //barrelBodyTransform.LookAt(lookAt);
        }
    }

    /// <summary>
    /// 타겟이 존재하고 볼 수 있는지 확인하는 함수
    /// </summary>
    /// <returns>타겟이 있고 볼 수 있으면 true, 타겟이 없거나 다른 물체에 가려져 있으면 false</returns>
    bool IsVisibleTarget()
    {
        bool result = false;
        if( target != null )    // 타겟이 있는지 확인
        {
            // 터렛의 barrelBodyTransform에서 target 방향으로 나가는 레이 생성
            Vector3 toTargetDir = target.position - barrelBodyTransform.position;
            Ray ray = new Ray(barrelBodyTransform.position, toTargetDir);

            // 레이케스트 시도
            if ( Physics.Raycast(ray, out RaycastHit hitInfo, sightTrigger.radius))
            {
                // 레이케스트에 성공했으면 hitInfo에 각종 충돌 정보들이 들어옴                
                //Debug.Log($"충돌 : {hitInfo.collider.gameObject.name}");
                if ( hitInfo.transform == target)   
                {
                    // 충돌 한 것의 transform과 target이 같으면. 충돌한 것은 플레이어.
                    // 그러면 true를 리턴
                    result = true;
                }
            }
        }

        return result;
    }
}
