using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 바닥에서 불이 나와 플레이어를 죽이는 함정
public class TrapFire : TrapBase
{
    /// <summary>
    /// 불이 나오는 기간
    /// </summary>
    public float duration = 5.0f;

    ParticleSystem ps;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        ps = child.GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        base.OnTrapActivate(target);
        ps.Play();                                      // 불 켜기
        Player player = target.GetComponent<Player>();
        if(player != null )
        {
            player.Die();                               // 플레이어 죽이기
        }
        StartCoroutine(StopEffect());                   // 일정 시간 후에 불 끄기
    }

    /// <summary>
    /// 일정 시간 후에 불 끄는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(duration);      // duration만큼 기다린 후
        ps.Stop();                                      // 불 끄기
    }
}
