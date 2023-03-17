using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    /// <summary>
    /// 총알 발사 시간 간격
    /// </summary>
    public float fireInterval;

    /// <summary>
    /// 총알이 발사되는 트랜스폼
    /// </summary>
    protected Transform fireTransform;

    /// <summary>
    /// 총몸의 트랜스폼
    /// </summary>
    protected Transform barrelBodyTransform;

    /// <summary>
    /// 총알을 계속 발사하는 코루틴
    /// </summary>
    protected IEnumerator fireCoroutine;

    /// <summary>
    /// 총알 발사 시간 간격만큼 기다리는 WaitForSeconds
    /// </summary>
    WaitForSeconds waitFireInterval;

    protected virtual void Awake()
    {
        barrelBodyTransform = transform.GetChild(2);        // BarrelBody
        fireTransform = barrelBodyTransform.GetChild(1);    // fireTransform
        fireCoroutine = PeriodFire();
    }

    protected virtual void Start()
    {
        waitFireInterval = new WaitForSeconds(fireInterval);
    }

    /// <summary>
    /// 주기적으로 총알을 발사하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator PeriodFire()
    {
        while(true)
        {
            OnFire();
            yield return waitFireInterval;
        }
    }

    /// <summary>
    /// 총알을 한발 발사하는 함수.
    /// </summary>
    protected virtual void OnFire()
    {
    }
}
