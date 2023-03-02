using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : Spawner
{    
    /// <summary>
    /// 오브젝트를 주기적으로 생성하는 코루틴
    /// </summary>
    /// <returns></returns>
    override protected IEnumerator Spawn()
    {
        while (true)     // 무한 반복(무한루프)
        {
            // 생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Enemy);

            Enemy enemy = obj.GetComponent<Enemy>();    // 생성한 게임오브젝트에서 Enemy 컴포넌트 가져오기
            enemy.TargetPlayer = player;                // Enemy에 플레이어 설정

            enemy.transform.position = transform.position;  // 스포너 위치로 이동
            float r = Random.Range(minY, maxY);             // 랜덤하게 적용할 기준 높이 구하고
            enemy.BaseY = transform.position.y + r;         // 기준 높이 적용

            //yield return wait;
            yield return new WaitForSeconds(interval);  // 인터벌만큼 대기
        }
    }
}
