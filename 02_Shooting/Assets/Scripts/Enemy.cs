using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject
{
    /// <summary>
    /// 적 이동 속도
    /// </summary>
    public float speed = 1.0f;

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
    /// 적이 터지는 이팩트
    /// </summary>
    public GameObject explosionPrefab;

    /// <summary>
    /// 이 적이 죽을때 플레이어에게 주는 점수
    /// </summary>
    public int score = 10;

    /// <summary>
    /// 누적 시간(사인 계산용)
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// 처음 등장한 위치
    /// </summary>
    float baseY;

    /// <summary>
    /// 살아있는지 여부를 나타내는 플래그(flag). true면 살아있고 false면 죽어있다.
    /// </summary>
    bool isAlive = true;

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

    private void OnEnable()
    {
        baseY = transform.position.y;   // 시작할 때 등장한 위치 저장        
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * frequency;  // frequency에 비례해서 시간 증가가 빠르게 된다
        float x = transform.position.x - speed * Time.deltaTime;    // x는 현재 위치에서 약간 왼쪽으로 이동
        float y = baseY + Mathf.Sin(timeElapsed) * amplitude;       // y는 시작위치에서 sin 결과값만큼 변경

        transform.position = new Vector3(x, y, 0);  // 구한 x,y를 이용해 높이 새로 지정
    }

    /// <summary>
    /// 충돌 했을 때 실행되는 이벤트 함수
    /// </summary>
    /// <param name="collision">충돌 정보</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))   // Bullet 태그를 가진 오브젝트와 충돌 했을 때만 실행
        {
            Die();
        }
    }

    /// <summary>
    /// 죽었을 때 실행되는 함수
    /// </summary>
    void Die()
    {
        if( isAlive )   // 살아있을 때만 죽이기
        {
            isAlive = false;    // 죽었다고 표시

            //GameObject player = GameObject.Find("Player");                    // 이름으로 찾기
            //GameObject player = GameObject.FindGameObjectWithTag("Player");   // 태그로 찾기
            //Player player = FindObjectOfType<Player>();                       // 타입으로 찾기

            player.AddScore(score);                         // 플레이어에게 점수 추가

            GameObject obj = Instantiate(explosionPrefab);  // 폭발 이팩트 생성
            obj.transform.position = transform.position;    // 위치는 적의 위치로 설정
            gameObject.SetActive(false);                    // 적 풀로 되돌리기
        }
    }
}
