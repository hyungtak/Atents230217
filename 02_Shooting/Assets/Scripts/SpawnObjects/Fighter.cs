using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : EnemyBase
{
    [Header("비행기 데이터 ---------------")]
    /// <summary>
    /// 적 위아래 이동 정도(비율)
    /// </summary>
    [Range(0.1f, 3.0f)]         // 변수 범위를 (min,max)사이로 변경시키는 슬라이더 추가
    public float amplitude = 1; // 사인 결과값을 증폭시킬 변수(위아래 차이 결정)

    /// <summary>
    /// 위아래로 한번 움직이는데 걸리는 거리(비율)
    /// </summary>
    public float frequency = 1; // 사인 그래프가 한번 도는데 걸리는 시간(가로 폭 결정)
            
    /// <summary>
    /// 누적 시간(사인 계산용)
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// 처음 등장한 위치
    /// </summary>
    float baseY;

    /// <summary>
    /// 적이 위아래로 움직이는 기준 위치 설정하는 프로퍼티(월드 기준, 쓰기 전용)
    /// </summary>
    public float BaseY
    {
        set => baseY = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        transform.localPosition = Vector3.zero; // 새로 꺼낼때 위치 초기화
        baseY = 0.0f;                           // 기본 높이 설정
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * frequency;          // frequency에 비례해서 시간 증가가 빠르게 된다
        float x = transform.position.x - moveSpeed * Time.deltaTime;    // x는 현재 위치에서 약간 왼쪽으로 이동
        float y = baseY + Mathf.Sin(timeElapsed) * amplitude;           // y는 시작위치에서 sin 결과값만큼 변경

        transform.position = new Vector3(x, y, 0);          // 구한 x,y를 이용해 높이 새로 지정
    }
}
