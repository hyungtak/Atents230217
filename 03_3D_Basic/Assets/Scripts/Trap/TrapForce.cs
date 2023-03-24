using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 함정의 앞쪽 위 방향으로 밀어내는 함정
public class TrapForce : TrapBase
{
    /// <summary>
    /// 밀어내는 힘
    /// </summary>
    public float forcePower = 5.0f;

    Animator anim;

    private void Awake()
    {        
        anim = GetComponent<Animator>(); 
    }

    protected override void OnTrapActivate(GameObject target)
    {
        base.OnTrapActivate(target);
        anim.SetTrigger("Activate");    // 애니메이션 실행해서 바닥이 밀어내는 것처럼 보여줌
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            Vector3 dir = (transform.forward + transform.up).normalized;    // 앞쪽 위 방향 구하기(스칼라는 제거)
            player.Rigid.AddForce(dir * forcePower, ForceMode.Impulse);     // 플레이어를 위에서 구한 방향으로 힘을 더하기
            player.SetForceJumpMode();  // 날아가던 중 점프 방지용
        }
    }
}
