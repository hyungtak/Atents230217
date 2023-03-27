using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WaypointUser
{    
    /// <summary>
    /// 톱날 회전 속도
    /// </summary>
    public float spinSpeed = 720.0f;

    /// <summary>
    /// 칼날
    /// </summary>
    Transform bladeMesh;

    private void Awake()
    {
        bladeMesh = transform.GetChild(0);
    }

    private void Update()
    {
        bladeMesh.Rotate(Time.deltaTime * spinSpeed * Vector3.right);   // 톱날 회전
    }

    private void FixedUpdate()
    {
        Move();        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 이 오브젝트는 플레이어하고만 충돌을 하므로 충돌이 일어나면 무조건 플레이어이다.
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Die();   // 컴포넌트 가져와서 죽이기
        }
    }

    protected override void SetTarget(Transform target)
    {
        base.SetTarget(target);        
        transform.LookAt(target);     // 목적지 바라보기
    }
}
