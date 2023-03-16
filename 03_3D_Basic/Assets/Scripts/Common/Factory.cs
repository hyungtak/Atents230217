using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 생성해주는 클래스
/// </summary>
public class Factory : Singleton<Factory>
{
    // 생성할 오브젝트의 풀들
    BulletPool bulletPool;

    /// <summary>
    /// 이 싱글톤이 만들어질 때 처음 한번만 호출될 함수
    /// </summary>
    protected override void PreInitialize()
    {
        // 자식으로 붙어있는 풀들 다 찾아놓기
        bulletPool = GetComponentInChildren<BulletPool>();        
    }

    /// <summary>
    /// 씬이 로드될 때마다 호출되는 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        bulletPool?.Initialize();       // ?.은 null이 아니면 실행, null이면 아무것도 하지 않는다.        
    }

    /// <summary>
    /// Bullet풀에서 Bullet하나 꺼내는 함수
    /// </summary>
    /// <param name="parentT">기준 트랜스폼(이 트랜스폼의 위치, 회전, 스케일 사용)</param>
    /// <returns></returns>
    public Bullet GetBullet(Transform parentT = null) => bulletPool?.GetObject(parentT);

}
