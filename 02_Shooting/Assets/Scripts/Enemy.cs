using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    public float amplitude = 1; // 사인 결과값을 증폭시킬 변수(위아래 차이 결정)
    public float frequency = 1; // 사인 그래프가 한번 도는데 걸리는 시간(가로 폭 결정)

    float timeElapsed = 0.0f;
    float baseY;

    private void Start()
    {
        baseY = transform.position.y;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * frequency;  // frequency에 비례해서 시간 증가가 빠르게 된다
        float x = transform.position.x - speed * Time.deltaTime;    // x는 현재 위치에서 약간 왼쪽으로 이동
        float y = baseY + Mathf.Sin(timeElapsed) * amplitude;       // y는 시작위치에서 sin 결과값만큼 변경

        transform.position = new Vector3(x, y, 0);  // 구한 x,y를 이용해 높이 새로 지정
    }
}
