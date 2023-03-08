using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAsteroid : Spawner
{
    /// <summary>
    /// 목적이 영역의 중심 트랜스폼
    /// </summary>
    Transform destination;

    private void Awake()
    {
        destination = transform.GetChild(0);    // 첫번째 자식 가져오기
    }

    protected override void OnSpawn(EnemyBase enemy)
    {
        base.OnSpawn(enemy);

        Vector3 destPos = destination.position;             // 목적지 중심지 저장
        destPos.y = Random.Range(minY, maxY);               // 목적지의 y값만 랜덤으로 조정

        Asteroid asteroid = enemy as Asteroid;              // enemy가 Asteroid 타입이 맞으면 캐스팅
        if( asteroid != null)
        {
            // 방향만 남기기 위해 normalize
            asteroid.Direction = (destPos - enemy.transform.position).normalized;
        }
        else
        {
            Debug.LogError("SpawnerAsteroid : 운석이 아닌데 스폰하려고 함");
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 목적지 영역을 큐브로 그리기
        Gizmos.color = Color.blue;
        if(destination == null)    // destination이 자식 transform이기 때문에 플레이전에는 없음
        {
            destination = transform.GetChild(0);    // 플레이 전인 상황이라면 찾아서 넣기
        }
        Gizmos.DrawWireCube(destination.position,
            new Vector3(1, Mathf.Abs(maxY) + Mathf.Abs(minY) + 2, 1));  // 큐브 그리기
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // 스폰 지점을 선으로 긋기
        Gizmos.color = Color.red;
        if (destination == null)
        {
            destination = transform.GetChild(0);
        }
        Vector3 from = destination.position + Vector3.up * minY;
        Vector3 to = destination.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);
    }
}
