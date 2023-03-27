using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAuto : Platform
{
    /// <summary>
    /// 플랫폼이 현재 움직임 여부를 설정하는 변수. true면 움직이고 false면 안움직인다.
    /// </summary>
    bool isMoving = true;

    private void FixedUpdate()
    {
        if (isMoving)   // isMoving이 true일 때만 움직임
        {
            Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))  // 플레이어가 바닥 트리거에 들어오면 움직임
        {
            isMoving = true;
        }
    }

    /// <summary>
    /// 도착 지점에 도달하면 실행되는 함수
    /// </summary>
    protected override void OnArrived()
    {
        base.OnArrived();
        isMoving = false;   // 도착하면 움직임 멈춤
    }
}
