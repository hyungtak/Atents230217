using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;

    private void Start()
    {
        Destroy(gameObject, 5.0f);      // 5초 뒤에 이 스크립트가 들어있는 게임오브젝트를 삭제해라
    }

    private void Update()
    {
        // 초당 speed의 속도로 오른쪽방향으로 이동(로컬 좌표를 기준으로 한 방향)
        //transform.Translate(Time.deltaTime * speed * Vector2.right); 
        //transform.Translate(Time.deltaTime * speed * transform.right, Space.World); 
        transform.position += Time.deltaTime * speed * transform.right;

        // local좌표와 world좌표
        // local좌표 : 각 오브젝트 별 기준으로 한 좌표계
        // world좌표 : 맵을 기준으로 한 좌표계
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("Enemy") )  // 부딪친 게임오브젝트의 태그가 "Enemy"일때만 처리
        {
            Debug.Log($"총알이 {collision.gameObject.name}과 충돌");
            // collision.contacts[0].point : 충돌지점

            //if(collision.gameObject.tag == "Enemy")   // 절대로 하지 말것. 더 느리고 메모리도 많이 쓴다.
        }
    }
}
