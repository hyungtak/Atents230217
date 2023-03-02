using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAsteroid : Spawner
{
    Transform destination;

    private void Awake()
    {
        destination = transform.GetChild(0);
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

            yield return new WaitForSeconds(interval);  // 인터벌만큼 대기
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 도착 영역을 큐브로 그리기
        Gizmos.color = Color.blue;
        if(destination == null )
        {
            destination = transform.GetChild(0);
        }
        Gizmos.DrawWireCube(destination.position,
            new Vector3(1, Mathf.Abs(maxY) + Mathf.Abs(minY) + 2, 1));
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
