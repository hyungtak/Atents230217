using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManual : Platform, IUseableObject
{
    /// <summary>
    /// 플랫폼이 현재 움직임 여부를 설정하는 변수. true면 움직이고 false면 안움직인다.
    /// </summary>
    bool isMoving = false;

    /// <summary>
    /// private 프로퍼티. 내부에서 isMoving이 변경될 때 실행될 함수 추가 실행
    /// </summary>
    bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;
            if(isMoving)
            {
                ActivatePlatform();
            }
            else
            {
                DeactivatePlatform();
            }
        }
    }

    /// <summary>
    /// 직접 사용 금지
    /// </summary>
    public bool IsDirectUse => false;

    private void FixedUpdate()
    {
        if (isMoving)   // isMoving이 true일 때만 실행
        {
            Move();
        }
    }

    /// <summary>
    /// 움직이기 시작할 때 실행될 함수
    /// </summary>
    void ActivatePlatform()
    {
    }

    /// <summary>
    /// 정지 시킬 때 실행될 함수
    /// </summary>
    void DeactivatePlatform()
    {
    }

    /// <summary>
    /// 아이템을 사용하면 실행되는 함수
    /// </summary>
    public void Used()
    {
        IsMoving = !IsMoving;   // isMoving만 반대로 변경
    }
}
