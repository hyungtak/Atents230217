using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float speed = 5.0f;

    public float bounceTime = 1.0f;
    float currentTime = 0.0f;

    Vector2 dir;

    private void Start()
    {
        dir = new Vector2(-1, 1);
        dir.Normalize();            // 방향만 남긴다.(=길이가 1이다.)
    }

    private void Update()
    {
        currentTime += Time.deltaTime;  // 시간 계속 누적하기
        if(currentTime>bounceTime)      // 지정된 시간이 지나면
        {
            currentTime = 0.0f;         // 시간 초기화하고
            dir.y = -dir.y;             // 방향 뒤집기
        }

        transform.Translate(Time.deltaTime * speed * dir);
    }
}
