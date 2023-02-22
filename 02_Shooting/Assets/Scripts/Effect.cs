using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트를 가지는 게임 오브젝트는 반드시 Animator를 가지게 되어있다.
[RequireComponent(typeof(Animator))]
public class Effect : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Destroy(gameObject, anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }
}
