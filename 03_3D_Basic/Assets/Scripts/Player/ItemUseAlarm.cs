using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseAlarm : MonoBehaviour
{
    /// <summary>
    /// 사용 가능한 아이템을 사용 시도한다는 알람용 델리게이트
    /// </summary>
    public Action<IUseableObject> onUseableItemUsed;

    /// <summary>
    /// 애니메이션에서 트리거를 껐다켰다 하는데 켜진 시점에서 컬라이더가 들어오면 실행
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);

        // 충돌한 컬라이더가 내가 원하는 스크립트가 있는 root가 아닐 수 있음
        Transform target = other.transform; 
        while(target.parent != null)
        {
            target = target.parent;     // 스크립트가 있는 최상단 부모가 나올 때까지 계속 부모타고 올라가기
        }

        // 사용 가능한 아이템인지 확인(IUseableObject 인터페이스가 있다. == 사용 가능한 오브젝트이다)
        IUseableObject obj = target.GetComponent<IUseableObject>();
        if( obj != null )
        {
            onUseableItemUsed?.Invoke( obj );  // 사용 가능한 오브젝트이니까 알람을 보냄
        }        
    }
}
