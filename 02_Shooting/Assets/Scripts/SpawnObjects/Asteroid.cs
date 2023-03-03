using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : AsteroidBase
{
    [Header("큰 운석 데이터 --------------")]

    /// <summary>
    /// 파괴 될 때 생성할 오브젝트의 종류
    /// </summary>
    public PoolObjectType childType = PoolObjectType.AsteroidSmall;

    /// <summary>
    /// 파괴 될 때 생성할 오브젝트의 갯수
    /// </summary>
    public int splitCount = 3;

    protected override void OnCrush()
    {
        base.OnCrush();

        splitCount = Random.Range(3, 8);    // 3~7개 생성

        float angleGap = 360.0f / splitCount;       // 작은 운석간의 사이각 계산
        float seed = Random.Range(0.0f, 360.0f);    // 처음 적용할 오차 랜덤으로 구하기

        for(int i=0;i<splitCount; i++)              // splitCount만큼 반복
        {
            GameObject obj = Factory.Inst.GetObject(childType);     // 작은 운석 생성
            obj.transform.position = transform.position;            // 위치는 우선 큰 운석 위치로
            AsteroidBase small = obj.GetComponent<AsteroidBase>();

            // Up(0,1,0) 벡터를 일단 z축을 기준으로 seed만큼 회전시키고
            // 추가로 angleGap * i만큼 더 회전 시키고
            // small의 방향으로 지정하는 코드
            small.Direction = Quaternion.Euler(0, 0, seed + angleGap * i ) * Vector3.up;    // 작은 운석의 방향지정하기
        }
    }

}
