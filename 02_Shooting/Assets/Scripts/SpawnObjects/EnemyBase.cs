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
    /// 파괴되었는지 여부
    /// </summary>
    bool isCrushed = true;

    /// <summary>
    /// 플레이어에 대한 참조
    /// </summary>
    Player player = null;

    /// <summary>
    /// player에 처음 한번만 값을 설정 가능한 프로퍼티. 쓰기 전용.
    /// </summary>
    public Player TargetPlayer
    {
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
        // 다시 살아난 것 표시
        isCrushed = true;

        // HP 최대치로 채우기
        hitPoint = maxHitPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnHit();
        }
    }

    protected virtual void OnHit()
    {
        hitPoint--;         // 맞으면 hitPoint 감소
        if (hitPoint < 1)   // hitPoint가 0아래로 내려가면
        {
            OnCrush();      // 파괴
        }
    }

    protected virtual void OnCrush()
    {
        if (isCrushed)
        {
            isCrushed = false;      // 파괴 되었다고 표시

            player.AddScore(score); // 점수 추가

            GameObject obj = Factory.Inst.GetObject(destroyEffect); // 터지는 이팩트 생성
            obj.transform.position = transform.position;    // 이팩트 위치 변경

            gameObject.SetActive(false);    // 비활성화
        }
    }
}
