using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PoolObject
{
    public float moveSpeed = 0.5f;

    Vector2 dir;

    private void OnEnable()
    {
        SetRandomDirection();
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);
    }

    void SetRandomDirection(bool allRandom = true)
    {
        if( allRandom)
        {
            dir = Random.insideUnitCircle;  // 반지름이 1인 원 안의 랜덤한 위치 가져오기
            dir = dir.normalized;           // 길이를 1로 만들어서 항상 동일한 속도가 되게 만들기
        }
    }
}
