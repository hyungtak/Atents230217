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
    /// 최소 수명
    /// </summary>
    public float minLifeTime = 4.0f;

    /// <summary>
    /// 최대 수명
    /// </summary>
    public float maxLifeTime = 7.0f;

    /// <summary>
    /// 크리티컬이 터질 확률
    /// </summary>
    [Range(0f, 1f)]
    public float criticalChance = 0.5f;

    /// <summary>
    /// 크리티컬이 터졌을 때 나올 작은 운석 갯수
    /// </summary>
    public int criticalSplitCount = 20;

    /// <summary>
    /// 파괴 될 때 생성할 오브젝트의 갯수
    /// </summary>
    int splitCount = 3;

    /// <summary>
    /// 자폭 여부 표시용 변수. true면 자폭한것, false면 플레이어가 터트린 것
    /// </summary>
    bool isSelfCrush = false;

    /// <summary>
    /// 1초 대기용. 자주 사용하므로 미리 만들어 놓기
    /// </summary>
    readonly WaitForSeconds oneSecond = new WaitForSeconds(1);

    // 찾아놓은 컴포넌트
    Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        isSelfCrush = false;
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);

        StopAllCoroutines();                    // 이전 코루틴 제거
        StartCoroutine(SelfCrush(lifeTime));    // 새 자폭 코루틴 시작
    }

    /// <summary>
    /// 자폭용 코루틴
    /// </summary>
    /// <param name="lifeTime">자폭 대기 시간</param>
    /// <returns></returns>
    IEnumerator SelfCrush(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime-1);    // 1초 전까지 대기
        anim.SetTrigger("SelfCrush");                   // 트리거 발동 시키고
        yield return oneSecond;                         // 1초 대기
        isSelfCrush = true;
        Crush();
    }

    protected override void OnCrush()
    {
        if(!isSelfCrush)                
        {
            TargetPlayer?.AddScore(score);  // 자폭이 아닐 때만 점수 추가
        }

        //float random = Random.Range(0.0f, 1.0f);  // 0~1 사이의 값을 받기(0이면 0%, 1이면 100%)
        if( Random.value < criticalChance )         // 정해진 확률 이하면 걸린 것으로 처리
        {
            splitCount = criticalSplitCount;        // 5%를 뚫으면 20개 생성
        }
        else
        {
            splitCount = Random.Range(3, 8);        // 아니면 3~7개 생성
        }

        float angleGap = 360.0f / splitCount;       // 작은 운석간의 사이각 계산
        float seed = Random.Range(0.0f, 360.0f);    // 처음 적용할 오차 랜덤으로 구하기

        for(int i=0;i<splitCount; i++)              // splitCount만큼 반복
        {
            GameObject obj = Factory.Inst.GetObject(childType);     // 작은 운석 생성
            obj.transform.position = transform.position;            // 위치는 우선 큰 운석 위치로
            AsteroidBase small = obj.GetComponent<AsteroidBase>();
            small.TargetPlayer = TargetPlayer;                      // 점수 추가를 위해 플레이어 설정

            // Up(0,1,0) 벡터를 일단 z축을 기준으로 seed만큼 회전시키고
            // 추가로 angleGap * i만큼 더 회전 시키고
            // small의 방향으로 지정하는 코드
            small.Direction = Quaternion.Euler(0, 0, seed + angleGap * i ) * Vector3.up;    // 작은 운석의 방향지정하기
        }
    }

}
