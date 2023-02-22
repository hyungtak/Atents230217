using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 오브젝트를 주기적으로 생성할 클래스
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// 생성할 게임 오브젝트
    /// </summary>
    public GameObject spawnPrefab;

    /// <summary>
    /// 생성할 위치(최소값)
    /// </summary>
    public float minY = -4;

    /// <summary>
    /// 생성할 위치(최대값)
    /// </summary>
    public float maxY = 4;

    /// <summary>
    /// 생성 시간 간격
    /// </summary>
    public float interval = 1.0f;

    //WaitForSeconds wait;

    private void Start()
    {
        //wait = new WaitForSeconds(interval);  // 게임 실행 도중에 interval이 변하지 않는다면 미리 만들어 두는 것이 좋다.
                
        StartCoroutine(Spawn());    // 시작할 때 Spawn 코루틴 시작
    }

    /// <summary>
    /// 오브젝트를 주기적으로 생성하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn()
    {
        while(true)     // 무한 반복(무한루프)
        {
            // 생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Instantiate(spawnPrefab, transform);
            float r = Random.Range(minY, maxY);         // 랜덤하게 적용할 높이 구하고
            obj.transform.Translate(Vector3.up * r);    // 높이 적용

            //yield return wait;
            yield return new WaitForSeconds(interval);  // 인터벌만큼 대기
        }
    }

    /// <summary>
    /// 씬창에 개발용 정보를 그리는 함수
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // Gizmos.color = new Color(0, 1, 0); // rgb값으로 색상을 만들 수도 있다.
        
        // 스폰 지점을 선으로 긋기
        Vector3 from = transform.position + Vector3.up * minY;
        Vector3 to = transform.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);

        Gizmos.DrawWireCube(transform.position, new Vector3(1,8,1));
    }
}
