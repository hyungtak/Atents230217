using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TrapActivate(other.gameObject); // 트리거에 플레이어가 들어오면 함정 발동
        }
    }

    /// <summary>
    /// 함정이 발동 될 때 해야할 일을 가지는 함수(공통)
    /// </summary>
    /// <param name="target">함정을 발동시킨 대상</param>
    private void TrapActivate(GameObject target)
    {
        OnTrapActivate(target);
    }

    /// <summary>
    /// TrapBase를 상속 받은 함정마다 오버라이드할 함수(상속 받은 함수별)
    /// </summary>
    /// <param name="target">함정을 발동시킨 대상</param>
    protected virtual void OnTrapActivate(GameObject target) 
    { 
    }
}
