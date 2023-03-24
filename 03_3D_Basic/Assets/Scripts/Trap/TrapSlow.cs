using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSlow : TrapBase
{
    /// <summary>
    /// 슬로우 되는 시간
    /// </summary>
    public float slowDuration = 5.0f;

    protected override void OnTrapActivate(GameObject target)
    {
        base.OnTrapActivate(target);

        Player player = target.GetComponent<Player>();
        if(player != null)
        {
            player.SetHalfSpeed();  // 속도 절반으로 줄이기
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                StartCoroutine(RestoreSpeed(player));   // 일정 시간 후에 속도 복귀시키기
            }
        }
    }

    /// <summary>
    /// 속도 정상으로 돌리는 코루틴
    /// </summary>
    /// <param name="player">대상 플레이어</param>
    /// <returns></returns>
    IEnumerator RestoreSpeed(Player player)
    {
        yield return new WaitForSeconds(slowDuration);
        player.ResetMoveSpeed();
    }
}
