using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorManual : DoorBase, IUseableObject
{
    /// <summary>
    /// 자동으로 닫힐때까지 걸리는 시간
    /// </summary>
    public float closeTime = 3.0f;

    /// <summary>
    /// 코루틴용으로 한번만 만들고 재활용하기 위한 용도
    /// </summary>
    WaitForSeconds closeWait;

    TextMeshPro text;

    public bool IsDirectUse => true;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMeshPro>();
    }

    void Start()
    {
        text.gameObject.SetActive(false);
        closeWait = new WaitForSeconds(closeTime); // 시작할 때 한번만 만들기 
    }

    /// <summary>
    /// 이 오브젝트가 실행되는 함수
    /// </summary>
    public void Used()
    {
        //Debug.Log("사용됨");
        Open();                         // 문 열기
        StartCoroutine(AutoClose());    // closeTime초 이후에 자동으로 닫히게 하기
    }

    IEnumerator AutoClose()
    {
        yield return closeWait; // closeTime초 만큼 대기하고
        Close();                // 문 닫기
    }

    private void OnTriggerEnter(Collider other)
    {
        text.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        text.gameObject.SetActive(false);        
    }
}
