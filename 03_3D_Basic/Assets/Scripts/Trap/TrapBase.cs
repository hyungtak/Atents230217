using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TrapActivate(other.gameObject);
        }
    }

    private void TrapActivate(GameObject target)
    {
        OnTrapActivate(target);
    }

    /// <summary>
    /// TrapBase를 상속 받은 함정마다 오버라이드할 함수
    /// </summary>
    /// <param name="target">함정을 밟은 대상</param>
    protected virtual void OnTrapActivate(GameObject target) 
    { 
    }
}
