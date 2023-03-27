using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSwitch : MonoBehaviour, IUseableObject
{
    /// <summary>
    /// 사용할 오브젝트
    /// </summary>
    public GameObject target;

    /// <summary>
    /// 사용할 오브젝트가 가지고 있는 IUseableObject 인터페이스
    /// </summary>
    IUseableObject useTarget;

    /// <summary>
    /// 사용중인지 표시하는 플래그
    /// </summary>
    bool isUsed = false;

    Animator anim;

    public bool IsDirectUse => true;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        useTarget = target.GetComponent<IUseableObject>();  // 미리 찾아놓는 용도
        if (useTarget == null)
        {
            Debug.LogWarning("사용할 수 없는 오브젝트가 설정되어 있습니다.");
        }
    }

    /// <summary>
    /// 이 오브젝트가 사용될 때 실행되는 함수
    /// </summary>
    public void Used()
    {
        if (useTarget != null)  // 사용할 대상이 있고
        {
            if (!isUsed)        // 사용중이 아니면
            {
                useTarget.Used();               // 대상을 사용하기
                StartCoroutine(ResetSwitch());  // 코루틴으로 애니메이션 처리
            }
        }
    }

    IEnumerator ResetSwitch()
    {
        isUsed = true;                          // 사용중이라고 표시
        anim.SetBool("IsOpen", true);           // 사용하는 애니메이션 실행
        yield return new WaitForSeconds(1);     // 1초 기다리기
        anim.SetBool("IsOpen", false);          // 원상태로 되돌리는 애니메이션 실행
        isUsed = false;                         // 사용이 끝났다고 표시
    }
}
