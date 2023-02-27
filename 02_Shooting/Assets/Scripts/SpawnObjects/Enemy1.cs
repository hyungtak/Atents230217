using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float speed = 5.0f;

    float baseY;
    float dir = 1.0f;
    public float height = 3.0f;

    private void Start()
    {
        baseY = transform.position.y;   // 시작할 때 시작한 높이 기록
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime* speed * -transform.right);      // 왼쪽으로 이동
        transform.Translate(Time.deltaTime * speed * dir * transform.up);   // 위아래로 이동

        // 이 게임오브젝트의 y위치가 일정 이상 올라가거나 내려가면 방향 변경
        if( (transform.position.y > baseY+height) || (transform.position.y < baseY - height) )
        {
            dir *= -1.0f;       // dir = dir * -1;
        }

        // 논리 연산자
        // && (and라고 읽음) : 양 변이 모두 true일때만 true이다.
        //   true && false는 false
        // || (or라고 읽음) : 양 변 중 하나만 true면 true이다.
        //   true || false는 true
    }
}
