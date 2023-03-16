using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    /// <summary>
    /// 시작 이동 속도
    /// </summary>
    public float initialSpeed = 20.0f;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()    
    {
        rigid.velocity = initialSpeed * transform.forward;  // 초기 운동량 결정
        StartCoroutine(LifeOver(10.0f));    // 시작하고 10초 뒤 비활성화
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(LifeOver(2.0f));    // 부딪치면 2초 뒤 비활성화
    }
}
