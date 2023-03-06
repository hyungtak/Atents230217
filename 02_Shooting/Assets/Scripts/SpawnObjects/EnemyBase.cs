using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : PoolObject
{
    [Header("적 기본 데이터 --------------")]
    /// <summary>
    /// 적의 최대 HP
    /// </summary>
    public int maxHitPoint = 1;

    /// <summary>
    /// 실제 이동 속도(초당 이동 거리)
    /// </summary>
    public float moveSpeed = 1.0f;
    
    /// <summary>
    /// 파괴했을 때의 점수
    /// </summary>
    public int score = 10;

    /// <summary>
    /// 파괴 이팩트
    /// </summary>
    public PoolObjectType destroyEffect = PoolObjectType.Explosion;

    /// <summary>
    /// 적의 HP
    /// </summary>
    int hitPoint = 1;
    
    /// <summary>
    /// 파괴되었는지 여부. true면 파괴된 상황, false면 멀쩡한 상황
    /// </summary>
    bool isCrushed = false;

    /// <summary>
    /// 플레이어에 대한 참조
    /// </summary>
    Player player = null;

    /// <summary>
    /// player에 처음 한번만 값을 설정 가능한 프로퍼티. 쓰기 전용. 자신과 상속받은 클래스에서는 읽기도 가능
    /// </summary>
    public Player TargetPlayer
    {
        protected get => player;
        set
        {
            if (player == null)     // player가 null일때만 설정
            {
                player = value;
            }
        }
    }

    protected virtual void OnEnable()
    {
        // 정상이 된 것 표시
        isCrushed = false;

        // HP 최대치로 채우기
        hitPoint = maxHitPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Attacked();
        }
    }

    /// <summary>
    /// 공격 당하면 무조건 실행해야하는 일들
    /// </summary>
    protected void Attacked()
    {
        hitPoint--;         // 맞으면 hitPoint 감소
        OnHit();
        if (hitPoint < 1)   // hitPoint가 0아래로 내려가면
        {
            Crush();        // 파괴
        }
    }

    /// <summary>
    /// 공격 당했을 때 상속받은 클래스별로 해야하는 일들
    /// </summary>
    protected virtual void OnHit()
    {
    }

    /// <summary>
    /// 부서지면 무조건 실행해야 할 일들 처리
    /// </summary>
    protected void Crush()
    {
        if (!isCrushed)
        {
            isCrushed = true;               // 파괴 되었다고 표시           

            OnCrush();                      // 클래스별 별도 파괴 처리

            GameObject obj = Factory.Inst.GetObject(destroyEffect); // 터지는 이팩트 생성
            obj.transform.position = transform.position;    // 이팩트 위치 변경

            gameObject.SetActive(false);    // 비활성화 
        }
    }

    /// <summary>
    /// 부서질때 상속받은 클래스별로 따로 처리할 일들 override해서 쓰기
    /// </summary>
    protected virtual void OnCrush()
    {
        player?.AddScore(score);        // 점수 추가
    }
}
