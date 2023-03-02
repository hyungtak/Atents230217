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

    protected override IEnumerator Spawn()
    {
        while (true)     // 무한 반복(무한루프)
        {
            // 생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Asteroid);

            Asteroid asteroid = obj.GetComponent<Asteroid>();   // 생성한 게임오브젝트에서 asteroid 컴포넌트 가져오기
            asteroid.TargetPlayer = player;                     // asteroid에 플레이어 설정

            asteroid.transform.position = transform.position;   // 스포너 위치로 이동
            float r = Random.Range(minY, maxY);                 // 랜덤하게 적용할 기준 높이 구하고
            asteroid.transform.Translate(r * Vector3.up);

            Vector3 destPos = destination.position;             // 목적지 중심지 저장
            destPos.y = Random.Range(minY, maxY);               // 목적지의 y값만 랜덤으로 조정

            // 방향만 남기기 위해 normalize
            asteroid.Direction = (destPos - asteroid.transform.position).normalized;    

            yield return new WaitForSeconds(interval);  // 인터벌만큼 대기
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
