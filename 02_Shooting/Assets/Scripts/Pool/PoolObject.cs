using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀이 사용할 게임 오브젝트
/// </summary>
public class PoolObject : MonoBehaviour
{
    /// <summary>
    /// 이 게임 오브젝트가 비활성화 될 때 실행되는 델리게이트
    /// </summary>
    public Action onDisable;

    /// <summary>
    /// 게임 오브젝트가 비활성화 될 때 실행
    /// </summary>
    protected virtual void OnDisable()
    {
        onDisable?.Invoke();    // 이 델리게이트에 등록된 함수들 실행
    }
}
