using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트를 가지는 게임 오브젝트는 반드시 Animator를 가지게 되어있다.
[RequireComponent(typeof(Animator))]
public class Effect : PoolObject
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }
}
